
using UnityEngine;

public class PlayerEntity : MonoBehaviour, ITargetable, IDamageable, IStatusEffectReceiver, IHealable
{
    [SerializeField] // TEST
    private bool _testIsHealthFull;
    [Header("Orb")]
    [SerializeField] private Transform _orbTransform;

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

    // scholarOrb
    private ScholarOrb _scholarOrb;


    private void Awake()
    {
        _combatController = GetComponent<PlayerCombatController>();

        _skillModifier = new();
        _skillModifier.Init();
        StatusEffects = new StatusEffectController();
    }

    public void Init(string playerId, PlayerViewModel playerViewModel, PlayerStatController playerStatController)
    {
        _playerViewModel = playerViewModel;
        _statController = playerStatController;

        var adapter = new PlayerStatusEffectAdapter(_statController, _skillModifier);
        StatModifierReceiver = adapter;
        SkillModifierReceiver = adapter;

        _playerOutGameViewModel = GameManager.Network.RequestPlayerOutGameViewModel();

        _isInitialized = true;
    }

    public void InitAfterSpawnPet(string playerId, PlayerSkillMaker playerSkillMaker, IStatusEffectReceiver[] petStatusEffectReceviers)
    {
        var build = playerSkillMaker.CreateSkillBuild(playerId, _statController, petStatusEffectReceviers);

        _scholarOrb = Instantiate(Utils.ResourcesLoad<ScholarOrb>("Player/ScholarOrb")
            , _orbTransform.position, Quaternion.identity);

        _scholarOrb.Init(_orbTransform);

        _combatController.Init(build, _skillModifier, _scholarOrb.transform);
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
            Logger.Log($"{GetType()} 플레이어 베리어로 데미지 흡수!");
        }
        else
        {
            _playerViewModel.ReduceHp(damageInfo.DamageAmount);
        }
    }

    public void Heal(float amount)
    {
        _playerViewModel.AddHp(amount);
    }

    public float GetSkillCoolTime(SkillSlot skillSlot)
    {
        return _combatController.GetSkillCoolTime(skillSlot);
    }

    public void Release()
    {
        _isInitialized = false;

        _scholarOrb.Release();

        StatusEffects.Clear();
        _statController.ClearAll();

        _combatController = null;
        _playerViewModel = null;
        _statController = null;
        _skillModifier = null;
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
                _playerOutGameViewModel.AddExp(enemyDropObject.Value);
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