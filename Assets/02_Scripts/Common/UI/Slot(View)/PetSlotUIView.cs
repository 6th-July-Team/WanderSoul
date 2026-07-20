using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PetSlotUIView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private UIButton _slotButton;
    [SerializeField] private Image _selectedImage;
    [SerializeField] private CanvasGroup _canvasGroup;

    private const float IN_PARTY_ALPHA = 0.4f;
    private const float DEFAULT_ALPHA = 1f;

    private long _petUniqueId;

    private event Action<long> OnSlotClicked;
    private event Action<long, PetSlotUIView> OnSlotHoverEnter;
    private event Action OnSlotHoverExit;

    public long PetUniqueId
    {
        get { return _petUniqueId; }
    }

    private void OnEnable()
    {
        _slotButton.BindOnClickButtonEvent(OnClickSlot);
    }

    public void InitSlot(PetSlotModel pet)
    {
        if (pet == null)
        {
            return;
        }

        _petUniqueId = pet.PetUniqueId;

        var petData = GameManager.DataTable.GetPetData(pet.PetDataId);
        if (petData == null)
        {
            return;
        }

        _nameText.text = petData.Name;
        RefreshIcon(petData.IconPath);
    }

    private void RefreshIcon(string iconPath)
    {
        if (string.IsNullOrEmpty(iconPath) == true)
        {
            return;
        }

        Sprite iconSprite = Utils.ResourcesLoad<Sprite>(iconPath);
        if (iconSprite != null)
        {
            _iconImage.sprite = iconSprite;
        }
    }

    public void BindSlotEvent(Action<long> onClicked, Action<long, PetSlotUIView> onHoverEnter, Action onHoverExit)
    {
        OnSlotClicked = onClicked;
        OnSlotHoverEnter = onHoverEnter;
        OnSlotHoverExit = onHoverExit;
    }

    private void OnClickSlot()
    {
        Debug.Log($"슬롯 클릭됨: {_petUniqueId}");

        if (OnSlotClicked != null)
        {
            OnSlotClicked.Invoke(_petUniqueId);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnSlotHoverEnter != null)
        {
            OnSlotHoverEnter.Invoke(_petUniqueId, this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnSlotHoverExit != null)
        {
            OnSlotHoverExit.Invoke();
        }
    }

    public void SetInPartyState(bool isInParty)
    {
        if (_selectedImage != null)
        {
            _selectedImage.gameObject.SetActive(isInParty);
        }

        if (_canvasGroup != null)
        {
            if (isInParty == true)
            {
                _canvasGroup.alpha = IN_PARTY_ALPHA;
            }
            else
            {
                _canvasGroup.alpha = DEFAULT_ALPHA;
            }
        }
    }

    private void OnDisable()
    {
        OnSlotClicked = null;
        OnSlotHoverEnter = null;
        OnSlotHoverExit = null;
    }
}