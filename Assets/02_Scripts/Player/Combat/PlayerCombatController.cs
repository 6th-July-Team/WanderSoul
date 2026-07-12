using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAimHandler))]
public class PlayerCombatController : MonoBehaviour
{
    // Components
    private PlayerInputHandle _inputHandle;
    private PlayerAimHandler _aimHandler;

    // Mana
    public ManaPool ManaPool { get; private set; }

    // Data
    private PlayerStatController _statController;

    // skill
    private PlayerClassSkillBuild _skillBuild;

    private PlayerClassSkillMaker _skillMaker;

    private void Awake()
    {
        _inputHandle = GetComponent<PlayerInputHandle>();
        _aimHandler = GetComponent<PlayerAimHandler>();

        Init();
    }

    public void Init()
    {
        PlayerStatData playerStatData = GameManager.DataTable.GetPlayerStatData("테스트 직업 아이디");
        _statController = new(playerStatData);

        ManaPool = new(_statController);
        _skillMaker = new(ManaPool);

        _skillBuild = _skillMaker.CreateSkillBuild("테스트 직업 아이디", _statController);
    }

    private void OnEnable()
    {
        _inputHandle.OnBasicAttackEvent += OnBasicAttack;
        _inputHandle.OnSpecialAttackEvent += OnSpecialAttack;
        _inputHandle.OnUltimateSkillEvent += OnUltimateAttack;
    }

    private void OnDisable()
    {
        _inputHandle.OnBasicAttackEvent -= OnBasicAttack;
        _inputHandle.OnSpecialAttackEvent -= OnSpecialAttack;
        _inputHandle.OnUltimateSkillEvent -= OnUltimateAttack;
    }

    private void Update()
    {
        if (GameManager.Time.IsPaused)
            return;

        ManaPool.Update(Time.deltaTime);

        // TODO(김익환): 자동 공격 옵션이 켜져 있으면 일반 공격은 자동공격 하기.
        // if(자동공격 옵션 bool 변수)
        //      OnBasicAttack();

        _skillBuild.Update(GameManager.Time.GameDeltaTime);
    }

    public void SetSkill(SkillSlot slot, PlayerSkill skill)
    {
        _skillBuild.SetSkill(slot, skill);
    }

    public SOSkillDefinition GetSkillInfo(SkillSlot slot)
    {
        return _skillBuild.GetSkillInfo(slot);
    }


    private void OnBasicAttack() => TryExecuteSkill(SkillSlot.Basic);
    private void OnSpecialAttack() => TryExecuteSkill(SkillSlot.Special);
    private void OnUltimateAttack() => TryExecuteSkill(SkillSlot.Ultimate);

    private void TryExecuteSkill(SkillSlot skillSlot)
    {
        _skillBuild.TryExecuteSkill(skillSlot, CreateSkillUseContext());
    }

    private SkillUseContext CreateSkillUseContext()
    {
        return new SkillUseContext(_aimHandler.transform, _aimHandler.AimDirection, _aimHandler.AimWorldPoint);
    }
}