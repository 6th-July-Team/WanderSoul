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


    private void Awake()
    {
        if (_splineAnimate == null)
            _splineAnimate = GetComponent<SplineAnimate>();
    }

    private void OnEnable()
    {
        // TODO(김익환): 마차 정보 가져와서 설정
        var model = new WagonModel();
        model.Durability = 100f;

        // TODO(김익환): UI 쪽에서 가져와야 하지 않나?
        _wagonViewModel = new WagonViewModel(model);

        _wagonViewModel.OnPropertyChanged_View += OnPropertyChanged;
        _wagonViewModel.PropertyChangedOnInit();
    }

    private void OnDisable()
    {
        _wagonViewModel.OnPropertyChanged_View -= OnPropertyChanged;
        _wagonViewModel.Dispose();
        _wagonViewModel = null;
    }
    public void TakeDamage(DamageInfo damageInfo)
    {
        _wagonViewModel.ReduceDurability(damageInfo.DamageAmount);
    }

    #region Speed

    private void SetSpeed(float newSpeed)
    {
        ApplySmoothSpeedChange(newSpeed, _speedChangeDuration).Forget();
    }

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
                    }
                }
                break;
            case nameof(WagonModel.EnemyCount):
                {
                    // TODO(김익환): 데이터 드리븐 추가시 아래 주석 해제
                    //var data = GameManager.DataTable.GetWagonData();
                    //if(data.무슨카운터 == _wagonViewModel.GetEnemyCount)
                    //    SetSpeed(data.속도);
                }
                break;
        }
    }
}