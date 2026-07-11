
using Cysharp.Threading.Tasks;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public static ResourceManager Resource { get { return Instance._resourceManager; } }
    public static SoundManager Sound { get { return Instance._soundManager; } }
    public static PoolManager Pool { get { return Instance._poolManager; } }
    public static DataTable DataTable { get { return Instance._dataTable; } }
    public static SaveManager Save { get { return Instance._userDataManager; } }
    public static TimeManager Time { get { return Instance._timeManager; } }

    #region Manager Varialbes

    private ResourceManager _resourceManager = new();
    private SoundManager _soundManager = new();
    private PoolManager _poolManager = new();
    private DataTable _dataTable = new();
    private SaveManager _userDataManager = new();
    private TimeManager _timeManager = new();

    #endregion

    #region Variables

    private Transform _poolRoot = null;

    #endregion

    #region Test Variables
    [SerializeField] private bool _skipStartupUIForTest = false;
    public Transform PlayerTransfrom { get; private set; }

    #endregion

    protected override void Init()
    {
        base.Init();

        _dataTable.LoadAllData();

        _userDataManager.Init();
        _userDataManager.LoadAllUserData();
        SetLoadData();

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
}
