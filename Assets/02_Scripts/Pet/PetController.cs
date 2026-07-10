using System.Collections.Generic;
using UnityEngine;


// TODO(김익환): 이동, 자동 공격(여기는 기본적으로 일반 공격인데, 스킬 쿨타임 돌면 바로 발동(대신 일반공격이 끝나고 나서 발동))
// TODO(김익환): 패시브 작동(상시 발동이니 한 번 호출하면 끝 아닌가?) -> 기획 아직 없어서 추후 작성

public class PetController : MonoBehaviour, ITargetable, IDamageable
{
    public float AttackPower => _petStausController.AttackPower;
    public Vector3 Position => transform.position;
    public EntityType EntityType => EntityType.Pet;
    public bool IsAlive => _petStausController.IsAlive;

    [SerializeField] private SOPetDefinition __SOPetDefinition;
    [SerializeField] private SOPetSearch __SOPetSearch;

    private PetMovement _petMovement;

    private PetStausController _petStausController;
    private PetCombatController _petCombatController;



    private void Awake()
    {
        _petMovement = GetComponent<PetMovement>();
        _petStausController = new();
        _petCombatController = new();

    }

    public void Init()
    {
        //_petMovement.Init();

        _petStausController.Init(__SOPetDefinition.BaseStats.MaxHp);
    }

    private void Update()
    {
        // TODO(김익환): 지속적으로 Nav Mesh Agent 목적지 갱신이 맞는가?
        //_petMovement.SetDestination(_petCommandResult.Target.Position);

        // TODO(김익환): 설정된 명령 상태를 실행.
        //PetCommandResult commandResult = _commandState.GetCommandResult(__SOPetSearch);
    }


    public void TakeDamage(DamageInfo damageInfo)
    {
        _petStausController.TakeDamage(damageInfo);
    }

    //public PetCommandResult EvaluateCommand(
    //    PetCommandController commandController,
    //    PetCommandContext context)
    //{
    //    return commandController.Evaluate(context);
    //}

    //public void ApplyCommandResult(PetCommandResult result)
    //{
    //    switch (result.MoveIntent)
    //    {
    //        case EPetMoveIntent.Stop:
    //        case EPetMoveIntent.None:
    //            _movement.Stop();
    //            break;

    //        case EPetMoveIntent.MoveToPosition:
    //        case EPetMoveIntent.ChaseTarget:
    //        case EPetMoveIntent.ReturnToAnchor:
    //            _movement.SetDestination(result.Destination);
    //            break;
    //    }
    //}

    //public void ExecuteCombat(PetCommandResult result, PetCombatContext context, float deltaTime)
    //{
    //    _combatController.Tick(context.WithTarget(result.Target), deltaTime);
    //}

    //public IReadOnlyList<ITargetable> ScanTargets()
    //{
    //    return _targetScanner.Scan();
    //}

}