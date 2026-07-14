using System;
using System.Collections.Generic;

public class PetStatController
{
    private readonly Dictionary<PetStatType, float> _baseValues = new();
    private readonly List<PetStatModifier> _modifiers = new();

    public PetStatController(PetStatData data)
    {
        for (int i = 0; i < (int)PetStatType.COUNT; i++)
        {
            _baseValues[(PetStatType)i] = data.GetBaseValue((PetStatType)i);
        }
    }

    public void SetBaseValue(PetStatType statType, float value)
    {
        _baseValues[statType] = value;
    }

    public float GetBaseValue(PetStatType statType)
    {
        return _baseValues.TryGetValue(statType, out float value) ? value : 0f;
    }

    public void AddModifier(PetStatModifier modifier)
    {
        _modifiers.Add(modifier);
    }

    public void RemoveModifiers(PetStatType statType)
    {
        _modifiers.RemoveAll(modifier => modifier.StatType == statType);
    }

    public void ClearModifiers()
    {
        _modifiers.Clear();
    }

    public float GetValue(PetStatType statType)
    {
        float baseValue = GetBaseValue(statType);

        float flat = 0f;
        float addPercent = 0f;
        float multipleMultiplier = 1f;

        foreach (PetStatModifier modifier in _modifiers)
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

    private float ApplyLimit(PetStatType statType, float value)
    {
        return statType switch
        {
            // TODO(김익환): 아래 임시 값, 제한 값 따로 존재한다면 데이터 드리븐이로 가져오기
            // 임시 수치
            PetStatType.FireResistance => Math.Clamp(value, 0f, 0.8f),
            PetStatType.ColdResistance => Math.Clamp(value, 0f, 0.8f),
            PetStatType.ElectricResistance => Math.Clamp(value, 0f, 0.8f),
            PetStatType.MagicResistance => Math.Clamp(value, 0f, 0.8f),
            PetStatType.CooldownReduction => Math.Clamp(value, 0f, 0.6f),
            PetStatType.MoveSpeed => Math.Max(0f, value),
            _ => value
        };
    }
}
