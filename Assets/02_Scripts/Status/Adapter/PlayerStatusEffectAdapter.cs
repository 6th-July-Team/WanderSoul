

public class PlayerStatusEffectAdapter : IStatModifierReceiver, ISkillModifierReceiver
{
    private PlayerStatController _playerStatController;
    private PlayerSkillModifier _playerSkillModifier;

    public PlayerStatusEffectAdapter(PlayerStatController playerStatController, PlayerSkillModifier playerSkillModifier)
    {
        _playerStatController = playerStatController;
        _playerSkillModifier = playerSkillModifier;
    }


    public ModifierHandle AddModifier(StatModifier modifier)
    {
        return _playerStatController.AddModifier(modifier);
    }

    public float GetValue(StatType statType)
    {
        return _playerStatController.GetValue(statType);
    }

    public void RemoveModifier(ModifierHandle handle)
    {
        _playerStatController.RemoveModifier(handle);
    }


    #region SkillModifier
    public ModifierHandle AddSkillModifier(SkillModifier modifier)
    {
        return _playerSkillModifier.AddModifier(modifier);
    }

    public void RemoveSkillModifier(ModifierHandle handle)
    {
        _playerSkillModifier.RemoveModifier(handle);
    }
    #endregion
}
