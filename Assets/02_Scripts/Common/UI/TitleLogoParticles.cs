using UnityEngine;
using UnityEngine.UI;

public class TitleLogoParticles : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _particleRoot;
    [SerializeField] private Material _particleMaterial;

    [Header("Pool And Spawn")]
    [SerializeField] private int _particleCount = 12;
    [SerializeField] private Vector2 _spawnIntervalRange = new Vector2(0.35f, 0.75f);
    [SerializeField, Range(0f, 1f)] private float _outerSpawnRatio = 0.7f;
    [SerializeField, Range(0.1f, 0.49f)] private float _outerBoundary = 0.32f;
    [SerializeField] private Vector2 _spawnXRange = new Vector2(0.05f, 0.95f);
    [SerializeField] private Vector2 _spawnYRange = new Vector2(0.12f, 0.35f);

    [Header("Particle Appearance")]
    [SerializeField] private Color _particleColor = new Color(0.45f, 0.9f, 1f, 1f);
    [SerializeField] private Vector2 _sizeRange = new Vector2(4f, 9f);
    [SerializeField] private Vector2 _maxAlphaRange = new Vector2(0.35f, 0.7f);

    [Header("Particle Movement")]
    [SerializeField] private Vector2 _lifetimeRange = new Vector2(3.5f, 6f);
    [SerializeField] private Vector2 _riseDistanceRange = new Vector2(90f, 180f);
    [SerializeField] private Vector2 _driftRange = new Vector2(8f, 22f);
    [SerializeField] private Vector2 _driftCycleRange = new Vector2(0.5f, 1.2f);

    private ParticleState[] _particles;
    private float _spawnTimer;
    private bool _isPlaying;

    private sealed class ParticleState
    {
        public RectTransform RectTransform;
        public Image Image;
        public Vector2 StartPosition;
        public float Age;
        public float Lifetime;
        public float RiseDistance;
        public float DriftAmount;
        public float DriftCycles;
        public float DriftPhase;
        public float MaxAlpha;
        public float BaseSize;
        public bool IsActive;
    }

    private void Awake()
    {
        if (!InitializePool())
        {
            enabled = false;
            return;
        }

        StopAnimation();
    }

    private void Update()
    {
        if (!_isPlaying || _particles == null)
        {
            return;
        }

        float deltaTime = Time.unscaledDeltaTime;
        _spawnTimer -= deltaTime;

        if (_spawnTimer <= 0f)
        {
            SpawnParticle();
            ResetSpawnTimer();
        }

        for (int i = 0; i < _particles.Length; i++)
        {
            UpdateParticle(_particles[i], deltaTime);
        }
    }

    private void OnDisable()
    {
        StopAnimation();
    }

    public void PlayAnimation()
    {
        if (_particles == null && !InitializePool())
        {
            return;
        }

        _isPlaying = true;
        _spawnTimer = 0.15f;
    }

    public void StopAnimation()
    {
        _isPlaying = false;
        _spawnTimer = 0f;

        if (_particles == null)
        {
            return;
        }

        for (int i = 0; i < _particles.Length; i++)
        {
            DeactivateParticle(_particles[i]);
        }
    }

    private bool InitializePool()
    {
        if (_particleRoot == null || _particleMaterial == null)
        {
            Debug.LogWarning("TitleLogoParticles의 Root와 Material 연결을 확인해 주세요.", this);
            return false;
        }

        int safeParticleCount = Mathf.Max(1, _particleCount);
        _particles = new ParticleState[safeParticleCount];

        for (int i = 0; i < safeParticleCount; i++)
        {
            GameObject particleObject = new GameObject($"Firefly_{i + 1:00}", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            particleObject.layer = gameObject.layer;

            RectTransform particleRect = particleObject.GetComponent<RectTransform>();
            particleRect.SetParent(_particleRoot, false);
            particleRect.anchorMin = new Vector2(0.5f, 0.5f);
            particleRect.anchorMax = new Vector2(0.5f, 0.5f);
            particleRect.pivot = new Vector2(0.5f, 0.5f);

            Image particleImage = particleObject.GetComponent<Image>();
            particleImage.material = _particleMaterial;
            particleImage.raycastTarget = false;

            ParticleState particle = new ParticleState
            {
                RectTransform = particleRect,
                Image = particleImage
            };

            _particles[i] = particle;
            DeactivateParticle(particle);
        }

        return true;
    }

    private void SpawnParticle()
    {
        ParticleState particle = FindInactiveParticle();

        if (particle == null)
        {
            return;
        }

        Rect rect = _particleRoot.rect;
        float normalizedX = GetSpawnX();
        float normalizedY = Random.Range(_spawnYRange.x, _spawnYRange.y);

        particle.StartPosition = new Vector2(
            Mathf.Lerp(rect.xMin, rect.xMax, normalizedX),
            Mathf.Lerp(rect.yMin, rect.yMax, normalizedY));
        particle.Age = 0f;
        particle.Lifetime = Random.Range(_lifetimeRange.x, _lifetimeRange.y);
        particle.RiseDistance = Random.Range(_riseDistanceRange.x, _riseDistanceRange.y);
        particle.DriftAmount = Random.Range(_driftRange.x, _driftRange.y);
        particle.DriftCycles = Random.Range(_driftCycleRange.x, _driftCycleRange.y);
        particle.DriftPhase = Random.Range(0f, Mathf.PI * 2f);
        particle.MaxAlpha = Random.Range(_maxAlphaRange.x, _maxAlphaRange.y);
        particle.BaseSize = Random.Range(_sizeRange.x, _sizeRange.y);
        particle.IsActive = true;

        particle.RectTransform.anchoredPosition = particle.StartPosition;
        particle.RectTransform.sizeDelta = Vector2.one * particle.BaseSize;
        particle.Image.enabled = true;
        SetParticleAlpha(particle, 0f);
    }

    private void UpdateParticle(ParticleState particle, float deltaTime)
    {
        if (!particle.IsActive)
        {
            return;
        }

        particle.Age += deltaTime;
        float normalizedAge = Mathf.Clamp01(particle.Age / Mathf.Max(0.01f, particle.Lifetime));

        if (normalizedAge >= 1f)
        {
            DeactivateParticle(particle);
            return;
        }

        float drift = Mathf.Sin(
            particle.DriftPhase + normalizedAge * particle.DriftCycles * Mathf.PI * 2f) * particle.DriftAmount;
        particle.RectTransform.anchoredPosition = particle.StartPosition + new Vector2(
            drift,
            particle.RiseDistance * normalizedAge);

        float fadeIn = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(normalizedAge / 0.15f));
        float fadeOut = 1f - Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((normalizedAge - 0.15f) / 0.85f));
        SetParticleAlpha(particle, particle.MaxAlpha * fadeIn * fadeOut);

        float sizePulse = 1f + Mathf.Sin(particle.DriftPhase + normalizedAge * Mathf.PI * 2f) * 0.12f;
        particle.RectTransform.sizeDelta = Vector2.one * particle.BaseSize * sizePulse;
    }

    private float GetSpawnX()
    {
        if (Random.value > _outerSpawnRatio)
        {
            return Random.Range(_spawnXRange.x, _spawnXRange.y);
        }

        if (Random.value < 0.5f)
        {
            return Random.Range(_spawnXRange.x, Mathf.Min(_outerBoundary, _spawnXRange.y));
        }

        return Random.Range(Mathf.Max(1f - _outerBoundary, _spawnXRange.x), _spawnXRange.y);
    }

    private ParticleState FindInactiveParticle()
    {
        for (int i = 0; i < _particles.Length; i++)
        {
            if (!_particles[i].IsActive)
            {
                return _particles[i];
            }
        }

        return null;
    }

    private void DeactivateParticle(ParticleState particle)
    {
        particle.IsActive = false;
        particle.Image.enabled = false;
    }

    private void SetParticleAlpha(ParticleState particle, float alpha)
    {
        Color color = _particleColor;
        color.a = alpha;
        particle.Image.color = color;
    }

    private void ResetSpawnTimer()
    {
        _spawnTimer = Random.Range(_spawnIntervalRange.x, _spawnIntervalRange.y);
    }
}
