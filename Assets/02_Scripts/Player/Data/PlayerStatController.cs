using System;
using System.Collections.Generic;

public class PlayerStatController
{
    private readonly Dictionary<PlayerStatType, float> _baseValues = new();
    private readonly List<StatModifier> _modifiers = new();

    public PlayerStatController(PlayerStatData data)
    {
        for (int i = 0; i < (int)PlayerStatType.COUNT; i++)
        {
            _baseValues[(PlayerStatType)i] = data.GetBaseValue((PlayerStatType)i);
        }
    }

    public void SetBaseValue(PlayerStatType statType, float value)
    {
        _baseValues[statType] = value;
    }

    public float GetBaseValue(PlayerStatType statType)
    {
        return _baseValues.TryGetValue(statType, out float value) ? value : 0f;
    }

    public void AddModifier(StatModifier modifier)
    {
        _modifiers.Add(modifier);
    }

    public void RemoveModifiers(PlayerStatType statType)
    {
        _modifiers.RemoveAll(modifier => modifier.StatType == statType);
    }

    public void ClearModifiers()
    {
        _modifiers.Clear();
    }

    public float GetValue(PlayerStatType statType)
    {
        float baseValue = GetBaseValue(statType);

        float flat = 0f;
        float addPercent = 0f;
        float multipleMultiplier = 1f;

        foreach (StatModifier modifier in _modifiers)
        {
            if (modifier.StatType != statType)
                continue;

            switch (modifier.Operation)
            {
                case StatModifierOperation.Flat:
                    flat += modifier.Value;
                    break;

                case StatModifierOperation.AddPercent:
                    addPercent += modifier.Value;
                    break;

                case StatModifierOperation.MultiplePercent:
                    multipleMultiplier *= 1f + modifier.Value;
                    break;
            }
        }

        float result =
            (baseValue + flat) *
            (1f + addPercent) *
            multipleMultiplier;

        return ApplyLimit(statType, result);
    }

    private float ApplyLimit(PlayerStatType statType, float value)
    {
        return statType switch
        {
            // TODO(김익환): 아래 임시 값, 제한 값 따로 존재한다면 데이터 드리븐이로 가져오기
            PlayerStatType.FireResistance => Math.Clamp(value, 0f, 0.8f),
            PlayerStatType.ColdResistance => Math.Clamp(value, 0f, 0.8f),
            PlayerStatType.ElectricResistance => Math.Clamp(value, 0f, 0.8f),
            PlayerStatType.ElementalResistance => Math.Clamp(value, 0f, 0.8f),
            PlayerStatType.CooldownReduction => Math.Clamp(value, 0f, 0.6f),
            PlayerStatType.MoveSpeed => Math.Max(0f, value),
            _ => value
        };
    }
}
