using UnityEngine;

public class StatusEffectInstance
{
    public string Id => _data.Id;
    public StatusEffectStackPolicy StackPolicy => _data.GetStackPolicy();
    public int StackCount { get; private set; }
    public float RemainingDuration { get; private set; }

    private readonly StatusEffectData _data;
    private readonly IStatusEffectExecution _execution;

    private float _tickTimer;

    public StatusEffectInstance(StatusEffectData data, IStatusEffectExecution execution)
    {
        _data = data;
        _execution = execution;

        StackCount = 1;
        RemainingDuration = data.Duration;
    }

    public void Apply()
    {
        _execution.OnApply();

        _execution.OnTick();
    }

    public bool Update(float deltaTime)
    {
        RemainingDuration -= deltaTime;

        if (_data.TickInterval > 0f)
        {
            _tickTimer += deltaTime;

            if (_tickTimer >= _data.TickInterval)
            {
                _tickTimer -= _data.TickInterval;
                _execution.OnTick();
            }
        }

        return RemainingDuration <= 0f;
    }

    public void RefreshDuration()
    {
        RemainingDuration = _data.Duration;
    }

    public void AddStack()
    {
        int nextStack = Mathf.Min(StackCount + 1, _data.MaxStack);

        if (nextStack == StackCount)
            return;

        StackCount = nextStack;
        _execution.OnStackChanged(StackCount);
    }

    public void Remove()
    {
        _execution.OnRemove();
    }
}
