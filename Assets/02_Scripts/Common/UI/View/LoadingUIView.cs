using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUIView : BaseUI
{
    [Header("Progress")]
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private Image _sliderColorImage;
    [SerializeField] private Color[] _loadingBarColors;

    [Header("Tip")]
    [SerializeField] private TMP_Text _tipText;
    [SerializeField] private string[] _tips;

    [Header("Background")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Sprite[] _backgroundSprites;

    public void SetupRandom()
    {
        RefreshRandomTip();
        RefreshRandomBackground();
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
        float clamped = Mathf.Clamp01(progress);
        _loadingBar.value = clamped;
        ChangeColorByProgress(clamped);
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
