using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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

    private PetCommandResult _commandResult;

    private bool _isInitialized = false;

    private CancellationTokenSource _aliveToken;

    private void Awake()
    {
        _isInitialized = false;
        _petMovement = GetComponent<PetMovement>();
    }

    public void Init(string petId, IPositionProvider playerAnchor, IPositionProvider wagonAnchor
        , PetSkillMaker petSkillMaker, IStatusEffectReceiver playerEffectReceiver, IHealable playerHealable
        , int avoidancePriority)
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

        _petViewModel = GameManager.Network.CreatePetViewModel(petId);

        _petViewModel.OnPropertyChanged_View += OnPropertyChanged;

        StatusEffects = new StatusEffectController();
        StatModifierReceiver = new PetStatusEffectAdapter(_petStatController);

        // 펫 스킬 강화가 생기면 플레이어 처럼 만들기.
        SkillModifierReceiver = null;

        _isInitialized = true;
    }

    private void Update()
    {
        if (false == _isInitialized)
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
        if (false == _isInitialized)
            return;

        _commandRefreshTimer += deltaTime;

        if (_commandRefreshTimer < _commandRefreshInterval)
            return;

        _commandRefreshTimer = 0f;

        GetCommandAndApply();
    }

    private void UpdateCombat()
    {
        if (false == _isInitialized)
            return;

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

    public void ApplyEffect(StatusEffectInstance instance)
    {
        StatusEffects.Apply(instance);
    }

    public void Dispose()
    {
        _isInitialized = false;

        DisposeToken();

        _combatController.Release();
        StatusEffects.Clear();
        _petStatController.ClearModifiers();


        if (null != _petViewModel)
            _petViewModel.OnPropertyChanged_View -= OnPropertyChanged;

        _commandResult = default;
        _petCommandController = null;
        _combatController = null;

        _petViewModel.Dispose();
        _petViewModel = null;

    }

    private void GetCommandAndApply()
    {
        _commandResult = _petCommandController.GetCommandResult();

        _petMovement.ApplyCommand(_commandResult);
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
                    if (_petViewModel.GetHp <= 0f)
                    {
                        // TODO(김익환): 사망 처리
                        // TODO(김익환): 사망 이펙트
                        // TODO(김익환): 사망 사운드


                        _aliveToken = new CancellationTokenSource();
                        DieAndRevive(_aliveToken.Token).Forget();
                        this.gameObject.SetActive(false);

                    }
                }
                break;
        }
    }

    private async UniTaskVoid DieAndRevive(CancellationToken token)
    {
        bool canceled = await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token).SuppressCancellationThrow();

        if (canceled)
            return;

        _petViewModel.SetHp(_petViewModel.GetMaxHp);
        this.gameObject.SetActive(true);

        DisposeToken();
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

    private void DisposeToken()
    {
        if (null != _aliveToken)
        {
            _aliveToken.Cancel();
            _aliveToken.Dispose();
            _aliveToken = null;
        }
    }
}