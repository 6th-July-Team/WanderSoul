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

    private bool _isEffectPaused;
    private bool[] _trailEmittingStates;

    private void Awake()
    {
        CacheComponents();

        _trailEmittingStates = new bool[_trails.Length];
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        CacheComponents();
    }
#endif

    private void OnEnable()
    {
        GameManager.Time.Paused += PauseEffect;
        GameManager.Time.Resumed += ResumeEffect;
    }

    private void OnDisable()
    {
        GameManager.Time.Paused -= PauseEffect;
        GameManager.Time.Resumed -= ResumeEffect;
    }

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
        _isEffectPaused = false;

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

        if(GameManager.Time.IsPaused)
        {
            PauseEffect();
        }

        if (continuous)
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
        _isEffectPaused = false;

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
        _isEffectPaused = false;

        DisposeToken();

        Clear();

        GameManager.Pool.DespawnToPool(gameObject);
    }

    private async UniTask DespawnAfterDelay(CancellationToken token)
    {
        bool isCanceled = await GameManager.Time.WaitForGameSeconds(_releaseDelay, token);


        if (!isCanceled)
        {
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

    private void PauseEffect()
    {
        if (_isEffectPaused || _isReleased)
        {
            return;
        }

        _isEffectPaused = true;

        for (int i = 0; i < _particles.Length; i++)
        {
            ParticleSystem particle = _particles[i];

            if (particle == null)
            {
                continue;
            }

            if (particle.isPlaying)
            {
                particle.Pause(true);
            }
        }

        for (int i = 0; i < _trails.Length; i++)
        {
            TrailRenderer trail = _trails[i];

            if (trail == null)
            {
                continue;
            }

            _trailEmittingStates[i] = trail.emitting;
            trail.emitting = false;
        }
    }

    private void ResumeEffect()
    {
        if (!_isEffectPaused || _isReleased)
        {
            return;
        }

        _isEffectPaused = false;

        for (int i = 0; i < _particles.Length; i++)
        {
            ParticleSystem particle = _particles[i];

            if (particle == null)
            {
                continue;
            }

            if (particle.isPaused)
            {
                particle.Play(true);
            }
        }

        for (int i = 0; i < _trails.Length; i++)
        {
            TrailRenderer trail = _trails[i];

            if (trail == null)
            {
                continue;
            }

            trail.emitting = _trailEmittingStates[i];
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
