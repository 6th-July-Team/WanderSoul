using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(PlayerAimHandler))]
public class PlayerCombatController : MonoBehaviour
{
    // Components
    private PlayerInputHandle _inputHandle;
    private PlayerAimHandler _aimHandler;

    // Animator
    //private Animator _animator;
    //private bool _isAnimating = false;

    // Mana
    public ManaPool ManaPool { get; private set; }

    // Data
    private PlayerStatController _statController;

    // skill
    private PlayerClassSkillBuild _skillBuild;
    private PlayerClassSkillMaker _skillMaker;

    private bool isInitialized = false;

    private void Awake()
    {
        _inputHandle = GetComponent<PlayerInputHandle>();
        _aimHandler = GetComponent<PlayerAimHandler>();
        //_animator = GetComponent<Animator>();

        // TODO(김익환): 추후 Init 함수는 플레이어가 소환될 때 호출하기.
        Init().Forget();
    }

    // TEST: Awkae에서 호출하다 보니 플로우 문제가 있음 이후 삭제 예정
    public async UniTask Init()
    {
        await UniTask.WaitForSeconds(0.1f);
        PlayerStatData playerStatData = GameManager.DataTable.GetPlayerStatData("테스트 직업 아이디");
        _statController = new(playerStatData);

        ManaPool = new(_statController);
        _skillMaker = new(ManaPool);

        _skillBuild = _skillMaker.CreateSkillBuild("테스트 직업 아이디", _statController);

        isInitialized = true;
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
        if (GameManager.Time.IsPaused || !isInitialized)
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

    public PlayerSkillData GetSkillInfo(SkillSlot slot)
    {
        return _skillBuild.GetSkillInfo(slot);
    }


    private void OnBasicAttack()
    {
        TryExecuteSkill(SkillSlot.Basic);
        //if(TryExecuteSkill(SkillSlot.Basic))
        //{
        //    _animator.SetTrigger("isBasicAttak");
        //}    
    }
    private void OnSpecialAttack() => TryExecuteSkill(SkillSlot.Special);
    private void OnUltimateAttack() => TryExecuteSkill(SkillSlot.Ultimate);

    private bool TryExecuteSkill(SkillSlot skillSlot)
    {
        Debug.Log($"{GetType()}: 공격 시도.");

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

        Debug.Log($"{GetType()}: SkillBuild 호출 완료.");

        return false;
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