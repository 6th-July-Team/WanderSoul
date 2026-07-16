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

    private PlayerSkill _skill;
    private ManaPool _manaPool;

    private float _lastCooldown = -1f;

    private const float DISABLED_ALPHA = 0.5f;

    public void SetSkill(PlayerSkill skill, ManaPool manaPool)
    {
        _skill = skill;
        _manaPool = manaPool;
        _lastCooldown = -1f;

        RefreshEmptyState();

        if (_skill == null)
        {
            return;
        }

        RefreshIcon();
        RefreshCooldown();
    }

    private void Update()
    {
        if(_skill == null)
        {
            return;
        }

        float remainingCooldown = _skill.RemainingCooldTime;

        if (Mathf.Approximately(_lastCooldown, remainingCooldown) == true)
        {
            RefreshUsable();
            return;
        }

        RefreshCooldown();
        _lastCooldown = remainingCooldown;
    }

    private void RefreshEmptyState()
    {
        bool isEmpty = (_skill == null);
        
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
    }

    private void RefreshIcon()
    {

        if (_skill == null)
        {
            return;
        }

        //TODO(이태영) : SOSkillDefuinition에 Icon 추가되면 연결
        //_iconImage.sprite = _skill.Definition.Icon;
    }

    private void RefreshCooldown()
    {
        
        if (_skill == null)
        {
            return;
        }

        float remaining = _skill.RemainingCooldTime;
        float total = _skill.SkillData.Cooldown;

        if (total <= 0f)
        {
            _cooldownFillImage.fillAmount = 0f;
            _cooldownText.gameObject.SetActive(false);
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
        if (_skill == null || _manaPool == null)
        {
            return;
        }

        bool hasEnoughMana = (_manaPool.CurrentMana >= _skill.SkillData.ManaCost);
        bool isUsable = (_skill.IsReady == true && hasEnoughMana == true);

        if (isUsable == true)
        {
            _canvasGroup.alpha = 1f;
        }
        else
        {
            _canvasGroup.alpha = DISABLED_ALPHA;
        }
    }

}

