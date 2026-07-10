using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyHudUIView : BaseUI<PartyHudUIView, PartyHudViewModel>
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Slider _hpSlider;

    protected override void OnPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(PartyMemberModel.CurrentHp) || propertyName == nameof(PartyMemberModel.MaxHp))
        {
            RefreshHp();
        }

        else if (propertyName == nameof(PartyMemberModel.Name))
        {
            _nameText.text = _viewModel.Name;
        }
    }
    private void RefreshHp()
    {
        _hpSlider.value = _viewModel.HpFillAmount;
    }
}
