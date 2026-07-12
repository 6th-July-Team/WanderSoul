using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIView : BaseUI<InventoryUIView, InventoryViewModel>
{
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _slotRoot;
    [SerializeField] private UIButton _closeButton;

    [Header("Animation")]
    [SerializeField] private CanvasGroup _dimCanvasGroup;    
    [SerializeField] private RectTransform _topRect;     
    [SerializeField] private RectTransform _leftRect;
    [SerializeField] private RectTransform _rightRect;

    private const float ANIM_DURATION = 0.3f;
    private const float SLIDE_DISTANCE = 100f;

    private bool _isClosing = false;
    private bool _isPositionCached = false;

    private Vector2 _topOriginPos;
    private Vector2 _leftOriginPos;
    private Vector2 _rightOriginPos;

    private List<ItemSlotUIView> _slotList = new List<ItemSlotUIView>();

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
    }

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

    private void OnClickClose()
    {

        if (_isClosing == true)
        {
            return;
        }
        _isClosing = true;
        PlayCloseAnimation();
    }

    private void PlayCloseAnimation()
    {
        _dimCanvasGroup.DOFade(0f, ANIM_DURATION);

        _topRect.DOAnchorPos(_topOriginPos + new Vector2(0f, SLIDE_DISTANCE), ANIM_DURATION).SetEase(Ease.InCubic);

        _leftRect.DOAnchorPos(_leftOriginPos + new Vector2(-SLIDE_DISTANCE, 0f), ANIM_DURATION).SetEase(Ease.InCubic);

        _rightRect.DOAnchorPos(_rightOriginPos + new Vector2(SLIDE_DISTANCE, 0f), ANIM_DURATION).SetEase(Ease.InCubic).OnComplete(OnCloseAnimationComplete);
    }

    private void OnCloseAnimationComplete()
    {
        GameManager.Time.OnResume();
        GameManager.UI.CloseUI(UIType.InventoryUIView);
    }

    protected override void OnPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(InventoryModel.ItemList))
        {
            RefreshSlots();
        }
    }

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
}
