using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class ParticleComponent : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particles;
    [SerializeField] private TrailRenderer[] _trails;

    [Header("Hit VFX Option")]
    [SerializeField] private float _releaseDelay = 2f;

    public float ReleaseDelay => _releaseDelay;

    private CancellationTokenSource _token;
    private bool _isReleased;

    private void Awake()
    {
        CacheComponents();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        CacheComponents();
    }
#endif

    private void CacheComponents()
    {
        _particles = GetComponentsInChildren<ParticleSystem>(true);
        _trails = GetComponentsInChildren<TrailRenderer>(true);

        for (int i = 0; i < _particles.Length; i++)
        {
            ParticleSystem particle = _particles[i];

            if (particle == null)
            {
                continue;
            }

            ParticleSystem.MainModule main = particle.main;
            main.stopAction = ParticleSystemStopAction.None;
        }

        for (int i = 0; i < _trails.Length; i++)
        {
            _trails[i].autodestruct = false;
        }
    }

    public void Play(bool continuous = false)
    {
        DisposeToken();

        _isReleased = false;

        Clear();

        for (int i = 0; i < _trails.Length; i++)
        {
            _trails[i].enabled = true;
            _trails[i].emitting = true;
        }

        for (int i = 0; i < _particles.Length; i++)
        {
            _particles[i].Play(true);
        }

        if(continuous)
        {
            return;
        }

        _token = new CancellationTokenSource();
        DespawnAfterDelay(_token.Token).Forget();
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void Clear()
    {
        for (int i = 0; i < _particles.Length; i++)
        {
            _particles[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        for (int i = 0; i < _trails.Length; i++)
        {
            _trails[i].emitting = false;
            _trails[i].Clear();
        }
    }

    public void ReleaseToPool()
    {
        if (_isReleased)
        {
            return;
        }

        _isReleased = true;

        DisposeToken();

        Clear();

        GameManager.Pool.DespawnToPool(gameObject);
    }

    private async UniTask DespawnAfterDelay(CancellationToken token)
    {
        bool isCanceled = await UniTask.Delay(TimeSpan.FromSeconds(_releaseDelay), cancellationToken: token)
            .SuppressCancellationThrow();

        if (isCanceled)
        {
            // 외부에서 ReleaseToPool()을 호출한 경우
            return;
        }

        ReleaseToPool();
    }

    private void DisposeToken()
    {
        if (_token != null)
        {
            _token.Cancel();
            _token.Dispose();
            _token = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!gameObject.activeSelf)
            return;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero, transform.localScale.x);
    }
#endif
}
