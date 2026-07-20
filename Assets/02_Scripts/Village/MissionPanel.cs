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

    private int _selectedQuestNumber;
    private bool _isMissionConfirmed;

    private void OnEnable()
    {
        _selectedQuestNumber = 0;
        _isMissionConfirmed = false;

        _questOneButton.interactable = true;
        _questTwoButton.interactable = true;
        _questThreeButton.interactable = true;

        _startMissionButton.interactable = false;
        _startMissionButtonText.text = "Start Mission";
    }

    public void SelectQuest(int questNumber)
    {
        if (_isMissionConfirmed)
        {
            return;
        }

        _selectedQuestNumber = questNumber;

        _questOneButton.interactable = questNumber != 1;
        _questTwoButton.interactable = questNumber != 2;
        _questThreeButton.interactable = questNumber != 3;

        _startMissionButton.interactable = true;

        Debug.Log($"Selected Quest: {_selectedQuestNumber}");
    }

    public void StartMission()
    {
        if (_selectedQuestNumber == 0)
        {
            return;
        }

        _isMissionConfirmed = true;

        _startMissionButton.interactable = false;
        _startMissionButtonText.text = "Already Selected";

        Debug.Log($"Starting Quest: {_selectedQuestNumber}");
    }
}
