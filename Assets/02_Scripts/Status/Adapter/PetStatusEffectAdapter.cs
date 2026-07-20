using UnityEngine;

public class PetStatusEffectAdapter : IStatModifierReceiver
{
    private PetStatController _petStatController;

    public PetStatusEffectAdapter(PetStatController petStatController)
    {
        _petStatController = petStatController;
    }

    public ModifierHandle AddModifier(StatModifier modifier)
    {
        return _petStatController.AddModifier(modifier);
    }

    public float GetValue(StatType statType)
    {
        return _petStatController.GetValue(statType);
    }

    public void RemoveModifier(ModifierHandle handle)
    {
        _petStatController.RemoveModifiers(handle);
    }
}
