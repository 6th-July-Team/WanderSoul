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

    [Header("Status")]
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private TMP_Text _statusDotText;
    [SerializeField] private string[] _statusMessages;
    [SerializeField] private float _statusInterval = 1.5f;
    [SerializeField] private float _dotInterval = 0.4f;

    private int _statusIndex = -1;
    private float _statusTimer = 0f;
    private float _dotTimer = 0f;
    private int _dotCount = 0;

    public void SetupRandom()
    {
        _targetProgress = 0f;

        if (_loadingBar != null)
        {
            _loadingBar.value = 0f;
        }

        _statusIndex = -1;
        _statusTimer = 0f;
        _dotTimer = 0f;
        _dotCount = 0;

        RefreshRandomTip();
        RefreshRandomBackground();
        RefreshNextStatus();
    }

    private void Update()
    {
        UpdateStatus();

        if (_loadingBar == null)
        {
            return;
        }

        _loadingBar.value = Mathf.MoveTowards(_loadingBar.value, _targetProgress
            , _fillSpeed * Time.unscaledDeltaTime);

        ChangeColorByProgress(_loadingBar.value);
    }

    private void UpdateStatus()
    {
        if (_statusText == null || _statusMessages == null || _statusMessages.Length == 0)
        {
            return;
        }

        _statusTimer += Time.unscaledDeltaTime;
        _dotTimer += Time.unscaledDeltaTime;

        if (_statusTimer >= _statusInterval)
        {
            _statusTimer = 0f;
            RefreshNextStatus();
            return;
        }

        if (_dotTimer >= _dotInterval)
        {
            _dotTimer = 0f;
            _dotCount = (_dotCount + 1) % 4;
            ApplyStatusText();
        }
    }

    private void RefreshNextStatus()
    {
        if (_statusText == null || _statusMessages == null || _statusMessages.Length == 0)
        {
            return;
        }

        int index = Random.Range(0, _statusMessages.Length);

        if (_statusMessages.Length > 1 && index == _statusIndex)
        {
            index = (index + 1) % _statusMessages.Length;
        }

        _statusIndex = index;
        _dotCount = 0;

        ApplyStatusText();
    }

    private void ApplyStatusText()
    {
        if (_statusIndex < 0 || _statusIndex >= _statusMessages.Length)
        {
            return;
        }

        string dots = new string('.', _dotCount);

        if (_statusDotText != null)
        {
            _statusText.text = _statusMessages[_statusIndex];
            _statusDotText.text = dots;
            return;
        }

        _statusText.text = _statusMessages[_statusIndex] + dots;
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
