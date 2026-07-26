using UnityEngine;
using UnityEngine.UI;

public class DashHudUIView : BaseUI
{
    [Header("Bar")]
    [SerializeField] private RectTransform _rootRect;
    [SerializeField] private Image[] _dashFillImages;

    [Header("Follow")]
    [SerializeField] private Vector2 _screenOffset = new Vector2(0f, -60f);

    private PlayerViewModel _viewModel;
    private Transform _followTarget;

    private float _totalChargeTime;
    private float _lastCoolTime = -1f;

    public void SetSource(PlayerViewModel viewModel, Transform followTarget)
    {
        _viewModel = viewModel;
        _followTarget = followTarget;

        _totalChargeTime = 0f;
        _lastCoolTime = -1f;

        RefreshBarCount();
        RefreshBars();
    }

    private void LateUpdate()
    {
        if (_viewModel == null)
        {
            return;
        }

        UpdateTotalChargeTime();
        UpdateFollowPosition();
        RefreshBars();
    }

    private void UpdateTotalChargeTime()
    {
        float coolTime = _viewModel.GetDashCoolTime;

        if (coolTime > _lastCoolTime)
        {
            _totalChargeTime = coolTime;
        }

        _lastCoolTime = coolTime;
    }

    private void UpdateFollowPosition()
    {
        if (_followTarget == null || _rootRect == null)
        {
            return;
        }

        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            return;
        }

        Vector3 screenPosition = mainCamera.WorldToScreenPoint(_followTarget.position);

        if (screenPosition.z < 0f)
        {
            _rootRect.gameObject.SetActive(false);
            return;
        }

        _rootRect.gameObject.SetActive(true);
        _rootRect.position = screenPosition + new Vector3(_screenOffset.x, _screenOffset.y, 0f);
    }

    private void RefreshBarCount()
    {
        if (_dashFillImages == null)
        {
            return;
        }

        int maxCount = (int)_viewModel.GetMaxDashCount;

        for (int i = 0; i < _dashFillImages.Length; i++)
        {
            if (_dashFillImages[i] == null)
            {
                continue;
            }

            _dashFillImages[i].transform.parent.gameObject.SetActive(i < maxCount);
        }
    }

    private void RefreshBars()
    {
        if (_dashFillImages == null)
        {
            return;
        }

        int currentCount = _viewModel.GetDashCount;
        float chargeRatio = GetChargeRatio();

        for (int i = 0; i < _dashFillImages.Length; i++)
        {
            if (_dashFillImages[i] == null)
            {
                continue;
            }

            _dashFillImages[i].fillAmount = GetFillAmount(i, currentCount, chargeRatio);
        }
    }

    private float GetFillAmount(int barIndex, int currentCount, float chargeRatio)
    {
        if (barIndex < currentCount)
        {
            return 1f;
        }

        if (barIndex == currentCount)
        {
            return chargeRatio;
        }

        return 0f;
    }

    private float GetChargeRatio()
    {
        if (_totalChargeTime <= 0f)
        {
            return 0f;
        }

        float remainTime = _viewModel.GetDashCoolTime;

        return Mathf.Clamp01((_totalChargeTime - remainTime) / _totalChargeTime);
    }
}
