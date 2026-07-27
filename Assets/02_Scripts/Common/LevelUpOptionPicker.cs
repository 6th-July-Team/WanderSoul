using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpOptionPicker
{
    private readonly List<LevelUpOptionData> _candidates = new();

    private IReadOnlyDictionary<string, int> _pickedStackDic;

    public List<string> PickOptionIds(int level, string playerClassId, int pickCount
        , IReadOnlyDictionary<string, int> pickedStackDic)
    {
        _pickedStackDic = pickedStackDic;

        CollectCandidates(level, playerClassId);

        List<string> pickedIds = new();

        for (int i = 0; i < pickCount; i++)
        {
            LevelUpOptionData picked = PickByWeight();

            if (picked == null)
            {
                break;
            }

            pickedIds.Add(picked.Id);
            _candidates.Remove(picked);
        }

        return pickedIds;
    }

    private void CollectCandidates(int level, string playerClassId)
    {
        _candidates.Clear();

        foreach (var optionData in GameManager.DataTable.LevelUpOptionDataTable.Values)
        {
            if (CanPick(optionData, level, playerClassId) == false)
            {
                continue;
            }

            _candidates.Add(optionData);
        }
    }

    private bool CanPick(LevelUpOptionData optionData, int level, string playerClassId)
    {
        if (level < optionData.MinLevel)
        {
            return false;
        }

        // MaxLevel이 0이면 상한 없음
        if (optionData.MaxLevel > 0 && level > optionData.MaxLevel)
        {
            return false;
        }

        if (string.IsNullOrEmpty(optionData.RequiredClassId) == false
            && optionData.RequiredClassId != playerClassId)
        {
            return false;
        }

        if (optionData.MaxStack > 0 && GetPickedStack(optionData.Id) >= optionData.MaxStack)
        {
            return false;
        }

        return optionData.Weight > 0;
    }

    private int GetPickedStack(string optionId)
    {
        if (_pickedStackDic == null)
        {
            return 0;
        }

        if (_pickedStackDic.TryGetValue(optionId, out int stack) == false)
        {
            return 0;
        }

        return stack;
    }

    private LevelUpOptionData PickByWeight()
    {
        int totalWeight = 0;

        foreach (var candidate in _candidates)
        {
            totalWeight += candidate.Weight;
        }

        if (totalWeight <= 0)
        {
            return null;
        }

        int roll = UnityEngine.Random.Range(0, totalWeight);

        foreach (var candidate in _candidates)
        {
            roll -= candidate.Weight;

            if (roll < 0)
            {
                return candidate;
            }
        }

        return null;
    }

    public bool TryGetStatModifier(string optionId, out StatModifier modifier)
    {
        modifier = default;

        var optionData = GameManager.DataTable.GetLevelUpOptionData(optionId);

        if (optionData == null)
        {
            return false;
        }

        if (Enum.TryParse<StatType>(optionData.TargetStatType, out StatType statType) == false)
        {
            Debug.LogWarning($"레벨업 옵션의 StatType을 해석할 수 없습니다: {optionId} / {optionData.TargetStatType}");
            return false;
        }

        if (Enum.TryParse<StatModifierOperation>(optionData.Operation, out StatModifierOperation operation) == false)
        {
            Debug.LogWarning($"레벨업 옵션의 Operation을 해석할 수 없습니다: {optionId} / {optionData.Operation}");
            return false;
        }

        modifier = new StatModifier(statType, operation, optionData.Value);
        return true;
    }
}
