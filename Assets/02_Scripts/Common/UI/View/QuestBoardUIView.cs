using System.Collections.Generic;
using UnityEngine;

public class QuestBoardUIView : BaseUI
{
    [SerializeField] private QuestBoardSlotUIView _slotPrefab;
    [SerializeField] private Transform _slotRoot;
    [SerializeField] private UIButton _closeButton;

    [Header("Animation")]
    [SerializeField] private UISlideAnimation _slideAnimation;

    private List<QuestBoardSlotUIView> _slotList = new();

    protected override void OnInit()
    {
        if (_closeButton != null)
        {
            _closeButton.BindOnClickButtonEvent(OnClickClose);
        }
    }

    protected override void OnOpened()
    {
        RefreshQuests();

        if (_slideAnimation == null)
        {
            return;
        }

        _slideAnimation.SetHidden();
        _slideAnimation.SlideIn();
    }

    protected override void OnClosed()
    {
        ClearSlots();

        GameManager.UI.CloseUI(UIType.QuestDetailUIView);
    }

    private void RefreshQuests()
    {
        ClearSlots();

        if (_slotPrefab == null || _slotRoot == null)
        {
            Debug.LogWarning("QuestBoardUIView의 슬롯 프리팹 또는 루트가 연결되지 않았습니다.");
            return;
        }

        List<QuestData> questList = GetSortedQuestList();

        foreach (QuestData questData in questList)
        {
            QuestBoardSlotUIView slot = Instantiate(_slotPrefab, _slotRoot);
            slot.SetQuest(questData, OnQuestSelected);

            _slotList.Add(slot);
        }
    }

    private List<QuestData> GetSortedQuestList()
    {
        List<QuestData> questList = new();

        foreach (QuestData questData in GameManager.DataTable.QuestDataTable.Values)
        {
            questList.Add(questData);
        }

        questList.Sort((left, right) => string.Compare(left.Id, right.Id));

        return questList;
    }

    private void OnQuestSelected(string questId)
    {
        GameManager.UI.OpenQuestDetailUI(questId);
    }

    private void OnClickClose()
    {
        GameManager.UI.CloseUI(UIType.QuestBoardUIView);
    }

    private void ClearSlots()
    {
        foreach (QuestBoardSlotUIView slot in _slotList)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }

        _slotList.Clear();
    }
}
