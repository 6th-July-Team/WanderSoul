using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIGameFlowTest : MonoBehaviour
{
    [Header("LevelUp Test")]
    [SerializeField] private string _testPlayerClassId = "player_scholar";
    [SerializeField] private bool _testUltimateAfterLevelUp = true;

    [Header("QuestDetail Test")]
    [SerializeField] private string _testQuestId = "quest_001";
    [SerializeField] private int _testReputation = 50;
    [SerializeField] private QuestState _testQuestState = QuestState.NotStarted;

    private readonly PetInventoryModel _testPetInventoryModel = new PetInventoryModel();

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
            ShowQuestDetail();
        }
    }

    private void ShowQuestDetail()
    {
        var questData = GameManager.DataTable.GetQuestData(_testQuestId);

        if (questData == null)
        {
            Debug.LogWarning($"QuestData를 찾을 수 없습니다: {_testQuestId}");
            return;
        }

        if (_testPetInventoryModel.PetList.Count == 0)
        {
            CreateTestPetList();
        }

        var questModel = new QuestModel(questData);
        questModel.State = _testQuestState;

        GameManager.UI.OpenQuestDetailUI(questModel, _testReputation, _testPetInventoryModel);
    }

    private void CreateTestPetList()
    {
        long uniqueId = 1;

        foreach (var petData in GameManager.DataTable.PetDataTable.Values)
        {
            var pet = new PetSlotModel();
            pet.PetUniqueId = uniqueId;
            pet.PetDataId = petData.Id;
            pet.Level = 1;
            _testPetInventoryModel.AddPet(pet);
            uniqueId++;
        }
    }

    private void ShowLevelUp()
    {
        var testIds = new List<string>
        {
            "levelup_health_common",
            "levelup_move_speed_rare",
            "levelup_all_stats_legendary",
        };

        GameManager.UI.OpenLevelUpUI(testIds, OnLevelUpOptionSelected, OnLevelUpUIClosed);
    }

    private void OnLevelUpOptionSelected(string optionId)
    {
        // TODO(이태영): 레벨업 로직이 붙으면 여기서 실제 스탯 적용 요청
        Debug.Log($"레벨업 옵션 선택: {optionId}");
    }

    // TODO(이태영): 레벨 시스템이 붙으면 '레벨 10 도달' 조건으로 교체
    private void OnLevelUpUIClosed()
    {
        if (_testUltimateAfterLevelUp == false)
        {
            return;
        }

        GameManager.UI.OpenUltimateSelectUI(_testPlayerClassId, OnUltimateSelected);
    }

    private void OnUltimateSelected(string skillId)
    {
        Debug.Log($"궁극기 선택: {skillId}");

        // TODO(이태영): PlayerSkillMaker.CreatePlayerSkill로 실제 스킬 교체 요청 필요
        GameManager.UI.SetSkillHudUltimate(skillId);
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