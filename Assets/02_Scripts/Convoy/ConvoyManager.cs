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
    private PlayerEntity _playerEntity;

    public ConvoyManager(PetSkillMaker petSkillMaker)
    {
        _petSkillMaker = petSkillMaker;
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
        // 게임 상태 변경
        // 풀 사용한거 Despawn - 몬스터
        // 펫 소환 해제
        // 플레이어 제거.
        // 마차 제거

        // TODO 결과에 따라 실패 시 의뢰 출발 마을 ID, 성공 시 도착 마을 ID 반환
        return "테스트 ID";
    }

    private async UniTaskVoid StartConvoyAsync()
    {
        // TODO(UI): 로딩 UI 또는 Fade In/Out 처리
        GameManager.UI.OpenLoadingUI();

        await UniTask.Delay(System.TimeSpan.FromSeconds(1f));

        LoadMap();
        SpawnPet();

        // CameraManager.SetBattleView(player.transform);
        // LoadingUI.Hide();

        StartBattle();

        GameManager.UI.CloseUI(UIType.LoadingUIView);
    }

    private void LoadMap()
    {
        var tradeRouteHandler = GameObject.Instantiate(Utils.ResourcesLoad<TradeRouteHandler>("Map_01-TEST"));

        SpawnPlayer();

        SpawnWagon(tradeRouteHandler.SplineContainer);
    }

    private void SpawnWagon(SplineContainer splineContainer)
    {
        var wagonViewModel = GameManager.Network.RequestCreateWagon();

        _wagon = GameObject.Instantiate(Utils.ResourcesLoad<Wagon>("Wagon_ProtoType"));
        _wagon.Init(wagonViewModel, _playerEntity);
        _wagon.SetSpline(splineContainer);
    }

    private void SpawnPlayer()
    {
        var playerViewModel = GameManager.Network.RequestCreatePlayer();
        var playerStatController = GameManager.Network.PlayerService.StatController;

        _playerEntity = GameObject.Instantiate(Utils.ResourcesLoad<PlayerEntity>("Test_Mercenary"));
        _playerEntity.Init(playerViewModel, playerStatController);
    }

    private void SpawnPet()
    {
        List<PetController> petControllers = new();

        for(int index = 0; index < _selectedPetIds.Count; index++)
        {
            GameObject petInstance = GameObject.Instantiate(Utils.ResourcesLoad<GameObject>("Test_Pet"));

            petInstance.GetComponent<PetController>().Init(_selectedPetIds[index]
                            , _playerEntity, _wagon
                            , _petSkillMaker
                            , _playerEntity, _playerEntity
                            , 30 + index * 10);

            petControllers.Add(petInstance.GetComponent<PetController>());
        }

        GameManager.PetParty.Init(petControllers);
    }


    private void StartBattle()
    {
        // 호위? HUD 표시
        // 게임 상태 변경
        // 아직 펫이 소환이 안 된 시점

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
                var petData = GameManager.DataTable.GetPetData(_selectedPetIds[i]);
                if (petData == null)
                {
                    continue;
                }

                partyHud.AddPet(petData.Name, 1f);
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

        // TODO(이태영): 스킬 HUD - PlayerCombatController 접근 방법 확인 필요
    }
}
