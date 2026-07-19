using System;
using System.Collections.Generic;

public class StatusEffectRegistry
{
    Dictionary<string, Func<StatusEffectCreateInfo, IStatusEffectExecution>> _factories = new();

    public void Register(string executionId, Func<StatusEffectCreateInfo, IStatusEffectExecution> factory)
    {
        if(!_factories.TryAdd(executionId, factory))
        {
            Logger.LogWarning($"{GetType()}: 중복 등록, executionId: {executionId} ");
        }
    }

    public IStatusEffectExecution CreateExecution(string executionId, StatusEffectCreateInfo createInfo)
    {
        if (_factories.TryGetValue(executionId, out var factory))
        {
            return factory.Invoke(createInfo);
        }
        else
        {
            Logger.LogWarning($"{GetType()}: 등록되지 않은 executionId: {executionId} ");
            return null;
        }
    }
}
