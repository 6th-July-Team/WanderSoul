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

        SetImageEnabled(_elementImage, false);
        SetImageEnabled(_iconImage, false);
    }

    public void SetPet(string petId, float hpFillAmount)
    {
        _hpSlider.value = hpFillAmount;

        if (_hpFillImage != null)
        {
            _hpFillImage.color = _perColor;
        }

        var petData = GameManager.DataTable.GetPetData(petId);

        if (petData == null)
        {
            Debug.LogWarning($"펫 데이터를 찾을 수 없습니다: {petId}");
            _nameText.text = string.Empty;
            SetImageEnabled(_elementImage, false);
            SetImageEnabled(_iconImage, false);
            return;
        }

        _nameText.text = petData.Name;

        RefreshSprite(_iconImage, petData.IconPath);
        RefreshSprite(_elementImage, GetElementIconPath(petData.GetElementType()));
    }

    private string GetElementIconPath(PetElement element)
    {
        if (element == PetElement.None)
        {
            return string.Empty;
        }

        return $"Sprites/UI/Icon/Element_{element}_128";
    }

    private void RefreshSprite(Image targetImage, string spritePath)
    {
        if (targetImage == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(spritePath) == true)
        {
            targetImage.enabled = false;
            return;
        }

        Sprite sprite = Utils.ResourcesLoad<Sprite>(spritePath);

        if (sprite == null)
        {
            Debug.LogWarning($"스프라이트를 찾을 수 없습니다: {spritePath}");
            targetImage.enabled = false;
            return;
        }

        targetImage.sprite = sprite;
        targetImage.enabled = true;
    }

    private void SetImageEnabled(Image targetImage, bool isEnabled)
    {
        if (targetImage != null)
        {
            targetImage.enabled = isEnabled;
        }
    }

    public void RefreshHp(float hpFillAmount)
    {
        _hpSlider.value = hpFillAmount;
    }
}
