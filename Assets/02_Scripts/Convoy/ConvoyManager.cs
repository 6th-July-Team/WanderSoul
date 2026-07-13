using System.Collections.Generic;
using UnityEngine;

public class ConvoyManager
{
    private string _selectedQuestId;
    private List<string> _selectedPetIds = new();

    private Wagon _wagon;

    // 의뢰 시작 -> 펫 선택 -> 로딩 UI를 보여주며 아래 init 함수 호출.
    public void InitConvoy(string questId, List<string> selectedPetIds)
    {
        _selectedQuestId = questId;

        _selectedPetIds.Clear();
        _selectedPetIds.AddRange(selectedPetIds);

        StartConvoyAsync();
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

    private void StartConvoyAsync()
    {
        // LoadingUI.Show();

        LoadResources();

        //Wagon cart = SpawnCart();
        //Player player = SpawnPlayer();

        //InitPetParty(player, cart);

        // CameraManager.SetBattleView(player.transform);
        // LoadingUI.Hide();

        StartBattle();
    }

    private void LoadResources()
    {
        // 의뢰 데이터, 맵, 플레이어, 마차, 펫 프리팹 로드
    }

    //private Wagon SpawnWagon()
    //{
    //    // 의뢰 데이터 기준으로 마차 생성
    //}

    //private Player SpawnPlayer()
    //{
    //    // 선택된 플레이어 캐릭터 생성
    //}

    //private PetController SpawnPet()
    //{
    //    PetData petData = GameManager.DataTable.GetPetData(petId);
    //    PetController petPrefab = GameManager.Resource.Load<PetController>(petData.PrefabAddress);

    //    Vector3 spawnPosition = GetPetSpawnPosition(i);

    //    PetController pet = Instantiate(petPrefab, spawnPosition, Quaternion.identity);

    //    pet.Init(petId, _owner, _wagon);
    //}

    private void InitPetParty(/*Player player, Wagon wagon*/)
    {
        // 아래 TEST용
        // 마차
        GameObject wagonInstance = GameObject.Instantiate(Utils.ResourcesLoad<GameObject>("Test_Wagon"));
        _wagon = wagonInstance.GetComponent<Wagon>();

        // 플레이어
        // PetPartyController petParty = player.GetComponent<PetPartyController>();
        GameObject playerInstance = GameObject.Instantiate(Utils.ResourcesLoad<GameObject>("Test_Player"));
        PlayerMovement playerMovement = playerInstance.GetComponent<PlayerMovement>();

        // 펫
        List<PetController> petControllers = new();
        foreach (var petId in _selectedPetIds)
        {
            GameObject petInstance = GameObject.Instantiate(Utils.ResourcesLoad<GameObject>(petId));

            petInstance.GetComponent<PetController>().Init(petId
            , playerMovement.GetComponent<IPositionProvider>(), wagonInstance.GetComponent<IPositionProvider>());

            petControllers.Add(petInstance.GetComponent<PetController>());
        }

        // 펫 파티


        GameManager.PetParty.Init(playerMovement, _wagon, petControllers);
    }

    private void StartBattle()
    {
        // 호위? HUD 표시
        // 게임 상태 변경
        // 아직 펫이 소환이 안 된 시점

        InitPetParty();
    }
}
