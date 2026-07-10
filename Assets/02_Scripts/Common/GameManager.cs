
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public static ResourceManager Resource { get { return Instance._resourceManager; } }
    public static SoundManager Sound { get { return Instance._soundManager; } }
    public static PoolManager Pool { get { return Instance._poolManager; } }
    public static DataTable DataTable { get { return Instance._dataTable; } }
    public static SaveManager Save { get { return Instance._userDataManager; } }
    public static TimeManager Time { get { return Instance._timeManager; } }
    public static UIManager UI { get { return Instance._uiManager; } }

    #region Manager Varialbes

    private ResourceManager _resourceManager = new();
    private SoundManager _soundManager = new();
    private PoolManager _poolManager = new();
    private DataTable _dataTable = new();
    private SaveManager _userDataManager = new();
    private TimeManager _timeManager = new();
    private UIManager _uiManager = new();

    #endregion

    #region

    public Transform PlayerTransfrom { get; private set; }
    // 마차 위치도 생각

    #endregion

    protected override void Init()
    {
        base.Init();
        _uiManager.Init();
    }

    

}
