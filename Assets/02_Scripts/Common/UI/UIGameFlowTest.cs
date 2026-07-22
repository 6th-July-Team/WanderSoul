using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIGameFlowTest : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            ShowLevelUp();
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            ShowSuccessResult();
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            ShowFailResult();
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            GameManager.UI.OpenSimplePopup("테스트 알림입니다");
        }
        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            GameManager.UI.OpenSkillHudUI();
        }
    }

    private void ShowLevelUp()
    {
        var testIds = new List<string> { "opt_power_001", "opt_speed_001", "opt_crit_001" };
        GameManager.UI.OpenLevelUpUI(testIds);
    }

    private void ShowSuccessResult()
    {
        var result = new ConvoyResultModel();
        result.IsSuccess = true;
        result.ClearTime = 125f;
        result.IsNewRecord = true;
        result.KilledMonsterCount = 47;
        result.GainedSoul = 350;
        result.GoldReward = 100;
        result.ReputationReward = 50;

        GameManager.UI.OpenConvoySuccessUI(result);
    }

    private void ShowFailResult()
    {
        var result = new ConvoyResultModel();
        result.IsSuccess = false;
        result.FailReason = ConvoyFailReason.WagonDestroyed;
        result.ReputationPenalty = 10;
        result.RepairCost = 50;
        result.IsRepairCostPaid = false;
        result.ExtraReputationPenalty = 5;

        GameManager.UI.OpenConvoyFailUI(result);
    }
}