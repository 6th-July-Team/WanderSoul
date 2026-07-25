using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUIView : BaseUI
{
    [Header("Progress")]
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private Image _sliderColorImage;
    [SerializeField] private Color[] _loadingBarColors;
    [SerializeField] private float _fillSpeed = 1f;

    private float _targetProgress = 0f;

    [Header("Tip")]
    [SerializeField] private TMP_Text _tipText;
    [SerializeField] private string[] _tips;

    [Header("Background")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Sprite[] _backgroundSprites;

    public void SetupRandom()
    {
        _targetProgress = 0f;

        if (_loadingBar != null)
        {
            _loadingBar.value = 0f;
        }

        RefreshRandomTip();
        RefreshRandomBackground();
    }

    private void Update()
    {
        if (_loadingBar == null)
        {
            return;
        }

        _loadingBar.value = Mathf.MoveTowards(_loadingBar.value, _targetProgress
            , _fillSpeed * Time.unscaledDeltaTime);

        ChangeColorByProgress(_loadingBar.value);
    }
    private void RefreshRandomTip()
    {
        if (_tipText == null || _tips == null || _tips.Length == 0)
        {
            return;
        }

        int index = Random.Range(0, _tips.Length);
        _tipText.text = _tips[index];
    }

    private void RefreshRandomBackground()
    {
        if (_backgroundImage == null || _backgroundSprites == null || _backgroundSprites.Length == 0)
        {
            return;
        }

        int index = Random.Range(0, _backgroundSprites.Length);
        _backgroundImage.sprite = _backgroundSprites[index];
    }

    public void SetProgress(float progress)
    {
        _targetProgress = Mathf.Clamp01(progress);
    }

    public async UniTask WaitUntilFilledAsync()
    {
        while (this != null && gameObject.activeInHierarchy
            && _loadingBar != null && _loadingBar.value < _targetProgress)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    private void ChangeColorByProgress(float value)
    {
        if (value > 0.8f)
        {
            SetColor(3);
        }
        else if (value > 0.6f)
        {
            SetColor(2);
        }
        else if (value > 0.4f)
        {
            SetColor(1);
        }
        else
        {
            SetColor(0);
        }
    }

    private void SetColor(int index)
    {
        if (_sliderColorImage == null)
        {
            return;
        }

        if (index < _loadingBarColors.Length)
        {
            _sliderColorImage.color = _loadingBarColors[index];
        }
        else
        {
            _sliderColorImage.color = Color.white;
        }
    }
}
