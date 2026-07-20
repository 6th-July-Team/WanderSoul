using System.Collections.Generic;

public class StatusEffectController
{
    private List<StatusEffectInstance> _effects = new();

    public bool Apply(StatusEffectInstance newEffect)
    {
        if (newEffect == null)
            return false;

        StatusEffectStackPolicy stackPolicy = newEffect.StackPolicy;

        if (stackPolicy == StatusEffectStackPolicy.Independent)
        {
            Activate(newEffect);
            return true;
        }

        StatusEffectInstance existingEffect = FindById(newEffect.Id);

        if (null == existingEffect)
        {
            Activate(newEffect);
            return true;
        }

        switch (stackPolicy)
        {
            case StatusEffectStackPolicy.Ignore:
                return false;

            case StatusEffectStackPolicy.RefreshDuration:
                existingEffect.RefreshDuration();
                return true;

            case StatusEffectStackPolicy.AddStack:
                existingEffect.AddStack();
                existingEffect.RefreshDuration();
                return true;

            case StatusEffectStackPolicy.Replace:
                Remove(existingEffect);
                Activate(newEffect);
                return true;

            default:
                return false;
        }
    }

    public void Update(float deltaTime)
    {
        for (int i = _effects.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance effect = _effects[i];

            bool expired = effect.Update(deltaTime);

            if (expired)
            {
                effect.Remove();
                _effects.RemoveAt(i);
            }
        }
    }

    public void Clear()
    {
        for (int i = _effects.Count - 1; i >= 0; i--)
        {
            _effects[i].Remove();
        }

        _effects.Clear();
    }

    private void Activate(StatusEffectInstance effect)
    {
        effect.Apply();
        _effects.Add(effect);
    }

    private StatusEffectInstance FindById(string effectId)
    {
        for (int i = 0; i < _effects.Count; i++)
        {
            if (_effects[i].Id == effectId)
                return _effects[i];
        }

        return null;
    }

    private void Remove(StatusEffectInstance effect)
    {
        effect.Remove();
        _effects.Remove(effect);
    }
}
