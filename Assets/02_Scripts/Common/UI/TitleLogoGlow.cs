using UnityEngine;
using UnityEngine.UI;

public class TitleLogoGlow : MonoBehaviour
{
    private static readonly int EffectTimeId = Shader.PropertyToID("_EffectTime");
    private static readonly int EffectAlphaId = Shader.PropertyToID("_EffectAlpha");

    [Header("References")]
    [SerializeField] private Image _glowImage;

    [Header("Fade In")]
    [SerializeField] private float _fadeInDuration = 1.5f;

    private Material _runtimeMaterial;
    private float _effectTime;
    private float _fadeElapsedTime;
    private bool _isPlaying;

    private void Awake()
    {
        InitializeMaterial();
        StopAnimation();
    }

    private void Update()
    {
        if (!_isPlaying || _runtimeMaterial == null)
        {
            return;
        }

        float deltaTime = Time.unscaledDeltaTime;
        _effectTime += deltaTime;
        _fadeElapsedTime += deltaTime;

        float safeFadeDuration = Mathf.Max(0.01f, _fadeInDuration);
        float effectAlpha = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(_fadeElapsedTime / safeFadeDuration));

        _runtimeMaterial.SetFloat(EffectTimeId, _effectTime);
        _runtimeMaterial.SetFloat(EffectAlphaId, effectAlpha);
    }

    private void OnDisable()
    {
        StopAnimation();
    }

    private void OnDestroy()
    {
        if (_runtimeMaterial != null)
        {
            Destroy(_runtimeMaterial);
        }
    }

    public void PlayAnimation()
    {
        if (_runtimeMaterial == null)
        {
            InitializeMaterial();
        }

        if (_runtimeMaterial == null)
        {
            return;
        }

        _effectTime = 0f;
        _fadeElapsedTime = 0f;
        _isPlaying = true;
        _glowImage.enabled = true;
        _runtimeMaterial.SetFloat(EffectTimeId, 0f);
        _runtimeMaterial.SetFloat(EffectAlphaId, 0f);
    }

    public void StopAnimation()
    {
        _effectTime = 0f;
        _fadeElapsedTime = 0f;
        _isPlaying = false;

        if (_runtimeMaterial != null)
        {
            _runtimeMaterial.SetFloat(EffectTimeId, 0f);
            _runtimeMaterial.SetFloat(EffectAlphaId, 0f);
        }

        if (_glowImage != null)
        {
            _glowImage.enabled = false;
        }
    }

    private void InitializeMaterial()
    {
        if (_glowImage == null || _glowImage.material == null)
        {
            Debug.LogWarning("TitleLogoGlow의 Image와 Material 연결을 확인해 주세요.", this);
            enabled = false;
            return;
        }

        _runtimeMaterial = new Material(_glowImage.material);
        _runtimeMaterial.name = $"{_glowImage.material.name} (Runtime)";
        _glowImage.material = _runtimeMaterial;
    }
}
