

using System;

public static class StatusEffectResiter
{
    public static void ResisterAll(StatusEffectRegistry registry)
    {


        registry.Register(
            "StatModifier"
            , (createInfo) =>
            {
                StatusEffectData data = createInfo.EffectData;

                StatModifier modifier = new(data.GetStat(), data.GetOperation(), data.Value);

                return new StatModifierEffect(createInfo.Receiver.StatModifierReceiver, modifier);
            });

        registry.Register(
            "SkillModifier"
            , (createInfo) =>
            {
                SkillModifierData data = createInfo.SkillModifierData;

                var modifier = CreateSkillModifier(data);

                return new SkillModifierEffect(createInfo.Receiver.SkillModifierReceiver, modifier);
            });



    }

    private static SkillModifier CreateSkillModifier(SkillModifierData data)
    {
        return data.GetScope() switch
        {
            SkillModifierScope.Skill =>
                SkillModifier.ForSkill(data.SkillId, data.GetValueType(), data.GetOperation(), data.Value),

            SkillModifierScope.Slot =>
                SkillModifier.ForSlot(data.GetSkillSlot(), data.GetValueType(), data.GetOperation(), data.Value),

            SkillModifierScope.All =>
                SkillModifier.ForAll(data.GetValueType(), data.GetOperation(), data.Value),

            _ => throw new ArgumentOutOfRangeException(nameof(data), data.GetScope(), "지원하지 않는 SkillModifierScope입니다.")
        };
    }
}