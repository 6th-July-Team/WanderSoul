using System;
using System.Collections.Generic;

public class PetActiveSkillExecutionRegistry
{
    Dictionary<string, Func<PetSkillCreateInfo, IPetActiveSkillExecution>> _factories = new();

    public void Register(string skillId, Func<PetSkillCreateInfo, IPetActiveSkillExecution> factory)
    {
        if (_factories == null)
        {
            Logger.LogError($"{GetType()}: Factory가 null");

        }

        if (!_factories.TryAdd(skillId, factory))
        {
            Logger.LogError($"{GetType()}: 이미 등록된 펫 스킬");
        }
    }

    public IPetActiveSkillExecution Create(string skillId, PetSkillCreateInfo createInfo)
    {
        if (!_factories.TryGetValue(skillId, out var factory))
        {
            Logger.LogError($"{GetType()}: 등록되지 않은 펫 스킬 - ID: {skillId}");
            return null;
        }

        return factory.Invoke(createInfo);
    }
}
