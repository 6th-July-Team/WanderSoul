using Unity.VisualScripting;
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
    public Transform Transform => this.transform;

    public bool IsHealthFull => _testIsHealthFull;


    private PlayerViewModel _playerViewModel;
    private PlayerOutGameViewModel _playerOutGameViewModel;


    private PlayerCombatController _combatController;
    private PlayerStatController _statController;
    private PlayerSkillModifier _skillModifier;

    private bool _isInitialized = false;
    private bool _isInBarrier;


    private void Awake()
    {
        _combatController = GetComponent<PlayerCombatController>();

        _skillModifier = new();
        StatusEffects = new StatusEffectController();
    }

    public void Init(string playerId, PlayerViewModel playerViewModel, PlayerStatController playerStatController, PlayerSkillMaker playerSkillMaker)
    {
        _playerViewModel = playerViewModel;
        _statController = playerStatController;

        var build = playerSkillMaker.CreateSkillBuild(playerId, playerStatController);

        _combatController.Init(_statController, build, _skillModifier);

        var adapter = new PlayerStatusEffectAdapter(_statController, _skillModifier);
        StatModifierReceiver = adapter;
        SkillModifierReceiver = adapter;

        _playerOutGameViewModel = GameManager.Network.RequestPlayerOutGameViewModel();

        _isInitialized = true;
    }

    private void Update()
    {
        if (GameManager.Time.IsPaused || !_isInitialized)
            return;

        _playerViewModel.Update(GameManager.Time.GameDeltaTime);
        StatusEffects.Update(GameManager.Time.GameDeltaTime);
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        // TODO 데미지 입는 로직
        if (_isInBarrier)
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

    public float GetSkillCoolTime(SkillSlot skillSlot)
    {
        return _combatController.GetSkillCoolTime(skillSlot);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IBarrierable barrierable))
        {
            _isInBarrier = true;
        }

        if (other.TryGetComponent<EnemyDropObject>(out var enemyDropObject))
        {
            if (enemyDropObject.Type == DropObjectType.Exp)
            {
                _playerViewModel.AddExp(enemyDropObject.Value);
            }
            else if (enemyDropObject.Type == DropObjectType.Soul)
            {
                _playerOutGameViewModel.AddSoul(enemyDropObject.Value);
            }

            enemyDropObject.OnCollected();
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