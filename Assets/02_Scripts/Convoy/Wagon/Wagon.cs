using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Splines;

public class Wagon : MonoBehaviour, ITargetable, IDamageable
{
    [SerializeField] private float _speedChangeDuration = 1f;

    public bool IsAlive => true;
    public Vector3 Position => transform.position;
    public EntityType EntityType => EntityType.Wagon;


    private SplineAnimate _splineAnimate;

    private WagonViewModel _wagonViewModel;

    private WagonSlowRuleData _wagonSlowRuleData;

    private float _baseMoveSpeed;
    private int _slowLevel = 0; // [0, 4]

    private void Awake()
    {
        if (_splineAnimate == null)
            _splineAnimate = GetComponent<SplineAnimate>();
    }

    private void OnEnable()
    {
        _splineAnimate.Completed += OnSplineCompleted;
    }

    private void OnDisable()
    {
        _splineAnimate.Completed -= OnSplineCompleted;

        _wagonViewModel.OnPropertyChanged_View -= OnPropertyChanged;
        _wagonViewModel.Dispose();
        _wagonViewModel = null;
    }

    private void Update()
    {
        _wagonViewModel.SetProgress(Mathf.Floor(_splineAnimate.NormalizedTime * 100) / 100);
    }

    public void Init(string wagonId, WagonViewModel wagonViewModel)
    {
        _wagonViewModel = wagonViewModel;

        WagonData wagonData = GameManager.DataTable.GetWagonData(wagonId);
        _wagonSlowRuleData = GameManager.DataTable.GetWagonSlowRuleData(wagonData.SlowDataId);

        _wagonViewModel.OnPropertyChanged_View += OnPropertyChanged;
        _wagonViewModel.PropertyChangedOnInit();

        InitChildComponent();
    }

    private void InitChildComponent()
    {
        GetComponentInChildren<WagonBoundary>().Init(_wagonViewModel);
        GetComponentInChildren<WagonMonsterCounter>().Init(_wagonViewModel);
    }

    public void SetSpline(SplineContainer splineContainer)
    {
        if (_splineAnimate == null)
            _splineAnimate = GetComponent<SplineAnimate>();

        _splineAnimate.Container = splineContainer;
        _splineAnimate.MaxSpeed = _wagonViewModel.GetMoveSpeed;
        _splineAnimate.Play();
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        _wagonViewModel.ReduceDurability(damageInfo.DamageAmount);
    }

    #region Speed

    //public void SetSpeed(float newSpeed)
    //{
    //    ApplySmoothSpeedChange(newSpeed, _speedChangeDuration).Forget();
    //}

    private async UniTask ApplySmoothSpeedChange(float targetSpeed, float duration)
    {
        float initialSpeed = _splineAnimate.MaxSpeed;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newSpeed = Mathf.Lerp(initialSpeed, targetSpeed, t);
            SetSpeedKeepingPosition(newSpeed);
            await UniTask.Yield();
        }

        SetSpeedKeepingPosition(targetSpeed);
    }

    private void SetSpeedKeepingPosition(float newSpeed)
    {
        newSpeed = Mathf.Max(0.001f, newSpeed);

        if (Mathf.Approximately(_splineAnimate.MaxSpeed, newSpeed))
            return;

        float normalizedTime = _splineAnimate.NormalizedTime;
        _splineAnimate.MaxSpeed = newSpeed;
        _splineAnimate.NormalizedTime = normalizedTime;
    }

    #endregion

    private void OnPropertyChanged(string propertyName)
    {
        switch(propertyName)
        {
            case nameof(WagonModel.Durability):
                {
                    // TODO(김익환): 피격 이펙트
                    // TODO(김익환): 피격 사운드
                    if (_wagonViewModel.GetDurability <= 0f)
                    {
                        // TODO(김익환): 마차 파괴 이펙트
                        // TODO(김익환): 체력 0 도달시 임무 실패 처리
                        GameManager.Convoy.FaildConvoy();
                    }
                }
                break;
            case nameof(WagonModel.EnemyCount):
                {
                    if(CheckChangeSpeed(_wagonViewModel.GetEnemyCount))
                    {
                        float newSpeed = _baseMoveSpeed * _wagonSlowRuleData.MoveSpeedRate[_slowLevel];
                        _wagonViewModel.SetMoveSpeed(newSpeed);
                        ApplySmoothSpeedChange(newSpeed, _speedChangeDuration).Forget();
                    }
                }
                break;
        }
    }

    private bool CheckChangeSpeed(int currentMonsterCnt)
    {
        // 이전 보다 몬스터 수가 많아지면 뒤쪽 Index로 가기
        // 아니라면 앞쪽 Index로 가기


        if (currentMonsterCnt < _wagonSlowRuleData.MinEnemyCount[_slowLevel])
        {
            if(_slowLevel > 0)
            {
                Debug.Log("Wagon: CheckChangeSpeed - Decrease Slow Level");
                _slowLevel--;
                return true;
            }
        }
        else if(currentMonsterCnt > _wagonSlowRuleData.MaxEnemyCount[_slowLevel])
        {
            if (_slowLevel < _wagonSlowRuleData.MoveSpeedRate.Count - 1)
            {
                Debug.Log("Wagon: CheckChangeSpeed - Increase Slow Level");
                _slowLevel++;
                return true;
            }
        }

        return false;
    }

    private void OnSplineCompleted()
    {
        GameManager.Convoy.SuccessConvoy();
    }
}