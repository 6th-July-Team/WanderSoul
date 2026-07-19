

public class SkillModifierEffect : IStatusEffectExecution
{
    private ISkillModifierReceiver _target;
    private SkillModifier _modifier;

    private ModifierHandle _handle;

    private bool _isApplied;

    public SkillModifierEffect(ISkillModifierReceiver target, SkillModifier modifier)
    {
        _target = target;
        _modifier = modifier;
    }

    public void OnApply()
    {
        if (_isApplied)
            return;

        _handle = _target.AddSkillModifier(_modifier);

        _isApplied = true;
    }

    public void OnRemove()
    {
        if (!_isApplied)
            return;

        _target.RemoveSkillModifier(_handle);
        _handle = default;

        _isApplied = false;
    }

    public void OnStackChanged(int stack)
    {
        
    }

    public void OnTick()
    {
        
    }
}
