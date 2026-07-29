
using UnityEngine;

public class PlayerEntity : MonoBehaviour, IDamageable, IStatusEffectReceiver, IHealable
{
    [SerializeField] // TEST
    private bool _testIsHealthFull;
    [Header("Orb")]
    [SerializeField] private Transform _orbTransform;

    // IStatusEffectReceiver
    public StatusEffectController StatusEffects { get; private set; }
    public IStatModifierReceiver StatModifierReceiver { get; private set; }
    public ISkillModifierReceiver SkillModifierReceiver { get; private set; }

    // IDamageable
    public bool IsAlive => true;
    public EntityType EntityType => EntityType.Player;
    public Vector3 Position => transform.position;
    public Transform Transform => this.transform;

    // IHealable
    public bool IsHealthFull => _testIsHealthFull;

    // MVVM
    private PlayerViewModel _playerViewModel;
    private PlayerOutGameViewModel _playerOutGameViewModel;

    // Components
    private PlayerCombatController _combatController;
    private PlayerStatController _statController;
    private PlayerSkillModifier _skillModifier;

    // Barrier
    private ScholarBarrier _currentBarrier;

    // scholarOrb
    private ScholarOrb _scholarOrb;

    // variables
    private bool _isInitialized = false;


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
        float remainDamage = damageInfo.DamageAmount;

        if (_currentBarrier != null)
        {
            remainDamage = _currentBarrier.AbsorbDamage(remainDamage);
        }

        if (remainDamage <= 0f)
            return;

        _playerViewModel.ReduceHp(remainDamage);
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
        if (other.TryGetComponent(out ScholarBarrier barrier))
        {
            _currentBarrier = barrier; 
            Debug.Log($"{GetType()}: 베리어 들어옴");

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
        if (other.TryGetComponent(out ScholarBarrier barrierable))
        {
            _currentBarrier = null;
            Debug.Log($"{GetType()}: 베리어 나감");

        }
    }
}