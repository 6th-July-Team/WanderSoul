using UnityEngine;
using UnityEngine.UI;

public class TitleBarrierShimmer : MonoBehaviour
{
    private static readonly int EffectTimeId = Shader.PropertyToID("_EffectTime");
    private static readonly int PlayingId = Shader.PropertyToID("_Playing");

    [SerializeField] private Image _barrierImage;

    private Material _runtimeMaterial;
    private float _effectTime;
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

        _effectTime += Time.unscaledDeltaTime;
        _runtimeMaterial.SetFloat(EffectTimeId, _effectTime);
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
        _isPlaying = true;
        _barrierImage.enabled = true;
        _runtimeMaterial.SetFloat(EffectTimeId, 0f);
        _runtimeMaterial.SetFloat(PlayingId, 1f);
    }

    public void StopAnimation()
    {
        _effectTime = 0f;
        _isPlaying = false;

        if (_runtimeMaterial != null)
        {
            _runtimeMaterial.SetFloat(EffectTimeId, 0f);
            _runtimeMaterial.SetFloat(PlayingId, 0f);
        }

        if (_barrierImage != null)
        {
            _barrierImage.enabled = false;
        }
    }

    private void InitializeMaterial()
    {
        if (_barrierImage == null || _barrierImage.material == null)
        {
            Debug.LogWarning("TitleBarrierShimmer의 Image와 Material 연결을 확인해 주세요.", this);
            enabled = false;
            return;
        }

        _runtimeMaterial = new Material(_barrierImage.material);
        _runtimeMaterial.name = $"{_barrierImage.material.name} (Runtime)";
        _barrierImage.material = _runtimeMaterial;
    }
}
