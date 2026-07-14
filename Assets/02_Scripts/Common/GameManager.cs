using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public static ResourceManager Resource { get { return Instance._resourceManager; } }
    public static SoundManager Sound { get { return Instance._soundManager; } }
    public static PoolManager Pool { get { return Instance._poolManager; } }
    public static DataTable DataTable { get { return Instance._dataTable; } }
    public static SaveManager Save { get { return Instance._userDataManager; } }
    public static TimeManager Time { get { return Instance._timeManager; } }
    public static ConvoyManager Convoy { get { return Instance._convoyManager; } }
    public static UIManager UI { get { return Instance._uiManager; } }
    public static PetPartyController PetParty { get { return Instance._petPartyController; } }



    #region Manager Varialbes

    private ResourceManager _resourceManager = new();
    private SoundManager _soundManager = new();
    private PoolManager _poolManager = new();
    private DataTable _dataTable = new();
    private SaveManager _userDataManager = new();
    private TimeManager _timeManager = new();
    private ConvoyManager _convoyManager = new();
    private UIManager _uiManager = new();
    private PetPartyController _petPartyController = new();

    #endregion

    #region Variables

    private Transform _poolRoot = null;

    #endregion

    #region Test Variables
    [SerializeField] private bool _skipStartupUIForTest = false;
    public SOPetSkillInfo TestSOPetSkillInfo;
    public Transform PlayerTransfrom { get; private set; }

    #endregion

    #region Init

    protected override void Init()
    {
        base.Init();

        _dataTable.LoadAllData();

        _userDataManager.Init();
        _userDataManager.LoadAllUserData();
        SetLoadData();

        _uiManager.Init();

        InitAsync().Forget();
    }

    private async UniTaskVoid InitAsync()
    {
        if (_skipStartupUIForTest)
        {
            await _resourceManager.Init();
            InitNonAsync();

            // 여기에 로딩은 없어도 초기화 해야할 것 넣기


            return;
        }

        // 여기에서 로딩 UI 오픈

        InitNonAsync();
    }

    private void InitNonAsync()
    {
        _soundManager.Init(this.gameObject);
        PoolInit();
    }

    private void PoolInit()
    {
        if (null == _poolRoot)
        {
            _poolRoot = Utils.CreateEmptyGameObject("PoolRoot", this.gameObject.transform).transform;
        }

        _poolManager.Init(_poolRoot);
    }

    private void SetLoadData()
    {
        // 여기에서 세이브 데이터 로드하여 설정할 것들 설정하기
        // ex) 저장된 골드, 레벨
    }

    #endregion

    public void EnterVillage(string villageId)
    {

    }

    public void ExitVillage()
    {

    }

    public void StartConvoy()
    {
        // 해당 시점 이전에 의뢰 선택 및 펫 선택이 완료되어야 합니다.
        // 선택된 의뢰 ID 및 선택된 펫 ID 리스트가 아래 필요합니다.
        List<string> testSelectedPetIds = new List<string> { "pet_fire_001", "pet_water_002", "pet_earth_003" };
        _convoyManager.InitConvoy("TEST", testSelectedPetIds);

        ExitVillage();
    }

    public void EndConvoy()
    { 
        // TODO 간단 로딩 실행
        string resultVillageId = _convoyManager.Release();
        EnterVillage(resultVillageId);
    }


    #region TEST Functions

    public void TestPlayerFollowMode()
    {
        _petPartyController.SetPetCommand(PetCommand.PlayerFollow);
    }

    public void TestWagonDefenseMode()
    {
        _petPartyController.SetPetCommand(PetCommand.GuardWagon);
    }

    public void TestWagonAggressiveMode()
    {
        _petPartyController.SetPetCommand(PetCommand.Aggressive);
    }
    #endregion
}
