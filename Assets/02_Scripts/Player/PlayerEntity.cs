using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerEntity : MonoBehaviour, ITargetable, IDamageable, IStatusEffectReceiver, IHealable
{
    [SerializeField] // TEST
    private bool _testIsHealthFull;

    public StatusEffectController StatusEffects { get; private set; }
    public IStatModifierReceiver StatModifierReceiver { get; private set; }
    public ISkillModifierReceiver SkillModifierReceiver { get; private set; }

    public bool IsAlive => true;

    public EntityType EntityType => EntityType.Player;

    public Vector3 Position => transform.position;

    public bool IsHealthFull => _testIsHealthFull;


    private PlayerCombatController _combatController;
    private PlayerStatController _statController;
    private PlayerSkillModifier _skillModifier;

    private bool _isInBarrier;


    private void Awake()
    {
        _combatController = GetComponent<PlayerCombatController>();

        PlayerStatData playerStatData = new();//GameManager.DataTable.GetPlayerStatData("테스트 직업 아이디");
        _statController = new(playerStatData);

        _skillModifier = new();

        var adapter = new PlayerStatusEffectAdapter(_statController, _skillModifier);
        StatModifierReceiver = adapter;
        SkillModifierReceiver = adapter;

        StatusEffects = new StatusEffectController();

        Init().Forget();
    }

    // TODO(김익환): 플레이어 생성 후 호출하기. 지금은 간단하게 UniTask로 호출
    public async UniTask Init()
    {
        await UniTask.WaitForSeconds(0.1f);
        _combatController.Init(_statController, _skillModifier);
    }

    private void Update()
    {
        if (GameManager.Time.IsPaused)
            return;

        StatusEffects.Update(GameManager.Time.GameDeltaTime);
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

    public float Heal(float amount)
    {
        Debug.Log($"{GetType()} 플레이어 회복: {amount}");
        return 0f;// StatusEffects.Heal(amount);
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
} 