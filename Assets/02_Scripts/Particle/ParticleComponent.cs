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

    public void Clear()
    {
        for (int i = 0; i < _particles.Length; i++)
        {
            _particles[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        ClearTrails();
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    private void ClearTrails()
    {
        for (int i = 0; i < _trails.Length; i++)
        {
            _trails[i].Clear();
        }
    }

    private void OnDisable()
    {
        Stop();
    }
}
