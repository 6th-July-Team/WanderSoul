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

    public void FaildConvoy()
    {
        // TODO(UI): 실패 결과 UI 표시
    }

    public void SuccessConvoy()
    {
        // TODO(UI): 성공 결과 UI 표시
        Debug.Log("호위 성공");
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
        var loading = GameManager.UI.OpenLoadingUI();

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
        GameManager.Network.RequestCreateWagon();
        var wagonViewModel = GameManager.Network.RequestCreateWagon();

        _wagon = GameObject.Instantiate(Utils.ResourcesLoad<Wagon>("Wagon_ProtoType"));
        _wagon.Init(wagonViewModel, _playerEntity);
        _wagon.SetSpline(splineContainer);
    }

    private void SpawnPlayer()
    {
        GameManager.Network.RequestCreatePlayer();
        var playerViewModel = GameManager.Network.RequestCreatePlayer();
        var playerStatController = GameManager.Network.PlayerService.StatController;

        _playerEntity = GameObject.Instantiate(Utils.ResourcesLoad<PlayerEntity>("Test_Scholar"));
        _playerEntity.Init(playerViewModel, playerStatController);
    }

    private void SpawnPet()
    {
        List<PetController> petControllers = new();

        for(int index = 0; index < _selectedPetIds.Count; index++)
        {
            GameObject petInstance = GameObject.Instantiate(Utils.ResourcesLoad<GameObject>("Test_Pet"));

            var viewModel = GameManager.Network.CreatePetViewModel(_selectedPetIds[index]);

            petInstance.GetComponent<PetController>().Init("pet_fire_001"/*_selectedPetIds[index]*/
                            , _playerEntity, _wagon
                            , _petSkillMaker
                            , _playerEntity, _playerEntity
                            , 30 + index * 10
                            , viewModel);

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

        if (_wagon != null)
        {
            GameManager.UI.OpenConvoyHudUI(_wagon.ViewModel, _selectedQuestId);
        }

        var partyHud = GameManager.UI.OpenPartyHudUI();

        if (partyHud != null)
        {
            partyHud.SetWagon("마차", 1f);
            partyHud.AddPet("펫1", 1f);
        }

        var resourceModel = new ResourceModel();

        resourceModel.Soul = 5;
        resourceModel.Money = 999999;
        GameManager.UI.OpenResourceHudUI(resourceModel);

        // TODO(이태영): 스킬 HUD - PlayerCombatController 접근 방법 확인 필요
        // TODO(이태영): 플레이어 HP/MP HUD - ManaPool, HP 소스 확인 필요
    }
}
