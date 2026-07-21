using UnityEngine;

[RequireComponent(typeof(PlayerAimHandler))]
public class PlayerCombatController : MonoBehaviour
{
    // Components
    private PlayerAimHandler _aimHandler;
    private PlayerInputHandle _inputHandle;
    private PlayerAnimationController _animationController;

    private PlayerStatController _statController;

    // skill
    private PlayerClassSkillBuild _skillBuild;

    private bool _isInitialized = false;

    // Skill Range Check
    private bool _isCheckingSkillRange = false;
    private SkillSlot _checkingSkillSlot;

    private void Awake()
    {
        _aimHandler = GetComponent<PlayerAimHandler>();
        _inputHandle = GetComponent<PlayerInputHandle>();
        _animationController = GetComponent<PlayerAnimationController>();
    }

    public void Init(PlayerStatController statController, PlayerClassSkillBuild skillBuild
        , PlayerSkillModifier skillModifier)
    {
        _statController = statController;

        _skillBuild = skillBuild;
        _skillBuild.Init(skillModifier);

        _isInitialized = true;
    }

    private void OnEnable()
    {
        _inputHandle.OnBasicAttackEvent += OnBasicAttack;

        _inputHandle.OnSpecialAttackEvent += OnSpecialAttack;
        _inputHandle.OnSpecialAttackCheckEvent += OnSpecialAttackCheck;

        _inputHandle.OnUltimateSkillEvent += OnUltimateAttack;
        _inputHandle.OnUltimateSkillCheckEvent += OnUltimateSkillCheck;
    }

    private void OnDisable()
    {
        _inputHandle.OnBasicAttackEvent -= OnBasicAttack;

        _inputHandle.OnSpecialAttackEvent -= OnSpecialAttack;
        _inputHandle.OnSpecialAttackCheckEvent -= OnSpecialAttackCheck;

        _inputHandle.OnUltimateSkillEvent -= OnUltimateAttack;
        _inputHandle.OnUltimateSkillCheckEvent -= OnUltimateSkillCheck;

        if (_isInitialized && _isCheckingSkillRange)
        {
            _skillBuild.HideSkillRange(_checkingSkillSlot);
        }

        _isCheckingSkillRange = false;
    }

    private void Update()
    {
        if (GameManager.Time.IsPaused || !_isInitialized)
            return;

        // TODO(김익환): 자동 공격 옵션이 켜져 있으면 일반 공격은 자동공격 하기.
        // if(자동공격 옵션 bool 변수)
        //      OnBasicAttack();

        _skillBuild.Update(GameManager.Time.GameDeltaTime);
    }

    private void LateUpdate()
    {
        if (_isCheckingSkillRange && _aimHandler.HasValidAim)
        {
            _skillBuild.CheckSkillRange(_checkingSkillSlot, CreateSkillUseContext());
        }
    }

    public void SetSkill(SkillSlot slot, PlayerSkill skill)
    {
        _skillBuild.SetSkill(slot, skill);
    }

    public PlayerSkillData GetSkillInfo(SkillSlot slot)
    {
        return _skillBuild.GetSkillInfo(slot);
    }

    private void OnBasicAttack()
    {
        bool isExecute = TryExecuteSkill(SkillSlot.Basic);
        if (isExecute)
        {
            _animationController.PlayBasicAttack();
        }
    }

    private void OnSpecialAttack() => EndSkillRangeCheckAndExecute(SkillSlot.Special);
    private void OnUltimateAttack() => EndSkillRangeCheckAndExecute(SkillSlot.Ultimate);

    private void OnSpecialAttackCheck() => BeginSkillRangeCheck(SkillSlot.Special);
    private void OnUltimateSkillCheck() => BeginSkillRangeCheck(SkillSlot.Ultimate);

    private bool TryExecuteSkill(SkillSlot skillSlot)
    {
        //if (_isAnimating)
        //{
        //    Debug.Log($"{GetType()}: 애니메이션 실행 중이라 차단.");
        //    return false;
        //}

        if (_skillBuild.TryExecuteSkill(skillSlot, CreateSkillUseContext()))
        {
            //_isAnimating = true;
            return true;
        }

        return false;
    }

    private void BeginSkillRangeCheck(SkillSlot skillSlot)
    {
        if (!_isInitialized)
            return;

        _checkingSkillSlot = skillSlot;
        Debug.Log($"{GetType()}: 스킬 범위 체크 시작. 스킬 슬롯: {skillSlot}");
        _isCheckingSkillRange = true;
    }

    private void EndSkillRangeCheckAndExecute(SkillSlot skillSlot)
    {
        if (!_isInitialized)
            return;

        _isCheckingSkillRange = false;

        TryExecuteSkill(skillSlot);

        _skillBuild.HideSkillRange(skillSlot);
    }

    private PlayerSkillUseContext CreateSkillUseContext()
    {
        return new PlayerSkillUseContext(_aimHandler.transform, _aimHandler.AimDirection, _aimHandler.AimWorldPoint);
    }

    public void EndAnimationEvent()
    {
        //_isAnimating = false;
        Debug.Log($"{GetType()}: 애니메이션 종료 이벤트 호출.");
    }
}