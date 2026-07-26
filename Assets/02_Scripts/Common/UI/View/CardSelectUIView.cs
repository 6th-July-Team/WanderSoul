using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class CardSelectUIView : BaseUI
{
    [FormerlySerializedAs("_optionSlotPrefab")]
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _slotRoot;
    [SerializeField] private CanvasGroup _dimCanvasGroup;

    private List<SelectCardSlotUIView> _slotList = new();

    private bool _isSelecting = false;

    private Action<string> _onSlotSelected;
    private Action _onClosed;

    protected abstract UIType SelfUIType { get; }

    protected override void OnOpened()
    {
        GameManager.Time.OnPause();
    }

    protected override void OnClosed()
    {
        GameManager.Time.OnResume();
    }

    protected void SetSlots(List<string> idList, Action<string> onSelected, Action onClosed)
    {
        ClearSlots();

        _isSelecting = false;
        _onSlotSelected = onSelected;
        _onClosed = onClosed;

        if (idList == null)
        {
            return;
        }

        foreach (var id in idList)
        {
            CreateSlot(id);
        }

        PlayAppearSequence();
    }

    private void PlayAppearSequence()
    {
        if (_dimCanvasGroup != null)
        {
            _dimCanvasGroup.alpha = 0f;
            _dimCanvasGroup.DOFade(1f, 0.3f);
        }

        for (int i = 0; i < _slotList.Count; i++)
        {
            _slotList[i].PlayAppearAnimation(i * 0.12f);
        }
    }

    private void CreateSlot(string id)
    {
        var slotObj = Instantiate(_slotPrefab, _slotRoot);

        if (slotObj == null)
        {
            return;
        }

        var slot = slotObj.GetComponent<SelectCardSlotUIView>();

        if (slot == null)
        {
            Debug.LogWarning($"슬롯 프리팹에 {nameof(SelectCardSlotUIView)}가 없습니다: {_slotPrefab.name}");
            Destroy(slotObj);
            return;
        }

        slot.InitSlot(id);
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

    private void OnSlotSelected(string id)
    {
        if (_isSelecting == true)
        {
            return;
        }
        _isSelecting = true;

        Debug.Log($"{SelfUIType} 선택: {id}");

        if (_onSlotSelected != null)
        {
            _onSlotSelected.Invoke(id);
        }

        foreach (var slot in _slotList)
        {
            if (slot.SlotId == id)
            {
                slot.PlaySelectedAnimation(OnSelectedAnimationComplete);
            }

            else
            {
                slot.PlayUnselectedAnimation();
            }
        }
    }

    private void OnSelectedAnimationComplete()
    {
        GameManager.UI.CloseUI(SelfUIType);

        if (_onClosed != null)
        {
            _onClosed.Invoke();
        }
    }
}
