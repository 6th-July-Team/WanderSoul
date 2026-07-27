using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LevelUpOptionSparkleUIView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _particleRoot;
    [SerializeField] private Material _particleMaterial;

    [Header("Grade Particle Counts")]
    [SerializeField] private int _commonCount = 3;
    [SerializeField] private int _rareCount = 6;
    [SerializeField] private int _epicCount = 10;
    [SerializeField] private int _legendaryCount = 14;

    [Header("Spawn")]
    [SerializeField] private Vector2 _spawnIntervalRange = new Vector2(0.18f, 0.42f);
    [SerializeField] private Vector2 _spawnXRange = new Vector2(0.08f, 0.92f);
    [SerializeField] private Vector2 _spawnYRange = new Vector2(0.02f, 0.14f);

    [Header("Appearance")]
    [SerializeField] private Color _defaultPreviewColor = new Color(1f, 0.76f, 0.13f, 1f);
    [SerializeField, Range(0f, 1f)] private float _colorTowardWhite = 0.3f;
    [SerializeField] private Vector2 _sizeRange = new Vector2(4f, 10f);
    [SerializeField] private Vector2 _maxAlphaRange = new Vector2(0.3f, 0.75f);

    [Header("Movement")]
    [SerializeField] private Vector2 _lifetimeRange = new Vector2(2.2f, 4f);
    [SerializeField] private Vector2 _riseDistanceRange = new Vector2(120f, 260f);
    [SerializeField] private Vector2 _driftRange = new Vector2(5f, 18f);
    [SerializeField] private Vector2 _driftCycleRange = new Vector2(0.4f, 1f);

    private SparkleState[] _sparkles;
    private Color _particleColor = Color.white;
    private int _activeLimit;
    private float _spawnTimer;
    private bool _isPlaying;

#if UNITY_EDITOR
    private double _lastEditorTime;
#endif

    private sealed class SparkleState
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
        if (_sparkles == null && !InitializePool())
        {
            enabled = false;
            return;
        }

        PlayDefaultPreview();
    }

    private void OnEnable()
    {
        if (_sparkles == null && !InitializePool())
        {
            enabled = false;
            return;
        }

        PlayDefaultPreview();

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            _lastEditorTime = UnityEditor.EditorApplication.timeSinceStartup;
            UnityEditor.EditorApplication.update -= OnEditorUpdate;
            UnityEditor.EditorApplication.update += OnEditorUpdate;
        }
#endif
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        Tick(Time.unscaledDeltaTime);
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.update -= OnEditorUpdate;
#endif

        StopEffect();
    }

#if UNITY_EDITOR
    private void OnEditorUpdate()
    {
        if (Application.isPlaying || !isActiveAndEnabled)
        {
            return;
        }

        double currentTime = UnityEditor.EditorApplication.timeSinceStartup;
        float deltaTime = Mathf.Clamp((float)(currentTime - _lastEditorTime), 0f, 0.05f);
        _lastEditorTime = currentTime;

        Tick(deltaTime);
        UnityEditor.SceneView.RepaintAll();
    }
#endif

    private void Tick(float deltaTime)
    {
        if (!_isPlaying || _sparkles == null)
        {
            return;
        }

        _spawnTimer -= deltaTime;

        if (_spawnTimer <= 0f && GetActiveCount() < _activeLimit)
        {
            SpawnSparkle();
            ResetSpawnTimer();
        }

        for (int i = 0; i < _sparkles.Length; i++)
        {
            UpdateSparkle(_sparkles[i], deltaTime);
        }
    }

    public void ApplyGrade(string grade, Color gradeColor)
    {
        if (_sparkles == null && !InitializePool())
        {
            return;
        }

        _activeLimit = GetParticleCount(grade);
        _particleColor = Color.Lerp(gradeColor, Color.white, _colorTowardWhite);
        _particleColor.a = 1f;
        _isPlaying = _activeLimit > 0;
        _spawnTimer = 0f;

        TrimActiveSparkles();
    }

    private void PlayDefaultPreview()
    {
        ApplyGrade(string.Empty, _defaultPreviewColor);
    }

    public void StopEffect()
    {
        _isPlaying = false;
        _spawnTimer = 0f;

        if (_sparkles == null)
        {
            return;
        }

        for (int i = 0; i < _sparkles.Length; i++)
        {
            DeactivateSparkle(_sparkles[i]);
        }
    }

    private bool InitializePool()
    {
        if (_particleRoot == null || _particleMaterial == null)
        {
            Debug.LogWarning("LevelUpOptionSparkleUIView의 Particle Root와 Material 연결을 확인해 주세요.", this);
            return false;
        }

        int poolCount = Mathf.Max(1, _commonCount, _rareCount, _epicCount, _legendaryCount);
        _sparkles = new SparkleState[poolCount];

        for (int i = 0; i < poolCount; i++)
        {
            GameObject sparkleObject = new GameObject(
                $"Sparkle_{i + 1:00}",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image));
            sparkleObject.layer = gameObject.layer;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                sparkleObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor;
            }
#endif

            RectTransform sparkleRect = sparkleObject.GetComponent<RectTransform>();
            sparkleRect.SetParent(_particleRoot, false);
            sparkleRect.anchorMin = new Vector2(0.5f, 0.5f);
            sparkleRect.anchorMax = new Vector2(0.5f, 0.5f);
            sparkleRect.pivot = new Vector2(0.5f, 0.5f);

            Image sparkleImage = sparkleObject.GetComponent<Image>();
            sparkleImage.material = _particleMaterial;
            sparkleImage.raycastTarget = false;

            SparkleState sparkle = new SparkleState
            {
                RectTransform = sparkleRect,
                Image = sparkleImage
            };

            _sparkles[i] = sparkle;
            DeactivateSparkle(sparkle);
        }

        return true;
    }

    private int GetParticleCount(string grade)
    {
        if (grade == "Common")
        {
            return Mathf.Max(0, _commonCount);
        }
        else if (grade == "Rare")
        {
            return Mathf.Max(0, _rareCount);
        }
        else if (grade == "Epic")
        {
            return Mathf.Max(0, _epicCount);
        }
        else if (grade == "Legendary")
        {
            return Mathf.Max(0, _legendaryCount);
        }

        return Mathf.Max(0, _legendaryCount);
    }

    private void SpawnSparkle()
    {
        SparkleState sparkle = FindInactiveSparkle();

        if (sparkle == null)
        {
            return;
        }

        Rect rootRect = _particleRoot.rect;
        float normalizedX = Random.Range(_spawnXRange.x, _spawnXRange.y);
        float normalizedY = Random.Range(_spawnYRange.x, _spawnYRange.y);

        sparkle.StartPosition = new Vector2(
            Mathf.Lerp(rootRect.xMin, rootRect.xMax, normalizedX),
            Mathf.Lerp(rootRect.yMin, rootRect.yMax, normalizedY));
        sparkle.Age = 0f;
        sparkle.Lifetime = Random.Range(_lifetimeRange.x, _lifetimeRange.y);
        sparkle.RiseDistance = Random.Range(_riseDistanceRange.x, _riseDistanceRange.y);
        sparkle.DriftAmount = Random.Range(_driftRange.x, _driftRange.y);
        sparkle.DriftCycles = Random.Range(_driftCycleRange.x, _driftCycleRange.y);
        sparkle.DriftPhase = Random.Range(0f, Mathf.PI * 2f);
        sparkle.MaxAlpha = Random.Range(_maxAlphaRange.x, _maxAlphaRange.y);
        sparkle.BaseSize = Random.Range(_sizeRange.x, _sizeRange.y);
        sparkle.IsActive = true;

        sparkle.RectTransform.anchoredPosition = sparkle.StartPosition;
        sparkle.RectTransform.sizeDelta = Vector2.one * sparkle.BaseSize;
        sparkle.Image.enabled = true;
        SetSparkleAlpha(sparkle, 0f);
    }

    private void UpdateSparkle(SparkleState sparkle, float deltaTime)
    {
        if (!sparkle.IsActive)
        {
            return;
        }

        sparkle.Age += deltaTime;
        float normalizedAge = Mathf.Clamp01(sparkle.Age / Mathf.Max(0.01f, sparkle.Lifetime));

        if (normalizedAge >= 1f)
        {
            DeactivateSparkle(sparkle);
            return;
        }

        float drift = Mathf.Sin(
            sparkle.DriftPhase + normalizedAge * sparkle.DriftCycles * Mathf.PI * 2f) * sparkle.DriftAmount;
        sparkle.RectTransform.anchoredPosition = sparkle.StartPosition + new Vector2(
            drift,
            sparkle.RiseDistance * normalizedAge);

        float fadeIn = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(normalizedAge / 0.18f));
        float fadeOut = 1f - Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((normalizedAge - 0.18f) / 0.82f));
        SetSparkleAlpha(sparkle, sparkle.MaxAlpha * fadeIn * fadeOut);

        float sizePulse = 1f + Mathf.Sin(sparkle.DriftPhase + normalizedAge * Mathf.PI * 2f) * 0.15f;
        sparkle.RectTransform.sizeDelta = Vector2.one * sparkle.BaseSize * sizePulse;
    }

    private SparkleState FindInactiveSparkle()
    {
        for (int i = 0; i < _sparkles.Length; i++)
        {
            if (!_sparkles[i].IsActive)
            {
                return _sparkles[i];
            }
        }

        return null;
    }

    private int GetActiveCount()
    {
        int count = 0;

        for (int i = 0; i < _sparkles.Length; i++)
        {
            if (_sparkles[i].IsActive)
            {
                count++;
            }
        }

        return count;
    }

    private void TrimActiveSparkles()
    {
        int activeCount = GetActiveCount();

        for (int i = _sparkles.Length - 1; i >= 0 && activeCount > _activeLimit; i--)
        {
            if (_sparkles[i].IsActive)
            {
                DeactivateSparkle(_sparkles[i]);
                activeCount--;
            }
        }
    }

    private void DeactivateSparkle(SparkleState sparkle)
    {
        sparkle.IsActive = false;
        sparkle.Image.enabled = false;
    }

    private void SetSparkleAlpha(SparkleState sparkle, float alpha)
    {
        Color color = _particleColor;
        color.a = alpha;
        sparkle.Image.color = color;
    }

    private void ResetSpawnTimer()
    {
        _spawnTimer = Random.Range(_spawnIntervalRange.x, _spawnIntervalRange.y);
    }
}
