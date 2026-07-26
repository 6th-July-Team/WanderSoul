using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDetailUIView : BaseUI<QuestDetailUIView, QuestViewModel>
{
    [Header("Quest")]
    [SerializeField] private TMP_Text _questNameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _goldRewardText;

    [Header("Difficulty")]
    [SerializeField] private Image[] _difficultyStarImages;
    [SerializeField] private Sprite _filledStarSprite;
    [SerializeField] private Sprite _emptyStarSprite;
    [SerializeField] private Color _filledStarColor = Color.white;
    [SerializeField] private Color _emptyStarColor = Color.white;

    [SerializeField] private TMP_Text _reputationRewardText;
    [SerializeField] private TMP_Text _startTownText;
    [SerializeField] private TMP_Text _arrivalTownText;

    [Header("Button")]
    [SerializeField] private UIButton _acceptButton;
    [SerializeField] private UIButton _closeButton;

    [Header("Animation")]
    [SerializeField] private UISlideAnimation _slideAnimation;

    private int _currentReputation;
    private Action _onAccepted;
    private bool _isClosing = false;

    protected override void OnInit()
    {
        _acceptButton.BindOnClickButtonEvent(OnClickAccept);

        if (_closeButton != null)
        {
            _closeButton.BindOnClickButtonEvent(OnClickClose);
        }
    }

    protected override void OnOpened()
    {
        _isClosing = false;

        if (_slideAnimation == null)
        {
            return;
        }

        _slideAnimation.SetHidden();
        _slideAnimation.SlideIn();
    }

    public void SetCurrentReputation(int currentReputation)
    {
        _currentReputation = currentReputation;
    }

    public void SetAcceptedCallback(Action onAccepted)
    {
        _onAccepted = onAccepted;
    }

    protected override void OnPropertyChanged(string propertyName)
    {

        if (propertyName == nameof(QuestModel.State))
        {
            RefreshQuest();
        }
    }

    private void RefreshQuest()
    {
        if (_viewModel == null)
        {
            return;
        }

        _questNameText.text = _viewModel.Name;
        _descriptionText.text = _viewModel.Description;
        _goldRewardText.text = _viewModel.GoldReward.ToString();

        RefreshDifficultyStars();
        RefreshOptionalTexts();
        RefreshAcceptButton();
    }

    private void RefreshDifficultyStars()
    {
        if (_difficultyStarImages == null)
        {
            return;
        }

        int filledCount = Mathf.Clamp(_viewModel.Difficulty, 0, _difficultyStarImages.Length);

        for (int i = 0; i < _difficultyStarImages.Length; i++)
        {
            if (_difficultyStarImages[i] == null)
            {
                continue;
            }

            if (i < filledCount)
            {
                SetStar(_difficultyStarImages[i], _filledStarSprite, _filledStarColor);
            }

            else
            {
                SetStar(_difficultyStarImages[i], _emptyStarSprite, _emptyStarColor);
            }
        }
    }

    private void SetStar(Image starImage, Sprite sprite, Color color)
    {

        if (sprite != null)
        {
            starImage.sprite = sprite;
        }

        starImage.color = color;
    }

    private void RefreshOptionalTexts()
    {
        if (_reputationRewardText != null)
        {
            _reputationRewardText.text = _viewModel.ReputationReward.ToString();
        }

        if (_startTownText != null)
        {
            _startTownText.text = GetTownName(_viewModel.StartTownId);
        }

        if (_arrivalTownText != null)
        {
            _arrivalTownText.text = GetTownName(_viewModel.ArrivalTownId);
        }
    }

    private string GetTownName(string townId)
    {
        var townData = GameManager.DataTable.GetTownData(townId);

        if (townData == null)
        {
            return string.Empty;
        }

        return townData.Name;
    }

    private void RefreshAcceptButton()
    {
        bool canAccept = _viewModel.CanAcceptQuest(_currentReputation);
        _acceptButton.SetInteractable(canAccept);
    }

    private void OnClickAccept()
    {
        if (_viewModel == null)
        {
            return;
        }

        if (_viewModel.TryAcceptQuest(_currentReputation) == false)
        {
            GameManager.UI.OpenSimplePopup("명성이 부족합니다.");
            return;
        }

        CloseWithSlide(_onAccepted);
    }

    private void OnClickClose()
    {
        CloseWithSlide(null);
    }

    private void CloseWithSlide(Action onClosed)
    {
        if (_isClosing == true)
        {
            return;
        }

        _isClosing = true;

        if (_slideAnimation == null)
        {
            Close(onClosed);
            return;
        }

        _slideAnimation.SlideOut(() => Close(onClosed));
    }

    private void Close(Action onClosed)
    {
        GameManager.UI.CloseUI(UIType.QuestDetailUIView);
        onClosed?.Invoke();
    }
}
