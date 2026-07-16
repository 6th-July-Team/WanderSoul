using UnityEngine;


// TODO(김익환): 자동 공격(여기는 기본적으로 일반 공격인데, 스킬 쿨타임 돌면 바로 발동(대신 일반공격이 끝나고 나서 발동))
// TODO(김익환): 패시브 작동(상시 발동이니 한 번 호출하면 끝 아닌가?) -> 기획 아직 없어서 추후 작성

public class PetController : MonoBehaviour, ITargetable, IDamageable
{
    [Header("Command Setting")]
    [SerializeField] private float _commandRefreshInterval = 0.2f;
    private float _commandRefreshTimer;

    public float AttackPower => _petStausController.AttackPower;
    public Vector3 Position => transform.position;
    public EntityType EntityType => EntityType.Pet;
    public bool IsAlive => _petStausController.IsAlive;
    public PetElement Element => __SOPetDefinition.Element;

    [Header("TEMP Pet Data")]
    [SerializeField] private SOPetDefinition __SOPetDefinition;
    [SerializeField] private SOPetSearch __SOPetSearch;

    private PetMovement _petMovement;

    private PetStatController _petStatController;
    private PetStausController _petStausController;
    private PetCommandController _petCommandController; 

    private PetCombatController _combatController;
    private PetSkillMaker _skillMaker;

    private bool isInitialized = false;

    private PetCommandResult _commandResult;

    private void Awake()
    {
        isInitialized = false;
        _petMovement = GetComponent<PetMovement>();
    }

    public void Init(string petId, IPositionProvider playerAnchor, IPositionProvider wagonAnchor)
    {
        _petStausController = new();

        PetStatData petStatData = GameManager.DataTable.GetPetStatData(petId);
        _petStatController = new(petStatData);

        _skillMaker = new();
        _combatController = _skillMaker.CreateCombatController(petId, _petStatController);

        _petCommandController = new(playerAnchor, wagonAnchor, this, __SOPetSearch, 32);

        _petMovement.Init(petId);
        _petStausController.Init(__SOPetDefinition.BaseStats.MaxHp);

        isInitialized = true;
    }

    private void Update()
    {
        if (false == isInitialized)
            return;

        if(GameManager.Time.IsPaused)
            return;

        _combatController.Update(Time.deltaTime);

        UpdateCommand(Time.deltaTime);
        UpdateCombat();
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
        ITargetable target = _commandResult.Target;

        if (null == target || !target.IsAlive)
            return;

        if (_combatController.IsBusy)
        {
            _petMovement.Stop();
            return;
        }

        PetActiveSkill selectedSkill = _combatController.SelectSkill(target);

        if (selectedSkill == null)
            return;

        float castRange = selectedSkill.CastRange;

        _petMovement.SetCombatRange(castRange);

        if (!_petMovement.IsTargetInRange(target, castRange))
            return;

        _petMovement.Stop();

        PetSkillUseContext context = new PetSkillUseContext();

        _combatController.TryExecute(selectedSkill, context);
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

    public void AddModifier(PetStatModifier modifier)
    {
        _petStatController.AddModifier(modifier);
    }

    public void RemoveModifiers(PetStatType statType)
    {
        _petStatController.RemoveModifiers(statType);
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