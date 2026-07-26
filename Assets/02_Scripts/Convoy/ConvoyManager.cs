using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class ConvoyManager
{
    private PetSkillMaker _petSkillMaker;
    private string _selectedQuestId;
    private List<string> _selectedPetIds = new();

    private Wagon _wagon;
    private List<GameObject> _petList = new();

    private PlayerEntity _playerEntity;
    private PlayerSkillMaker _playerSkillMaker;

    private CameraHandler _cameraHandler;

    private const string PLAYER_ID = "player_scholar"; // 현재 플레이어 선택 불가능하여, 학자가 고정되어 있음.

    public ConvoyManager(PetSkillMaker petSkillMaker, PlayerSkillMaker playerSkillMaker)
    {
        _petSkillMaker = petSkillMaker;
        _playerSkillMaker = playerSkillMaker;
    }

    // 의뢰 시작 -> 펫 선택 -> 로딩 UI를 보여주며 아래 init 함수 호출.
    public void InitConvoy(string questId, List<string> selectedPetIds)
    {
        _selectedQuestId = questId;

        _selectedPetIds.Clear();
        _selectedPetIds.AddRange(selectedPetIds);

        StartConvoyAsync().Forget();
    }

    public void FaildConvoy(ConvoyFailReason failReason = ConvoyFailReason.WagonDestroyed)
    {
        GameManager.UI.CloseAllConvoyHuds();
        var result = MakeConvoyResult(false, failReason);
        GameManager.UI.OpenConvoyFailUI(result);
    }

    public void SuccessConvoy()
    {
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

        // TODO(이태영): 실제 집계 값으로 교체
        result.ClearTime = 185f;
        result.IsNewRecord = false;
        result.KilledMonsterCount = 42;
        result.GainedSoul = 120;

        if (isSuccess == true)
        {
            result.GoldReward = 850;
            result.ReputationReward = 10;
        }
        else
        {
            result.ReputationPenalty = 5;
            result.RepairCost = 300;
            result.IsRepairCostPaid = true;
            result.ExtraReputationPenalty = 3;
        }

        // TODO(이태영): Release()에서 반환하는 마을 ID와 연동 필요
        result.ReturnTownId = "town_lavendil";

        return result;
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

        // 6. 게임 상태 변경

        // 7. 미쳐 반환하지 못한 풀 반환
        GameManager.Pool.AllDespawnToPool();

        // TODO 결과에 따라 실패 시 의뢰 출발 마을 ID, 성공 시 도착 마을 ID 반환
        return "테스트 ID";
    }

    private async UniTaskVoid StartConvoyAsync()
    {
        GameManager.UI.OpenLoadingUI();

        await UniTask.Delay(System.TimeSpan.FromSeconds(1f));

        LoadMap();
        SpawnPet();
        InitCamera();

        StartBattle();

        GameManager.UI.CloseUI(UIType.LoadingUIView);
    }

    private void LoadMap()
    {
        var tradeRouteHandler = GameObject.Instantiate(Utils.ResourcesLoad<TradeRouteHandler>("Map/Map_01-TEST"));

        SpawnPlayer();

        SpawnWagon(tradeRouteHandler.SplineContainer);
    }

    private void SpawnWagon(SplineContainer splineContainer)
    {
        var wagonViewModel = GameManager.Network.RequestCreateWagon();

        _wagon = GameObject.Instantiate(Utils.ResourcesLoad<Wagon>("Wagon/Wagon_ProtoType"));
        _wagon.Init(wagonViewModel, _playerEntity);
        _wagon.SetSpline(splineContainer);
    }

    private void SpawnPlayer()
    {
        var playerViewModel = GameManager.Network.RequestCreatePlayer();
        var playerStatController = GameManager.Network.PlayerService.StatController;

        _playerEntity = GameObject.Instantiate(Utils.ResourcesLoad<PlayerEntity>("Test_Scholar"));

        _playerEntity.Init(PLAYER_ID, playerViewModel, playerStatController);
    }

    private void SpawnPet()
    {
        List<PetController> petControllers = new();

        for(int index = 0; index < _selectedPetIds.Count; index++)
        {
            var petOB = GameManager.Resource.GetLoadedAsset<GameObject>(_selectedPetIds[index]);
            GameObject petInstance = GameObject.Instantiate(petOB);
            _petList.Add(petInstance);

            var viewModel = GameManager.Network.CreatePetViewModel(_selectedPetIds[index]);

            petInstance.GetComponent<PetController>().Init(_selectedPetIds[index]
                            , _playerEntity, _wagon
                            , _petSkillMaker
                            , _playerEntity, _playerEntity
                            , 30 + index * 10
                            , viewModel);

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
            partyHud.SetWagon("마차", 1f);

            for (int i = 0; i < _selectedPetIds.Count; i++)
            {
                partyHud.AddPet(_selectedPetIds[i], 1f);
            }
        }

        var playerVm = GameManager.Network.PlayerService.GetPlayerViewModel();
        if (playerVm != null)
        {
            GameManager.UI.OpenPlayerHudUI(playerVm);
        }

        var resourceModel = new ResourceModel();

        resourceModel.Soul = 5;
        resourceModel.Money = 999999;

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
