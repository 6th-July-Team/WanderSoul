using System;
using TMPro;
using UnityEngine;

public class QuestBoardSlotUIView : MonoBehaviour
{
    [SerializeField] private UIButton _selectButton;
    [SerializeField] private TMP_Text _nameText;

    private string _questId;
    private Action<string> _onSelected;

    public void SetQuest(QuestData questData, Action<string> onSelected)
    {
        if (questData == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        _questId = questData.Id;
        _onSelected = onSelected;

        if (_nameText != null)
        {
            _nameText.text = questData.Name;
        }

        if (_selectButton != null)
        {
            _selectButton.BindOnClickButtonEvent(OnClickSelect);
        }
    }

    private void OnClickSelect()
    {
        if (_onSelected == null)
        {
            return;
        }

        _onSelected.Invoke(_questId);
    }
}
