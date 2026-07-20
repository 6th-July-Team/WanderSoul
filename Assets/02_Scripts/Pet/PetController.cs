using UnityEngine;


public class PetController : MonoBehaviour, ITargetable, IDamageable, IStatusEffectReceiver
{
    public StatusEffectController StatusEffects { get; private set; }
    public IStatModifierReceiver StatModifierReceiver { get; private set; }
    public ISkillModifierReceiver SkillModifierReceiver { get; private set; }

    public float AttackPower => _petStausController.AttackPower;
    public Vector3 Position => transform.position;
    public EntityType EntityType => EntityType.Pet;
    public bool IsAlive => _petStausController.IsAlive;
    public PetElement Element => __SOPetDefinition.Element;

    [Header("Command Setting")]
    [SerializeField] private float _commandRefreshInterval = 0.2f;
    private float _commandRefreshTimer;

    [Header("TEMP Pet Data")]
    [SerializeField] private SOPetDefinition __SOPetDefinition;
    [SerializeField] private SOPetSearch __SOPetSearch;

    private PetMovement _petMovement;
    private PetStatController _petStatController;
    private PetCombatController _combatController;
    private PetStausController _petStausController;
    private PetCommandController _petCommandController;

    private bool isInitialized = false;

    private PetCommandResult _commandResult;

    private void Awake()
    {
        isInitialized = false;
        _petMovement = GetComponent<PetMovement>();
    }

    public void Init(string petId, IPositionProvider playerAnchor, IPositionProvider wagonAnchor
        , PetSkillMaker petSkillMaker, IStatusEffectReceiver playerEffectReceiver, IHealable playerHealable
        , int avoidancePriority)
    {
        PetStatData petStatData = GameManager.DataTable.GetPetStatData(petId);

        _petStatController = new(petStatData);

        _combatController = petSkillMaker.CreateCombatController(petId, _petStatController, playerEffectReceiver, playerHealable, StatModifierReceiver);

        // TODO(김익환): SO 제거
        _petCommandController = new(playerAnchor, wagonAnchor, this, __SOPetSearch, 32);

        _petMovement.Init(petId, avoidancePriority);

        _petStausController = new();
        _petStausController.Init(__SOPetDefinition.BaseStats.MaxHp); // TODO(김익환): SO 제거


        StatusEffects = new StatusEffectController();
        StatModifierReceiver = new PetStatusEffectAdapter(_petStatController);

        // 펫 스킬 강화가 생기면 플레이어 처럼 만들기.
        SkillModifierReceiver = null;

        isInitialized = true;
    }

    private void Update()
    {
        if (false == isInitialized)
            return;

        if (GameManager.Time.IsPaused)
            return;

        _combatController.Update(Time.deltaTime);

        UpdateCommand(Time.deltaTime);
        UpdateCombat();

        StatusEffects.Update(GameManager.Time.GameDeltaTime);
    }

    private void UpdateCommand(float deltaTime)
    {
        _commandRefreshTimer += deltaTime;

        if (_commandRefreshTimer < _commandRefreshInterval)
            return;

        _commandRefreshTimer = 0f;

        GetCommandAndApply();
    }

    private void UpdateCombat()
    {
        if (_combatController.IsBusy)
        {
            _petMovement.Stop();
            return;
        }

        ITargetable target = _commandResult.Target;

        PetActiveSkill selectedSkill = _combatController.SelectSkill(target);

        if (selectedSkill == null)
            return;

        switch (selectedSkill.TargetType)
        {
            case TargetType.Player:
            case TargetType.Pet:
                ExecuteNoEnemySkill(selectedSkill);
                return;
            case TargetType.Enemy:
                ExecuteEnemySkill(selectedSkill, target);
                return;
        }
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        // TODO(김익환): 저항 적용하기
        _petStausController.TakeDamage(damageInfo);
    }

    public void SetCommandMode(PetCommand commandMode)
    {
        _petCommandController.SetCommandMode(commandMode);
        GetCommandAndApply();
    }

    private void GetCommandAndApply()
    {
        _commandResult = _petCommandController.GetCommandResult();

        _petMovement.ApplyCommand(_commandResult);
    }

    public void AddModifier(StatModifier modifier)
    {
        _petStatController.AddModifier(modifier);
    }

    public void RemoveModifiers(StatType statType)
    {
        // _petStatController.RemoveModifiers(statType);
    }

    private void ExecuteNoEnemySkill(PetActiveSkill skill)
    {
        PetSkillUseContext context = new(transform.position, skill.SkillData);

        _combatController.TryExecute(skill, context);
    }

    private void ExecuteEnemySkill(PetActiveSkill skill, ITargetable target)
    {
        if(null == target || !target.IsAlive)
            return;

        float castRange = skill.CastRange;

        _petMovement.SetCombatRange(castRange);

        if (!_petMovement.IsTargetInRange(target, castRange))
            return;

        _petMovement.Stop();

        PetSkillUseContext context = new PetSkillUseContext(transform.position, skill.SkillData);

        _combatController.TryExecute(skill, context);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, __SOPetSearch.RangeWhenGuardCart);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, __SOPetSearch.RangeWhenFollowPlayer);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, __SOPetSearch.RangeWhenAggressive);
    }
}