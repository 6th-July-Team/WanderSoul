using System.Collections.Generic;
using UnityEngine;

public class PeriodicBuffEffect : IStatusEffectExecution
{
    private IStatModifierReceiver _target;
    private StatModifier _modifier;

    private List<ModifierHandle> _handles = new();

    private bool _isApplied;


    public PeriodicBuffEffect(IStatModifierReceiver target, StatModifier modifier)
    {
        _target = target;
        _modifier = modifier;
    }

    public void OnApply()
    {
        if (_isApplied)
            return;

        _handles.Add(_target.AddModifier(_modifier));
        _isApplied = true;
    }

    public void OnStackChanged(int stack)
    {
        
    }

    public void OnTick()
    {
        _handles.Add(_target.AddModifier(_modifier));
    }

    public void OnRemove()
    {
        if (!_isApplied)
            return;

        foreach (var handle in _handles)
        {
            _target.RemoveModifier(handle);
        }

        Debug.Log($"{GetType()}: StatModifierEffect 삭제");

        _isApplied = false;
    }
}
