using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHudUIView : BaseUI
{

    [Header("HP")]
    [SerializeField] private Image _hpFillImage;
    [SerializeField] private TMP_Text _hpText;

    [Header("Mana")]
    [SerializeField] private Image _manaFillImage;
    [SerializeField] private TMP_Text _manaText;

    private ManaPool _manaPool;
    private float _lastMana = -1f;

    public void SetManaPool(ManaPool manaPool)
    {
        _manaPool = manaPool;
        _lastMana = -1f;
        RefreshMana();
    }

    private void Update()
    {
        if (_manaPool == null)
        {
            return;
        }

        float currentMana = _manaPool.CurrentMana;

        if (Mathf.Approximately(_lastMana, currentMana) == true)
        {
            return;
        }

        RefreshMana();
        _lastMana = currentMana;
    }

    private void RefreshMana()
    {
        if(_manaPool == null)
        {
            return;
        }

        float maxMana = _manaPool.MaxMana;
        if(maxMana <= 0f)
        {
            return;
        }

        _manaFillImage.fillAmount = _manaPool.CurrentMana / maxMana;
        _manaText.text = $"{(int)_manaPool.CurrentMana:N0} / {(int)maxMana:N0}";
    }
}
