
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public static ResourceManager Resource { get { return Instance._resourceManager; } }
    public static SoundManager Sound { get { return Instance._soundManager; } }
    public static PoolManager Pool { get { return Instance._poolManager; } }
    public static DataTable DataTable { get { return Instance._dataTable; } }
    public static SaveManager Save { get { return Instance._userDataManager; } }
    public static TimeManager Time { get { return Instance._timeManager; } }
    public static PetPartyController PetParty { get { return Instance._petPartyController; } }


    #region Manager Varialbes

    private ResourceManager _resourceManager = new();
    private SoundManager _soundManager = new();
    private PoolManager _poolManager = new();
    private DataTable _dataTable = new();
    private SaveManager _userDataManager = new();
    private TimeManager _timeManager = new();
    private PetPartyController _petPartyController = new(); // TODO(김익환): 추후 필요시 new 하기

    #endregion

    #region

    public Transform PlayerTransfrom { get; private set; }
    // 마차 위치도 생각

    #endregion

    protected override void Init()
    {
        base.Init();
    }

    

}
