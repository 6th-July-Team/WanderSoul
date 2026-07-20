using UnityEngine;

public class StatModifierEffect : IStatusEffectExecution
{
    private IStatModifierReceiver _target;
    private StatModifier _modifier;

    private ModifierHandle handle;

    private bool _isApplied;

    public StatModifierEffect(IStatModifierReceiver target, StatModifier modifier)
    {
        _target = target;
        _modifier = modifier;
    }

    public void OnApply()
    {
        if(_isApplied)
            return;

        handle = _target.AddModifier(_modifier);
        _isApplied = true;
    }

    public void OnTick()
    {
        
    }

    public void OnStackChanged(int stack)
    {
        
    }

    public void OnRemove()
    {
        if(!_isApplied)
            return;

        _target.RemoveModifier(handle);
        Debug.Log($"{GetType()}: StatModifierEffect 삭제");

        _isApplied = false;
    }

}
