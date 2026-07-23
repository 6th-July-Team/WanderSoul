using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIOrbLiquid : MonoBehaviour
{
    private static readonly int FillAmountId = Shader.PropertyToID("_FillAmount");
    private static readonly int HitImpulseId = Shader.PropertyToID("_HitImpulse");
    private static readonly int WavePhaseId = Shader.PropertyToID("_WavePhase");
    private static readonly int WaveSpeedMultiplierId = Shader.PropertyToID("_WaveSpeedMultiplier");

    [Header("Liquid Images")]
    [SerializeField] private Image _mainLiquidImage;
    [SerializeField] private Image _delayedLiquidImage;

    [Header("Decrease Animation")]
    [SerializeField] private float _delayBeforeDecrease = 0.25f;
    [SerializeField] private float _decreaseSpeed = 0.45f;

    [Header("Hit Animation")]
    [SerializeField] private float _hitImpulse = 0.8f;
    [SerializeField] private float _hitImpulseFadeSpeed = 3f;

    [Header("Wave Layer Separation")]
    [SerializeField, Range(0f, 1f)] private float _delayedWavePhase = 0.17f;
    [SerializeField, Range(0.5f, 1.5f)] private float _delayedWaveSpeedMultiplier = 0.93f;

    private Material _mainMaterial;
    private Material _delayedMaterial;
    private float _currentValue = 1f;
    private float _delayedValue = 1f;
    private float _decreaseDelayTimer;
    private float _currentHitImpulse;
    private bool _isInitialized;

    private void Awake()
    {
        if (_mainLiquidImage == null)
        {
            _mainLiquidImage = GetComponent<Image>();
        }

        _mainMaterial = CreateMaterialInstance(_mainLiquidImage);
        _delayedMaterial = CreateMaterialInstance(_delayedLiquidImage);
        ApplyValues();
    }

    private void Update()
    {
        UpdateDelayedValue();
        UpdateHitImpulse();
        ApplyValues();
    }

    private void OnDestroy()
    {
        DestroyMaterial(_mainMaterial);
        DestroyMaterial(_delayedMaterial);
    }

    public void SetValue(float normalizedValue)
    {
        float nextValue = Mathf.Clamp01(normalizedValue);

        if (!_isInitialized)
        {
            _currentValue = nextValue;
            _delayedValue = nextValue;
            _isInitialized = true;
            ApplyValues();
            return;
        }

        if (nextValue < _currentValue)
        {
            _decreaseDelayTimer = _delayBeforeDecrease;
            _currentHitImpulse = _hitImpulse;
        }
        else if (nextValue > _delayedValue)
        {
            _delayedValue = nextValue;
        }

        _currentValue = nextValue;
        ApplyValues();
    }

    private void UpdateDelayedValue()
    {
        if (_delayedValue <= _currentValue)
        {
            _delayedValue = _currentValue;
            return;
        }

        if (_decreaseDelayTimer > 0f)
        {
            _decreaseDelayTimer -= Time.unscaledDeltaTime;
            return;
        }

        _delayedValue = Mathf.MoveTowards(
            _delayedValue,
            _currentValue,
            _decreaseSpeed * Time.unscaledDeltaTime);
    }

    private void UpdateHitImpulse()
    {
        _currentHitImpulse = Mathf.MoveTowards(
            _currentHitImpulse,
            0f,
            _hitImpulseFadeSpeed * Time.unscaledDeltaTime);
    }

    private void ApplyValues()
    {
        if (_mainMaterial != null)
        {
            _mainMaterial.SetFloat(FillAmountId, _currentValue);
            _mainMaterial.SetFloat(HitImpulseId, _currentHitImpulse);
            _mainMaterial.SetFloat(WavePhaseId, 0f);
            _mainMaterial.SetFloat(WaveSpeedMultiplierId, 1f);
        }

        if (_delayedMaterial != null)
        {
            _delayedMaterial.SetFloat(FillAmountId, _delayedValue);
            _delayedMaterial.SetFloat(HitImpulseId, _currentHitImpulse * 0.35f);
            _delayedMaterial.SetFloat(WavePhaseId, _delayedWavePhase);
            _delayedMaterial.SetFloat(WaveSpeedMultiplierId, _delayedWaveSpeedMultiplier);
        }
    }

    private Material CreateMaterialInstance(Image targetImage)
    {
        if (targetImage == null || targetImage.material == null)
        {
            return null;
        }

        Material materialInstance = new Material(targetImage.material);
        materialInstance.name = $"{targetImage.material.name} ({name} Instance)";
        targetImage.material = materialInstance;
        return materialInstance;
    }

    private void DestroyMaterial(Material materialInstance)
    {
        if (materialInstance == null)
        {
            return;
        }

        Destroy(materialInstance);
    }
}