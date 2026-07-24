using Cysharp.Threading.Tasks;
using UnityEngine;

// [촉수 공격 신규] 촉수 모델의 이동과 파티클만 담당합니다.
// 공격 대상 탐색과 데미지는 EnemyAreaDelayObject가 계속 담당합니다.
public class TentacleEruptionEffect : MonoBehaviour
{
    [Header("촉수 모델")]
    [SerializeField] private Transform _tentaclePivot;
    [SerializeField] private Vector3 _hiddenLocalPosition = new Vector3(0f, -3f, 0f);
    [SerializeField] private Vector3 _exposedLocalPosition = Vector3.zero;

    [Header("시간")]
    [SerializeField] private float _riseDuration = 0.2f;
    [SerializeField] private float _holdDuration = 0.5f;
    [SerializeField] private float _hideDuration = 0.35f;

    [Header("이동 곡선")]
    [SerializeField] private AnimationCurve _riseCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve _hideCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("파티클")]
    [SerializeField] private ParticleSystem _groundDustParticle;
    [SerializeField] private ParticleSystem _impactParticle;

    public float RiseDuration => Mathf.Max(0f, _riseDuration);
    public float DurationAfterImpact => Mathf.Max(0f, _holdDuration) + Mathf.Max(0f, _hideDuration);

    // [촉수 공격 신규] 프리팹 생성 직후 모델을 지면 아래로 이동시킵니다.
    public void Prepare()
    {
        if (_tentaclePivot == null)
        {
            Logger.LogError($"{name}: {nameof(_tentaclePivot)}이 연결되지 않았습니다.");
            return;
        }

        _tentaclePivot.localPosition = _hiddenLocalPosition;
        StopParticle(_groundDustParticle);
        StopParticle(_impactParticle);
    }

    // [촉수 공격 신규] 예고 종료 직전에 호출되어 촉수를 지면 위로 올립니다.
    public async UniTask PlayRise(float duration)
    {
        if (_tentaclePivot == null)
        {
            return;
        }

        PlayParticle(_groundDustParticle);

        await MoveTentacle(
            _hiddenLocalPosition,
            _exposedLocalPosition,
            duration,
            _riseCurve);
    }

    // [촉수 공격 신규] 실제 데미지 시점에 충격 파티클을 재생하고 잠시 후 촉수를 내립니다.
    public async UniTaskVoid PlayImpactAndHide()
    {
        PlayParticle(_impactParticle);

        await Delay(_holdDuration);

        await MoveTentacle(
            _exposedLocalPosition,
            _hiddenLocalPosition,
            _hideDuration,
            _hideCurve);
    }

    private async UniTask MoveTentacle(
        Vector3 startPosition,
        Vector3 endPosition,
        float duration,
        AnimationCurve curve)
    {
        if (_tentaclePivot == null)
        {
            return;
        }

        if (duration <= 0f)
        {
            _tentaclePivot.localPosition = endPosition;
            return;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float ratio = Mathf.Clamp01(elapsed / duration);
            float curvedRatio = curve == null ? ratio : curve.Evaluate(ratio);

            _tentaclePivot.localPosition = Vector3.LerpUnclamped(
                startPosition,
                endPosition,
                curvedRatio);

            await UniTask.Yield(
                PlayerLoopTiming.Update,
                this.GetCancellationTokenOnDestroy());
        }

        _tentaclePivot.localPosition = endPosition;
    }

    private async UniTask Delay(float seconds)
    {
        if (seconds <= 0f)
        {
            return;
        }

        await UniTask.Delay(
            System.TimeSpan.FromSeconds(seconds),
            cancellationToken: this.GetCancellationTokenOnDestroy());
    }

    private void PlayParticle(ParticleSystem particle)
    {
        if (particle == null)
        {
            return;
        }

        particle.Play(true);
    }

    private void StopParticle(ParticleSystem particle)
    {
        if (particle == null)
        {
            return;
        }

        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
