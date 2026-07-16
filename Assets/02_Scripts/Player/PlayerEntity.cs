using Cysharp.Threading.Tasks;
using Unity.AppUI.UI;
using UnityEngine;

public class PlayerEntity : MonoBehaviour, ITargetable, IDamageable
{
    public bool IsAlive => true;

    public EntityType EntityType => EntityType.Player;

    public Vector3 Position => transform.position;

    private PlayerCombatController _combatController;
    private PlayerStatController _statController;

    private bool _isInBarrier;

    // 기즈모를 위해 임시로 들고옴, 추후 삭제
    private PlayerAimHandler _aimHandler;


    private void Awake()
    {
        _aimHandler = GetComponent<PlayerAimHandler>();

        _combatController = GetComponent<PlayerCombatController>();

        PlayerStatData playerStatData = new();//GameManager.DataTable.GetPlayerStatData("테스트 직업 아이디");
        _statController = new(playerStatData);

        Init().Forget();
    }

    // TODO(김익환): 플레이어 생성 후 호출하기. 지금은 간단하게 UniTask로 호출
    public async UniTask Init()
    {
        await UniTask.WaitForSeconds(0.1f);
        _combatController.Init(_statController);
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        // TODO 데미지 입는 로직
        if(_isInBarrier)
        {
            Debug.Log($"{GetType()} 플레이어 베리어로 데미지 흡수!");
        }
        else
        {

            Debug.Log($"{GetType()} 플레이어 피격!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IBarrierable barrierable))
        {
            _isInBarrier = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IBarrierable barrierable))
        {
            _isInBarrier = false;
        }
    }

    [SerializeField] Vector3 _gizmoOffset = new Vector3(0, 1.5f, 1.5f);
    [SerializeField] Vector3 _Radius = new Vector3(1.5f, 1.5f, 1.5f);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        
    }
} 