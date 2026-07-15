using System.Collections.Generic;
using UnityEngine;

public class LevelUpUIView : BaseUI
{
    [SerializeField] private GameObject _optionSlotPrefab;
    [SerializeField] private Transform _slotRoot;

    List<LevelUpOptionSlotUIView> _slotList = new();

    public void SetOptions(List<string> optionIdList)
    {
        ClearSlots();
        if (optionIdList == null)
        {
            return;
        }

        foreach (var option in optionIdList)
        {
            CreateSlot(option);
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
        Debug.Log($"레벨업 선택: {optionId}");

        //TODO(이태영): 선택된 옵션 효과 적용 요청

        GameManager.UI.CloseUI(UIType.LevelUpUIView);
    }
}
