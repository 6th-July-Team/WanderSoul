using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberSlotUIView : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Image _hpFillImage;
    [SerializeField] private Image _elementImage;
    [SerializeField] private Image _iconImage;

    [Header("Colors")]
    [SerializeField] private Color _wagonColor;
    [SerializeField] private Color _perColor;

    public void SetWagon(string name, float hpFillAmount)
    {
        _nameText.text = name;
        _hpSlider.value = hpFillAmount;

        if (_hpFillImage != null)
        {
            _hpFillImage.color = _wagonColor;
        }

        if (_elementImage != null)
        {
            _elementImage.enabled = false;
        }

        if (_iconImage != null)
        {
            _iconImage.enabled = false;
        }
    }

    public void SetPet(string name, float hpFillAmount)
    {
        _nameText.text = name;
        _hpSlider.value = hpFillAmount;

        if (_hpFillImage != null)
        {
            _hpFillImage.color = _perColor;
        }

        if (_elementImage != null)
        {
            _elementImage.enabled = true;
        }

        if (_iconImage != null)
        {
            _iconImage.enabled = true;
        }
    }

    public void RefreshHp(float hpFillAmount)
    {
        _hpSlider.value = hpFillAmount;
    }
}
