using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanel : MonoBehaviour
{
    [SerializeField] private Button _questOneButton;
    [SerializeField] private Button _questTwoButton;
    [SerializeField] private Button _questThreeButton;
    [SerializeField] private Button _startMissionButton;
    [SerializeField] private TMP_Text _startMissionButtonText;

    private readonly List<QuestData> _quests = new();

    private int _selectedQuestIndex = -1;
    private bool _isMissionConfirmed;

    private void OnEnable()
    {
        _selectedQuestIndex = -1;
        _isMissionConfirmed = false;

        LoadQuests();

        _startMissionButton.interactable = false;
        _startMissionButtonText.text = "의뢰 출발";
    }

    private void LoadQuests()
    {
        _quests.Clear();

        foreach (QuestData questData in GameManager.DataTable.QuestDataTable.Values)
        {
            _quests.Add(questData);
        }

        _quests.Sort((left, right) => string.Compare(left.Id, right.Id));

        SetQuestButton(_questOneButton, 0);
        SetQuestButton(_questTwoButton, 1);
        SetQuestButton(_questThreeButton, 2);
    }

    private void SetQuestButton(Button button, int questIndex)
    {
        bool hasQuest = questIndex < _quests.Count;

        button.interactable = hasQuest;

        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            buttonText.text = hasQuest ? _quests[questIndex].Name : "-";
        }
    }

    public void SelectQuest(int questNumber)
    {
        if (_isMissionConfirmed)
        {
            return;
        }

        int questIndex = questNumber - 1;

        if (questIndex < 0 || questIndex >= _quests.Count)
        {
            return;
        }

        _selectedQuestIndex = questIndex;

        RefreshQuestButtons();

        _startMissionButton.interactable = true;

        QuestData selectedQuest = _quests[_selectedQuestIndex];
        Debug.Log($"선택 의뢰: {selectedQuest.Id} / {selectedQuest.Name}");
    }

    public void StartMission()
    {
        if (_selectedQuestIndex < 0)
        {
            return;
        }

        _isMissionConfirmed = true;

        RefreshQuestButtons();

        _startMissionButton.interactable = false;
        _startMissionButtonText.text = "선택 완료";

        QuestData selectedQuest = _quests[_selectedQuestIndex];
        Debug.Log($"선택 의뢰: {selectedQuest.Id} / {selectedQuest.Name}");
    }

    private void RefreshQuestButtons()
    {
        _questOneButton.interactable = CanSelectQuest(0);
        _questTwoButton.interactable = CanSelectQuest(1);
        _questThreeButton.interactable = CanSelectQuest(2);
    }

    private bool CanSelectQuest(int questIndex)
    {
        if (_isMissionConfirmed || questIndex >= _quests.Count)
        {
            return false;
        }

        return questIndex != _selectedQuestIndex;
    }
}