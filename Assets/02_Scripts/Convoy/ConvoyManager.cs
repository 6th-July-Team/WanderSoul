using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Cysharp.Threading.Tasks;

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
        SpawnPlayer();
        //SpawnPet();

        // CameraManager.SetBattleView(player.transform);
        // LoadingUI.Hide();

        StartBattle();

        GameManager.UI.CloseUI(UIType.LoadingUIView);
    }

    private void LoadMap()
    {
        var tradeRouteHandler = Object.Instantiate(Utils.ResourcesLoad<TradeRouteHandler>("Map_01-TEST"));
        SpawnWagon(tradeRouteHandler.SplineContainer, "wagon_001");
    }

    private void SpawnWagon(SplineContainer splineContainer, string wagonId)
    {
        GameManager.Network.RequestCreateWagon(wagonId);
        var wagonViewModel = GameManager.Network.WagonService.GetWagonViewModel(wagonId);

        _wagon = Object.Instantiate(Utils.ResourcesLoad<Wagon>("Wagon_ProtoType"));
        _wagon.Init(wagonId, wagonViewModel);
        _wagon.SetSpline(splineContainer);
    }

    private void SpawnPlayer()
    {
        GameManager.Network.RequestCreatePlayer();
        var playerViewModel = GameManager.Network.PlayerService.GetPlayerViewModel();
        var playerStatController = GameManager.Network.PlayerService.StatController;

        _playerEntity = GameObject.Instantiate(Utils.ResourcesLoad<PlayerEntity>("Test_Mercenary"));
        _playerEntity.Init(playerViewModel, playerStatController);
    }

    private void SpawnPet()
    {
        List<PetController> petControllers = new();
        foreach (var petId in _selectedPetIds)
        {
            GameObject petInstance = GameObject.Instantiate(Utils.ResourcesLoad<GameObject>("Test_Pet"));

            // TODO(김익환): 펫 생성 코드 수정
            petInstance.GetComponent<PetController>().Init(petId, _playerEntity, _wagon, _petSkillMaker, _playerEntity, _playerEntity);

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
