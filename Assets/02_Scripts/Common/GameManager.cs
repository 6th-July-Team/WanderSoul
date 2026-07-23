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
    public static NetworkManager Network { get { return Instance._networkManager; } }


    #region Manager Varialbes

    private ResourceManager _resourceManager = new();
    private SoundManager _soundManager = new();
    private PoolManager _poolManager = new();
    private DataTable _dataTable = new();
    private SaveManager _userDataManager = new();
    private TimeManager _timeManager = new();
    private UIManager _uiManager = new();
    private PetPartyController _petPartyController = new();
    private NetworkManager _networkManager = new();


    private ConvoyManager _convoyManager;
    private LoadingUIView _loadingUI;

    #endregion

    #region Variables

    public string SelectedPlayerId { get; private set; }

    [SerializeField] private bool _skipStartupUIForTest = false;
    private Transform _poolRoot = null;

    private PetSkillMaker _petSkillMaker;
    private PlayerSkillMaker _playerSkillMaker;
    private StatusEffectMaker _statusEffectMaker;

    #endregion

    #region Test Variables


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
            InitNonAsync();
            // 여기에 로딩은 없어도 초기화 해야할 것 넣기
            var testPetIds = new List<string> { "pet_fire_001", "pet_water_002", "pet_earth_003" };
            StartConvoy(testPetIds);
            return;
        }

        // 여기에서 로딩 UI 오픈

        _loadingUI = _uiManager.OpenLoadingUI();

        var loadTask = _resourceManager.Init(OnLoadingProgress);
        var minTimeTask = UniTask.Delay(System.TimeSpan.FromSeconds(1.5f));


        await UniTask.WhenAll(loadTask, minTimeTask);

        _uiManager.CloseUI(UIType.LoadingUIView);
        _loadingUI = null;

        InitNonAsync();

        ShowTitle();
    }

    private void ShowTitle()
    {
        var titleUI = _uiManager.OpenUI<TitleUI>(UIType.TitleUI);
        if (titleUI != null)
        {
            titleUI.OnStartClicked += OnGameStart;
        }
    }

    private void OnGameStart()
    {
        _uiManager.CloseUI(UIType.TitleUI);
        // TODO(이태영): 시작 마을 ID를 세이브 데이터나 기획 값에서 가져오기
        EnterVillage("town_lavendil");
    }

    private void OnLoadingProgress(float progress)
    {
        if (_loadingUI == null)
        {
            return;
        }

        _loadingUI.SetProgress(progress);
    }

    private void InitNonAsync()
    {
        _soundManager.Init(this.gameObject);
        PoolInit();

        InitStatusEffect();
        InitPetSystem();
        InitPlayerSkillMaker();

        _convoyManager = new ConvoyManager(_petSkillMaker, _playerSkillMaker);
    }

    private void PoolInit()
    {
        if (null == _poolRoot)
        {
            _poolRoot = Utils.CreateEmptyGameObject("PoolRoot", this.gameObject.transform).transform;
        }

        _poolManager.Init(_poolRoot);
    }

    private void InitStatusEffect()
    {
        StatusEffectRegistry statusEffectRegistry = new();
        StatusEffectResiter.ResisterAll(statusEffectRegistry);
        _statusEffectMaker = new StatusEffectMaker(statusEffectRegistry);
    }

    private void InitPetSystem()
    {
        PetActiveSkillExecutionRegistry activeRegistry = new();
        PetPassiveSkillExecutionRegistry passiveRegistry = new();

        PetSkillRegistor.RegisterAllActiveSkills(activeRegistry);
        PetSkillRegistor.RegisterAllPassiveSkills(passiveRegistry);

        _petSkillMaker = new PetSkillMaker(activeRegistry, passiveRegistry, _statusEffectMaker);
    }

    private void InitPlayerSkillMaker()
    {
        PlayerSkillRegistry playerSkillRegistry = new();
        PlayerSkillRegistor.RegisterAll(playerSkillRegistry);

        var playerViewModel = _networkManager.RequestCreatePlayer();

        _playerSkillMaker = new PlayerSkillMaker(playerSkillRegistry, _petPartyController, playerViewModel
            , _statusEffectMaker);
    }


    private void SetLoadData()
    {
        // 여기에서 세이브 데이터 로드하여 설정할 것들 설정하기
        // ex) 저장된 골드, 레벨
    }

    #endregion

    public void EnterVillage(string villageId)
    {
        _uiManager.OpenUI<MainMenuUI>(UIType.MainMenuUI);

        var resourceModel = new ResourceModel();
        resourceModel.Soul = 12413451;
        resourceModel.Money = 8520;

        var resourceHud = _uiManager.OpenResourceHudUI(resourceModel);

        if (resourceHud != null)
        {
            resourceHud.SetVillageLayout();
        }

        var villageModel = new VillageModel();
        villageModel.TownDataId = villageId;
        villageModel.CurrentReputation = 50;
        _uiManager.OpenVillageInfoHudUI(villageModel);
    }

    public void ExitVillage()
    {
        _uiManager.CloseUI(UIType.MainMenuUI);
        _uiManager.CloseUI(UIType.ResourceHudUIView);
        _uiManager.CloseUI(UIType.VillageInfoHudUIView);
    }

    public void StartConvoy(List<string> selectedPetIds)
    {
        // 해당 시점 이전에 의뢰 선택 및 펫 선택이 완료되어야 합니다.
        // 선택된 의뢰 ID 및 선택된 펫 ID 리스트가 아래 필요합니다.
        _convoyManager.InitConvoy("TEST_QuestId", selectedPetIds);

        ExitVillage();
    }

    public void EndConvoy()
    {
        // TODO 간단 로딩 실행
        string resultVillageId = _convoyManager.Release();
        EnterVillage(resultVillageId);
        _networkManager.InGameServiceRelease();
    }
}
