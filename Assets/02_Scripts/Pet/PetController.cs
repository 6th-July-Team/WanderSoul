using Cysharp.Threading.Tasks;
using System;
using UnityEngine;


public class PetController : MonoBehaviour, ITargetable, IDamageable, IStatusEffectReceiver, IPet, IDisposable
{
    public StatusEffectController StatusEffects { get; private set; }
    public IStatModifierReceiver StatModifierReceiver { get; private set; }
    public ISkillModifierReceiver SkillModifierReceiver { get; private set; }


    public Vector3 Position => transform.position;
    public Transform Transform => this.transform;
    public EntityType EntityType => EntityType.Pet;
    public bool IsAlive => _petViewModel.GetHp > 0;
    public PetElement Element => _petData.GetElementType();

    // Data
    private PetData _petData;
    [SerializeField] private SOPetSearch __SOPetSearch; // 거리 체크 용 - 추후 삭제


    [Header("Command Setting")]
    [SerializeField] private float _commandRefreshInterval = 0.2f;
    private float _commandRefreshTimer;

    private PetMovement _petMovement;
    private PetStatController _petStatController;
    private PetCombatController _combatController;
    private PetCommandController _petCommandController;

    private PetViewModel _petViewModel;

    private bool isInitialized = false;

    private PetCommandResult _commandResult;


    private Renderer _petRenderer; // 기웅 : 펫 렌더러 << Awake에서 GetComponentInChildren으로 가져오게 했음 나중에 기호에 맞게 Parent나 그냥 GetComponent로 바꾸기 << 아직 프리팹 구조를 몰라서 이렇게 했음
    private bool _isDead = false; // 기웅 : 사망 했을 때 사망 처리가 끝나기 전에 DieAndRevive로 재집입 하는 걸 막기 위해서 임시로 추가 ( 비동기니까 ) << 나중에 맞는 위치로 이동해야 할듯

    [Header("Dissolve Effect")]
    [SerializeField] private float DissolveDuration = 1f; // 디졸브 유지시간 << 이 유지시간 동안 디졸브 이펙트 출력 (아마도?)

    private static readonly int DISSOLVE_ID = Shader.PropertyToID("_DissolveAmount"); // 디졸브 머터리얼 내부에 있는 프로퍼티를 int로 변환하는 메서드 << AI 도움 받음

    private void Awake()
    {
        isInitialized = false;
        _petMovement = GetComponent<PetMovement>();
        _petRenderer = GetComponentInChildren<Renderer>();
    }

    public void Init(string petId, IPositionProvider playerAnchor, IPositionProvider wagonAnchor
        , PetSkillMaker petSkillMaker, IStatusEffectReceiver playerEffectReceiver, IHealable playerHealable
        , int avoidancePriority, PetViewModel viewModel)
    {
        _petData = GameManager.DataTable.GetPetData(petId);

        PetStatData petStatData = GameManager.DataTable.GetPetStatData(petId);

        _petStatController = new(petStatData);

        _combatController = petSkillMaker.CreateCombatController(petId, _petStatController
            , playerEffectReceiver, playerHealable, StatModifierReceiver
            , this, this);

        // TODO(김익환): SO 제거
        _petCommandController = new(playerAnchor, wagonAnchor, this, __SOPetSearch, 32);

        _petMovement.Init(petId, avoidancePriority);

        _petViewModel = viewModel;

        _petViewModel.OnPropertyChanged_View += OnPropertyChanged;

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
        //if (_combatController.IsBusy)
        //{
        //    _petMovement.Stop();
        //    return;
        //}

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
        _petViewModel.SetHp(_petViewModel.GetHp - damageInfo.DamageAmount);
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

    public void ApplyEffect(StatusEffectInstance instance)
    {
        StatusEffects.Apply(instance);
    }

    private void ExecuteNoEnemySkill(PetActiveSkill skill)
    {
        PetSkillUseContext context = new(this, transform.position, skill.SkillData);

        _combatController.TryExecute(skill, context);
    }

    private void ExecuteEnemySkill(PetActiveSkill skill, ITargetable target)
    {
        if (null == target || !target.IsAlive)
            return;

        float castRange = skill.CastRange;

        _petMovement.SetCombatRange(castRange);

        if (!_petMovement.IsTargetInRange(target, castRange))
            return;

        _petMovement.Stop();

        PetSkillUseContext context = new PetSkillUseContext(this, transform.position, skill.SkillData);

        _combatController.TryExecute(skill, context);
    }

    private void OnPropertyChanged(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(PetViewModel.GetHp):
                {
                    if (_petViewModel.GetHp <= 0f && _isDead == false)
                    {
                        // TODO(김익환): 사망 사운드
                        Debug.Log("사망");
                        _isDead = true;
                        DieAndRevive().Forget(); // 기웅 : DieAndRevive에 사망 이펙트 및 소환 이펙트 추가함

                    }
                }
                break;
        }
    }

    private async UniTaskVoid DieAndRevive()
    {
        float live = 0f; // 디졸브에서 0은 오브젝트가 보이는 것
        float dead = 1f; // 디졸브에서 1은 오브젝트가 사라진 것

        await Dissolve(live, dead); // 생존 > 사망
        gameObject.SetActive(false);

        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        _petViewModel.SetHp(_petViewModel.GetMaxHp);

        gameObject.SetActive(true);
        await Dissolve(dead, live); // 사망 > 생존

        _isDead = false;
    }

    private async UniTask Dissolve(float start, float end)
    {
        float time = 0f;

        while (time < DissolveDuration)
        {
            time += Time.deltaTime;
            _petRenderer.material.SetFloat(DISSOLVE_ID, Mathf.Lerp(start, end, (time / DissolveDuration)));
            await UniTask.Yield();
        }

        _petRenderer.material.SetFloat(DISSOLVE_ID, end);
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

    public void Dispose()
    {
        StatusEffects.Clear();
        _petViewModel.OnPropertyChanged_View -= OnPropertyChanged;
    }

#if UNITY_EDITOR
    [ContextMenu("체력 1/2 감소시키기")] // 기웅 : 테스트용
    private void TakeHalfHpDamage()
    {
        Vector3 hitDirection = Vector3.forward;

        DamageInfo damageInfo = new(_petViewModel.GetMaxHp * 0.5f, hitDirection, DamageType.Physical);

        TakeDamage(damageInfo);
    }
#endif
}