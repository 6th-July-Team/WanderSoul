using System;
using System.Collections.Generic;

public class PetStatController
{
    private readonly Dictionary<StatType, float> _baseValues = new();
    private readonly Dictionary<int, StatModifier> _modifiers = new();

    private int _nextModifierId = 0;

    public PetStatController(PetStatData data)
    {
        for (int i = 0; i < (int)StatType.COUNT; i++)
        {
            _baseValues[(StatType)i] = data.GetBaseValue((StatType)i);
        }
    }

    public void SetBaseValue(StatType statType, float value)
    {
        _baseValues[statType] = value;
    }

    public float GetBaseValue(StatType statType)
    {
        return _baseValues.TryGetValue(statType, out float value) ? value : 0f;
    }

    public ModifierHandle AddModifier(StatModifier modifier)
    {
        int id = ++_nextModifierId;
        _modifiers.Add(id, modifier);
        return new ModifierHandle(id);
    }

    public void RemoveModifiers(ModifierHandle handle)
    {
        _modifiers.Remove(handle.Id);
    }

    public void ClearModifiers()
    {
        _modifiers.Clear();
    }

    public float GetValue(StatType statType)
    {
        float baseValue = GetBaseValue(statType);

        float flat = 0f;
        float addPercent = 0f;
        float multipleMultiplier = 1f;

        foreach (StatModifier modifier in _modifiers.Values)
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

        float result = (baseValue + flat) * (1f + addPercent) * multipleMultiplier;

        return ApplyLimit(statType, result);
    }

    private float ApplyLimit(StatType statType, float value)
    {
        return statType switch
        {
            // TODO(김익환): 아래 임시 값, 제한 값 따로 존재한다면 데이터 드리븐이로 가져오기
            // 임시 수치
            StatType.FireResistance => Math.Clamp(value, 0f, 0.8f),
            StatType.ColdResistance => Math.Clamp(value, 0f, 0.8f),
            StatType.ElectricResistance => Math.Clamp(value, 0f, 0.8f),
            StatType.MagicResistance => Math.Clamp(value, 0f, 0.8f),
            StatType.CooldownReduction => Math.Clamp(value, 0f, 0.6f),
            StatType.MoveSpeed => Math.Max(0f, value),
            _ => value
        };
    }
}
