using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClinicPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _treatmentCostText;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private Button _treatButton;

    [Header("Temporary Data")]
    [SerializeField] private int _currentHp = 35;
    [SerializeField] private int _maxHp = 100;
    [SerializeField] private int _gold = 500;
    [SerializeField] private int _treatmentCost = 130;

    private void OnEnable()
    {
        Refresh();

        if (_currentHp >= _maxHp)
        {
            _messageText.text = "치료가 필요하지 않습니다.";
        }
        else
        {
            _messageText.text = "치료가 필요하면 누르세요.";
        }
    }

    public void Treat()
    {
        if (_currentHp >= _maxHp)
        {
            _messageText.text = "치료가 필요하지 않습니다.";
            return;
        }

        if (_gold < _treatmentCost)
        {
            _messageText.text = "보유 골드가 부족합니다.";
            return;
        }

        _gold -= _treatmentCost;
        _currentHp = _maxHp;

        Refresh();
        _messageText.text = "치료가 완료되었습니다.";
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void Refresh()
    {
        _hpSlider.value = _maxHp > 0 ? _currentHp / (float)_maxHp : 0;
        _hpText.text = $"{_currentHp} / {_maxHp}";
        _treatmentCostText.text = $"치료 비용: {_treatmentCost} G";
        _goldText.text = $"보유 골드: {_gold} G";

        _treatButton.interactable = _currentHp < _maxHp && _gold >= _treatmentCost;
    }
}
