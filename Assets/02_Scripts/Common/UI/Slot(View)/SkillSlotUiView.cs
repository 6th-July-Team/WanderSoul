using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUiView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _cooldownFillImage;
    [SerializeField] private TMP_Text _cooldownText;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _emptySlotObject;

    [SerializeField] private GameObject _readyEffectObject;

    private SkillSlot _slot;
    private PlayerSkillData _skillData;

    private PlayerSkillViewModel _skillViewModel;
    private PlayerViewModel _playerViewModel;

    private float _lastCooldown = -1f;

    private const float DISABLED_ALPHA = 0.5f;

    public void SetSkill(SkillSlot slot, PlayerSkillData skillData
        , PlayerSkillViewModel skillViewModel, PlayerViewModel playerViewModel)
    {
        _slot = slot;
        _skillData = skillData;
        _skillViewModel = skillViewModel;
        _playerViewModel = playerViewModel;
        _lastCooldown = -1f;

        RefreshEmptyState();

        if (_skillData == null)
        {
            return;
        }

        RefreshIcon();
        RefreshCooldown();
    }

    private void Update()
    {
        if (IsBound() == false)
        {
            return;
        }

        float remainingCooldown = GetRemainingCooldown();

        if (Mathf.Approximately(_lastCooldown, remainingCooldown) == true)
        {
            RefreshUsable();
            return;
        }

        _lastCooldown = remainingCooldown;
        RefreshCooldown();
    }

    private bool IsBound()
    {
        if (_skillData == null)
        {
            return false;
        }

        if (_skillViewModel == null)
        {
            return false;
        }

        return true;
    }

    private float GetRemainingCooldown()
    {
        return _skillViewModel.GetSkillCoolTime(_slot);
    }

    private void RefreshEmptyState()
    {
        bool isEmpty = (_skillData == null);

        if (_emptySlotObject != null)
        {
            _emptySlotObject.SetActive(isEmpty);
        }

        if (_iconImage != null)
        {
            _iconImage.gameObject.SetActive(isEmpty == false);
        }

        if (_cooldownFillImage != null)
        {
            _cooldownFillImage.gameObject.SetActive(isEmpty == false);
        }

        if (_cooldownText != null && isEmpty == true)
        {
            _cooldownText.gameObject.SetActive(false);
        }

        if (_readyEffectObject != null && isEmpty == true)
        {
            _readyEffectObject.SetActive(false);
        }

        if (_canvasGroup != null && isEmpty == true)
        {
            _canvasGroup.alpha = DISABLED_ALPHA;
        }
    }

    private void RefreshIcon()
    {
        if (_skillData == null || _iconImage == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(_skillData.IconPath) == true)
        {
            return;
        }

        Sprite iconSprite = Utils.ResourcesLoad<Sprite>(_skillData.IconPath);

        if (iconSprite == null)
        {
            Debug.LogWarning($"스킬 아이콘을 찾을 수 없습니다: {_skillData.Id} / {_skillData.IconPath}");
            return;
        }

        _iconImage.sprite = iconSprite;
    }

    private void RefreshCooldown()
    {
        if (IsBound() == false)
        {
            return;
        }

        float remaining = GetRemainingCooldown();
        float total = _skillData.Cooldown;

        if (total <= 0f)
        {
            _cooldownFillImage.fillAmount = 0f;
            _cooldownText.gameObject.SetActive(false);
            RefreshUsable();
            return;
        }

        _cooldownFillImage.fillAmount = remaining / total;

        bool isOnCooldown = (remaining > 0f);
        _cooldownText.gameObject.SetActive(isOnCooldown);

        if (isOnCooldown == true)
        {
            _cooldownText.text = $"{remaining:F1}";
        }

        RefreshUsable();
    }

    private void RefreshUsable()
    {
        if (IsBound() == false)
        {
            return;
        }

        bool isReady = (GetRemainingCooldown() <= 0f);

        if (_readyEffectObject != null)
        {
            _readyEffectObject.SetActive(isReady);
        }

        if (_canvasGroup == null)
        {
            return;
        }

        bool hasEnoughMana = true;

        if (_playerViewModel != null)
        {
            hasEnoughMana = (_playerViewModel.GetMp >= _skillData.ManaCost);
        }

        if (isReady == true && hasEnoughMana == true)
        {
            _canvasGroup.alpha = 1f;
        }

        else
        {
            _canvasGroup.alpha = DISABLED_ALPHA;
        }
    }
}
