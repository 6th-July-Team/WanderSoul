using System;
using System.Collections.Generic;

public class PlayerSkillRegistry
{
    private Dictionary<string, Func<PlayerSkillCreateInfo, IPlayerSkillExecution>> _factories = new();

    public void Register(string executionId, Func<PlayerSkillCreateInfo, IPlayerSkillExecution> factory)
    {
        if (_factories == null)
        {
            Logger.LogError($"{GetType()}: Factory가 null");

        }

        if (!_factories.TryAdd(executionId, factory))
        {
            Logger.LogError($"{GetType()}: 이미 등록된 플레이어 스킬");
        }
    }

    public IPlayerSkillExecution Create(string executionId, PlayerSkillCreateInfo createInfo)
    {
        if (!_factories.TryGetValue(executionId, out var factory))
        {
            Logger.LogError($"{GetType()}: 등록되지 않은 플레이어 스킬 - ID: {executionId}");
            return null;
        }

        return factory.Invoke(createInfo);
    }
}