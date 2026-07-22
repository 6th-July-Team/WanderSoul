using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUIView : BaseUI
{
    [SerializeField] private GameObject _optionSlotPrefab;
    [SerializeField] private Transform _slotRoot;
    [SerializeField] private CanvasGroup _dimCanvasGroup;

    List<LevelUpOptionSlotUIView> _slotList = new();

    private bool _isSelecting = false;

    // 선택된 옵션 Id를 바깥으로 알린다. 실제 효과 적용은 이 콜백을 받는 쪽 책임.
    private Action<string> _onOptionSelected;

    public void SetOptions(List<string> optionIdList, Action<string> onOptionSelected)
    {
        ClearSlots();
        _isSelecting = false;
        _onOptionSelected = onOptionSelected;

        if (optionIdList == null)
        {
            return;
        }

        foreach (var option in optionIdList)
        {
            CreateSlot(option);
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

    private void CreateSlot(string optionId)
    {
        var slotObj = Instantiate(_optionSlotPrefab, _slotRoot);

        if (slotObj == null)
        {
            return;
        }

        var slot = slotObj.GetComponent<LevelUpOptionSlotUIView>();

        if (slot == null)
        {
            return;
        }

        slot.InitSlot(optionId);
        slot.BindSelectEvent(OnOptionSelected);
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

    private void OnOptionSelected(string optionId)
    {
        if (_isSelecting == true)
        {
            return;
        }
        _isSelecting = true;

        Debug.Log($"레벨업 선택: {optionId}");

        if (_onOptionSelected != null)
        {
            _onOptionSelected.Invoke(optionId);
        }

        foreach (var slot in _slotList)
        {

            if (slot.OptionId == optionId)
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
        GameManager.UI.CloseUI(UIType.LevelUpUIView);
    }
}
