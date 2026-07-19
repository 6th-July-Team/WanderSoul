

public interface ISkillModifierReceiver
{
    ModifierHandle AddSkillModifier(SkillModifier modifier);
    void RemoveSkillModifier(ModifierHandle handle);
}