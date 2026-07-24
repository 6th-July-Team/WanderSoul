using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class ParticleComponent : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particles;
    [SerializeField] private TrailRenderer[] _trails;

    [Header("Hit VFX Option")]
    [SerializeField] private float _releaseDelay = 2f;

    public float ReleaseDelay => _releaseDelay;

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
        if (_particles == null || _particles.Length == 0)
        {
            _particles = GetComponentsInChildren<ParticleSystem>(true);
        }

        if (_trails == null || _trails.Length == 0)
        {
            _trails = GetComponentsInChildren<TrailRenderer>(true);
        }
    }

    public void Play()
    {
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

        Despawn().Forget();
    }

    public void Stop()
    {
        for (int i = 0; i < _particles.Length; i++)
        {
            _particles[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        for (int i = 0; i < _trails.Length; i++)
        {
            _trails[i].emitting = false;
            ClearTrails();
        }
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

        ClearTrails();
    }

    private void ClearTrails()
    {
        for (int i = 0; i < _trails.Length; i++)
        {
            _trails[i].Clear();
        }
    }

    public async UniTask Despawn()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_releaseDelay));
        Clear();
        GameManager.Pool.DespawnToPool(gameObject);
    }
}
