using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class ConvoyManager
{
    private PetSkillMaker _petSkillMaker;
    private string _selectedQuestId;
    private List<string> _selectedPetIds = new();

    private TradeRouteHandler _loadedMap;
    private Wagon _wagon;
    private List<GameObject> _petList = new();

    private PlayerEntity _playerEntity;
    private PlayerSkillMaker _playerSkillMaker;

    private CameraHandler _cameraHandler;

    private const string PLAYER_ID = "player_scholar"; // 현재 플레이어 선택 불가능하여, 학자가 고정되어 있음.

    private bool _isConvoyEnded;

    private int _convoyStartSoul;

    public ConvoyManager(PetSkillMaker petSkillMaker, PlayerSkillMaker playerSkillMaker)
    {
        _petSkillMaker = petSkillMaker;
        _playerSkillMaker = playerSkillMaker;
    }

    // 의뢰 시작 -> 펫 선택 -> 로딩 UI를 보여주며 아래 init 함수 호출.
    public async UniTask InitConvoy(string questId, List<string> selectedPetIds)
    {
        _selectedQuestId = questId;
        _convoyStartSoul = GameManager.Network.RequestPlayerOutGameViewModel().GetSoul;

        _selectedPetIds.Clear();
        _selectedPetIds.AddRange(selectedPetIds);

        await StartConvoyAsync();
    }

    public void FaildConvoy(ConvoyFailReason failReason = ConvoyFailReason.WagonDestroyed)
    {
        if (_isConvoyEnded == true)
            return;

        _isConvoyEnded = true;

        GameManager.UI.CloseAllConvoyHuds();
        var result = MakeConvoyResult(false, failReason);
        GameManager.UI.OpenConvoyFailUI(result);
    }

    public void SuccessConvoy()
    {
        if (_isConvoyEnded == true)
            return;

        _isConvoyEnded = true;

        GameManager.UI.CloseAllConvoyHuds();
        var result = MakeConvoyResult(true, ConvoyFailReason.None);
        GameManager.UI.OpenConvoySuccessUI(result);
    }

    // TODO(이태영): 집계 데이터 연동 필요 - 몬스터 처치 수, 소요 시간, 보상 계산은 현재 테스트
    private ConvoyResultModel MakeConvoyResult(bool isSuccess, ConvoyFailReason failReason)
    {
        var result = new ConvoyResultModel();

        result.IsSuccess = isSuccess;
        result.FailReason = failReason;

        result.ClearTime = GetConvoyClearTime();
        result.GainedSoul = GetGainedSoul();

        // TODO(이태영): 처치 수는 EnemySpawner의 집계, 신기록은 세이브 데이터가 붙어야 채울 수 있음
        result.IsNewRecord = false;
        result.KilledMonsterCount = 0;

        var questData = GameManager.DataTable.GetQuestData(_selectedQuestId);

        if (isSuccess == true)
        {
            result.GoldReward = GetGoldReward(questData);
            result.ReputationReward = GetReputationReward(questData);
        }
        else
        {
            // TODO(이태영): 실패 페널티도 기획 데이터에서 가져오기
            result.ReputationPenalty = 5;
            result.RepairCost = 300;
            result.IsRepairCostPaid = true;
            result.ExtraReputationPenalty = 3;
        }

        ApplyReputation(result);
        ApplyGold(result);

        // TODO(이태영): Release()에서 반환하는 마을 ID와 연동 필요
        result.ReturnTownId = "town_lavendil";

        return result;
    }

    private float GetConvoyClearTime()
    {
        var wagonViewModel = GameManager.Network.WagonService.GetWagonViewModel();

        if (wagonViewModel == null)
        {
            return 0f;
        }

        return wagonViewModel.GetTime;
    }

    private int GetGainedSoul()
    {
        int currentSoul = GameManager.Network.RequestPlayerOutGameViewModel().GetSoul;

        return Mathf.Max(0, currentSoul - _convoyStartSoul);
    }

    private int GetReputationReward(QuestData questData)
    {
        if (questData == null)
        {
            return 0;
        }

        return questData.ReputationReward;
    }

    private int GetGoldReward(QuestData questData)
    {
        if (questData == null)
        {
            return 0;
        }

        return questData.GoldReward;
    }

    private void ApplyGold(ConvoyResultModel result)
    {
        var outGameViewModel = GameManager.Network.RequestPlayerOutGameViewModel();

        if (result.IsSuccess == true)
        {
            outGameViewModel.AddGold(result.GoldReward);
            return;
        }

        if (result.IsRepairCostPaid == false)
        {
            return;
        }

        outGameViewModel.ReduceGold(result.RepairCost);
    }

    private void ApplyReputation(ConvoyResultModel result)
    {
        var outGameViewModel = GameManager.Network.RequestPlayerOutGameViewModel();

        if (result.IsSuccess == true)
        {
            outGameViewModel.AddReputation(result.ReputationReward);
            return;
        }

        int penalty = result.ReputationPenalty;

        if (result.IsRepairCostPaid == false)
        {
            penalty += result.ExtraReputationPenalty;
        }

        outGameViewModel.AddReputation(-penalty);
    }

    public string Release()
    {

        // 1. 카메라 해제
        GameManager.Camera.Release();

        // 2. 펫 해제
        ReleasePet();

        // 3. 플레이어 해제
        ReleasePlayer();

        // 4. 마차 해제
        ReleaseWagon();

        // 5. 맵 해제
        GameObject.Destroy(_loadedMap.gameObject);


        GameManager.Pool.AllDespawnToPool();

        // TODO 결과에 따라 실패 시 의뢰 출발 마을 ID, 성공 시 도착 마을 ID 반환
        return "테스트 ID";
    }

    private async UniTask StartConvoyAsync()
    {
        LoadMap();
        SpawnPet();
        InitCamera();

        StartBattle();

        await UniTask.NextFrame();
    }

    private void LoadMap()
    {
        _loadedMap = GameObject.Instantiate(Utils.ResourcesLoad<TradeRouteHandler>("Map/Map_01-TEST"));

        SpawnPlayer();

        SpawnWagon(_loadedMap.SplineContainer);
    }

    private void SpawnWagon(SplineContainer splineContainer)
    {
        var wagonViewModel = GameManager.Network.RequestCreateWagon();

        _wagon = GameObject.Instantiate(Utils.ResourcesLoad<Wagon>("Wagon/Wagon_ProtoType"));
        _wagon.Init(wagonViewModel, _playerEntity, _selectedQuestId);
        _wagon.SetSpline(splineContainer);
    }

    private void SpawnPlayer()
    {
        var inGameViewModel = GameManager.Network.RequestCreatePlayer();
        var playerStatController = GameManager.Network.PlayerService.StatController;

        _playerEntity = GameObject.Instantiate(Utils.ResourcesLoad<PlayerEntity>("Test_Scholar"));

        _playerEntity.Init(PLAYER_ID, inGameViewModel, playerStatController);
    }

    private void SpawnPet()
    {
        List<PetController> petControllers = new();

        var petViewModels = GameManager.Network.PetService.GetPetViewModels(_selectedPetIds);

        for (int index = 0; index < _selectedPetIds.Count; index++)
        {
            var petOB = GameManager.Resource.GetLoadedAsset<GameObject>(_selectedPetIds[index]);
            GameObject petInstance = GameObject.Instantiate(petOB);
            _petList.Add(petInstance);

            petInstance.GetComponent<PetController>().Init(_selectedPetIds[index]
                            , _playerEntity, _wagon
                            , _petSkillMaker
                            , _playerEntity, _playerEntity
                            , 30 + index * 10
                            , petViewModels[index]);

            petControllers.Add(petInstance.GetComponent<PetController>());
        }

        GameManager.PetParty.Init(petControllers);

        _playerEntity.InitAfterSpawnPet(PLAYER_ID, _playerSkillMaker, GameManager.PetParty.GetStatusEffectReceiverForAllPet());
    }

    private void InitCamera()
    {
        GameManager.Camera.InitInGame(_playerEntity.transform);
    }


    private void StartBattle()
    {
        _isConvoyEnded = false;
        OpenConvoyHuds();
    }

    private void OpenConvoyHuds()
    {

        var wagonVm = GameManager.Network.WagonService.GetWagonViewModel();
        if (wagonVm != null)
        {
            GameManager.UI.OpenConvoyHudUI(wagonVm, _selectedQuestId);
            GameManager.UI.OpenWagonAreaWarningUI(wagonVm);
        }

        var partyHud = GameManager.UI.OpenPartyHudUI();

        if (partyHud != null)
        {
            partyHud.BindWagon(wagonVm);

            var petViewModels = GameManager.Network.GetPetViewModels(_selectedPetIds);

            for (int i = 0; i < _selectedPetIds.Count; i++)
            {
                partyHud.BindPet(_selectedPetIds[i], petViewModels[i]);
            }
        }

        var playerVm = GameManager.Network.PlayerService.GetPlayerViewModel();
        if (playerVm != null)
        {
            GameManager.UI.OpenPlayerHudUI(playerVm);
        }

        var resourceModel = new ResourceModel();

        var resourceHud = GameManager.UI.OpenResourceHudUI(resourceModel);

        if (resourceHud != null)
        {
            resourceHud.SetConvoyLayout();
        }

        GameManager.UI.OpenSkillHudUI(PLAYER_ID);
    }

    private void ReleasePet()
    {
        GameManager.PetParty.Release();

        foreach (var pet in _petList)
        {
            GameObject.Destroy(pet);
        }

        _petList.Clear();
        _selectedPetIds.Clear();
    }

    private void ReleasePlayer()
    {
        if (_playerEntity == null)
            return;

        _playerEntity.Release();

        GameObject.Destroy(_playerEntity.gameObject);

        _playerEntity = null;
    }

    private void ReleaseWagon()
    {
        if (_wagon == null)
            return;

        _wagon.Release();
        GameObject.Destroy(_wagon.gameObject);
        _wagon = null;
    }
}
