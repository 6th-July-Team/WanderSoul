using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillModifier
{
    private readonly Dictionary<int, SkillModifier> _modifiers = new();

    private int _nextModifierId = 0;

    public void Init()
    {
        // TODO (김익환): 저장된 데이터 불러와서 적용하기
    }

    public ModifierHandle AddModifier(SkillModifier modifier)
    {
        int id = ++_nextModifierId;

        _modifiers.Add(id, modifier);

        Debug.Log($"Added modifier: {modifier.ValueType} {modifier.Operation} {modifier.Value} (Scope: {modifier.Scope}) with ID: {id}");

        return new ModifierHandle(id);
    }

    public void RemoveModifier(ModifierHandle handle)
    {
        _modifiers.Remove(handle.Id);
    }

    public float GetValue(string skillId, SkillSlot skillSlot, SkillValueType valueType, float baseValue)
    {
        float flat = 0f;
        float addPercent = 0f;
        float multipleMultiplier = 1f;

        foreach (SkillModifier modifier in _modifiers.Values)
        {
            if (modifier.ValueType != valueType)
                continue;

            if (!IsValidation(modifier, skillId, skillSlot))
            {
                continue;
            }


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

        return (baseValue + flat)
            * (1f + addPercent)
            * multipleMultiplier;
    }

    public void ClearAll()
    {
        _modifiers.Clear();
    }

    private bool IsValidation(SkillModifier modifier, string skillId, SkillSlot skillSlot)
    {
        switch (modifier.Scope)
        {
            case SkillModifierScope.Skill:
                return modifier.SkillId == skillId;

            case SkillModifierScope.Slot:
                return modifier.SkillSlot == skillSlot;

            case SkillModifierScope.All:
                return true;

            default:
                return false;
        }
    }
}


public enum SkillValueType
{
    Power,
    Cooldown,
}

public enum SkillModifierScope
{
    Skill,
    Slot,
    All
}