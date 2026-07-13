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

    private PetStausController _petStausController;
    private PetCombatController _petCombatController;
    private PetCommandController _petCommandController;

    private bool isInitialized = false;

    private void Awake()
    {
        isInitialized = false;
        _petMovement = GetComponent<PetMovement>();
    }

    public void Init(string petId, IPositionProvider playerAnchor, IPositionProvider wagonAnchor)
    {
        _petStausController = new();
        _petCombatController = new();
        _petCommandController = new(playerAnchor, wagonAnchor, this, __SOPetSearch, 32);

        _petMovement.Init(petId);
        _petStausController.Init(__SOPetDefinition.BaseStats.MaxHp);

        isInitialized = true;
    }

    private void Update()
    {
        if (false == isInitialized)
            return;

        _commandRefreshTimer += Time.deltaTime;

        if (_commandRefreshTimer < _commandRefreshInterval)
            return;

        _commandRefreshTimer = 0f;

        GetCommandAndApply();

        //_petCombatController.SetTarget(result.Target);
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        _petStausController.TakeDamage(damageInfo);
    }

    public void SetCommandMode(PetCommand commandMode)
    {
        _petCommandController.SetCommandMode(commandMode);
        GetCommandAndApply();
    }

    private void GetCommandAndApply()
    {
        PetCommandResult result = _petCommandController.GetCommandResult();
        _petMovement.ApplyCommand(result);
    }

    // 명령에 따른 행동 실행
    // 전투 시퀀스 실행
}