using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIView : BaseUI<InventoryUIView, InventoryViewModel>
{
    #region Variables

    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _slotRoot;
    [SerializeField] private UIButton _closeButton;

    [Header("Animation")]
    [SerializeField] private CanvasGroup _dimCanvasGroup;    
    [SerializeField] private RectTransform _topRect;     
    [SerializeField] private RectTransform _leftRect;
    [SerializeField] private RectTransform _rightRect;

    [Header("Item Detail")]
    [SerializeField] private GameObject _detailPanel;
    [SerializeField] private TMP_Text _detailNameText;
    [SerializeField] private TMP_Text _detailTypeText;
    [SerializeField] private TMP_Text _detailDescriptionText;
    [SerializeField] private Image _detailIconImage;

    private const float ANIM_DURATION = 0.3f;
    private const float SLIDE_DISTANCE = 100f;

    private bool _isClosing = false;
    private bool _isPositionCached = false;

    private Vector2 _topOriginPos;
    private Vector2 _leftOriginPos;
    private Vector2 _rightOriginPos;

    private List<ItemSlotUIView> _slotList = new List<ItemSlotUIView>();

    #endregion

    protected override void OnInit()
    {
        _closeButton.BindOnClickButtonEvent(OnClickClose);
    }

    private void OnEnable()
    {
        _isClosing = false;
        CacheOriginPositions();
        GameManager.Time.OnPause();
        PlayOpenAnimation();

        _detailPanel.SetActive(false);
    }

    protected override void OnPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(InventoryModel.ItemList))
        {
            RefreshSlots();
        }
    }

    private void OnClickClose()
    {

        if (_isClosing == true)
        {
            return;
        }
        _isClosing = true;
        PlayCloseAnimation();
    }

    private void OnCloseAnimationComplete()
    {
        GameManager.Time.OnResume();
        GameManager.UI.CloseUI(UIType.InventoryUIView);
    }

    #region Animation
    private void CacheOriginPositions()
    {
        if (_isPositionCached == true)
        {
            return;
        }

        _topOriginPos = _topRect.anchoredPosition;
        _leftOriginPos = _leftRect.anchoredPosition;
        _rightOriginPos = _rightRect.anchoredPosition;

        _isPositionCached = true;
    }

    private void PlayOpenAnimation()
    {
        _dimCanvasGroup.alpha = 0f;
        _dimCanvasGroup.DOFade(1f, ANIM_DURATION);

        _topRect.anchoredPosition = _topOriginPos + new Vector2(0f, SLIDE_DISTANCE);
        _topRect.DOAnchorPos(_topOriginPos, ANIM_DURATION).SetEase(Ease.OutCubic);

        _leftRect.anchoredPosition = _leftOriginPos + new Vector2(-SLIDE_DISTANCE, 0f);
        _leftRect.DOAnchorPos(_leftOriginPos, ANIM_DURATION).SetEase(Ease.OutCubic);

        _rightRect.anchoredPosition = _rightOriginPos + new Vector2(SLIDE_DISTANCE, 0f);
        _rightRect.DOAnchorPos(_rightOriginPos, ANIM_DURATION).SetEase(Ease.OutCubic);
    }

    private void PlayCloseAnimation()
    {
        _dimCanvasGroup.DOFade(0f, ANIM_DURATION);

        _topRect.DOAnchorPos(_topOriginPos + new Vector2(0f, SLIDE_DISTANCE), ANIM_DURATION).SetEase(Ease.InCubic);

        _leftRect.DOAnchorPos(_leftOriginPos + new Vector2(-SLIDE_DISTANCE, 0f), ANIM_DURATION).SetEase(Ease.InCubic);

        _rightRect.DOAnchorPos(_rightOriginPos + new Vector2(SLIDE_DISTANCE, 0f), ANIM_DURATION).SetEase(Ease.InCubic).OnComplete(OnCloseAnimationComplete);
    }

    #endregion

    #region Slots

    private void RefreshSlots()
    {
        ClearSlots();

        var itemList = _viewModel.ItemList;
        if (itemList == null)
        {
            return;
        }

        foreach (var item in itemList)
        {
            CreateSlot(item);
        }
    }

    private void CreateSlot(ItemSlotModel item)
    {
        var slotObj = Instantiate(_slotPrefab, _slotRoot);
        if (slotObj == null)
        {
            return;
        }

        var slot = slotObj.GetComponent<ItemSlotUIView>();

        if (slot == null)
        {
            return;
        }

        slot.InitSlot(item);
        slot.BindSelectEvent(OnSlotSelected);
        _slotList.Add(slot);
    }

    private void ClearSlots()
    {
        foreach (var slot in _slotList)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }
        _slotList.Clear();
    }

    private void OnSlotSelected(long itemUniqueId)
    {
        _viewModel.SelectItem(itemUniqueId);

        foreach (var slot in _slotList)
        {
            bool isSelected = (slot.ItemUniqueId == itemUniqueId);
            slot.SetSelected(isSelected);
        }
        RefreshDetail();
    }

    #endregion

    #region Item Detail

    private void RefreshDetail()
    {
        var selectedItem = _viewModel.SelectedItem;

        if (selectedItem == null)
        {
            _detailPanel.SetActive(false);
            return;
        }

        var itemData = GameManager.DataTable.GetItemData(selectedItem.ItemDataId);
        if (itemData == null)
        {
            _detailPanel.SetActive(false);
            return;
        }

        _detailPanel.SetActive(true);

        _detailNameText.text = itemData.Name;
        _detailTypeText.text = itemData.ItemType;
        _detailDescriptionText.text = itemData.Description;

        RefreshDetailIcon(itemData.IconPath);
    }

    private void RefreshDetailIcon(string iconPath)
    {
        if (string.IsNullOrEmpty(iconPath) == true)
        {
            return;
        }

        Sprite iconSprite = Utils.ResourcesLoad<Sprite>(iconPath);
        if (iconSprite == null)
        {
            return;
        }

        _detailIconImage.sprite = iconSprite;
    }

    #endregion
}