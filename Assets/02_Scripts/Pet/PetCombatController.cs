using System.Collections.Generic;
using UnityEngine;

/// 일반 공격 쿨타임 관리
// 특수 스킬 쿨타임 관리
/// 특수 스킬 쿨타임 관리
/// 현재 타겟 검증
/// 공격 가능 거리 확인
/// 스킬 실행자 호출
public class PetCombatController
{
    private PetCombatRuntime _normalAttack;
    private PetCombatRuntime _specialSkill;

    private PetSkillExecutor _skillExecutor;

    public void Init(PetSkillData normalAttackData, PetSkillData specialSkillData)
    {
        _normalAttack = new PetCombatRuntime(normalAttackData);
        _specialSkill = new PetCombatRuntime(specialSkillData);

        var registry = new PetSkillEffectHandlerRegistry();
        _skillExecutor = new PetSkillExecutor(registry);
    }

    public void Tick(PetCombatContext context, float deltaTime)
    {
        TickSkill(_normalAttack, context, deltaTime);
        TickSkill(_specialSkill, context, deltaTime);
    }

    private void TickSkill(
        PetCombatRuntime runtime,
        PetCombatContext context,
        float deltaTime)
    {
        if (runtime == null)
            return;

        runtime.TickCooldown(deltaTime);

        if (!runtime.CanUse)
            return;

        if (!HasValidTarget(runtime, context))
            return;

        _skillExecutor.Execute(runtime, context);
        runtime.ResetCooldown();
    }

    private bool HasValidTarget(PetCombatRuntime runtime, PetCombatContext context)
    {
        if (runtime.SkillData.Trigger == EPetSkillTrigger.AutoInterval)
            return true;

        return context.Target != null && context.Target.IsAlive;
    }
}

public class PetCombatRuntime
{
    public PetSkillData SkillData { get; }

    private float _cooldownTimer;

    public bool CanUse => _cooldownTimer <= 0f;

    public PetCombatRuntime(PetSkillData skillData)
    {
        SkillData = skillData;
        _cooldownTimer = 0f;
    }

    public void TickCooldown(float deltaTime)
    {
        if (_cooldownTimer > 0f)
            _cooldownTimer -= deltaTime;
    }

    public void ResetCooldown()
    {
        _cooldownTimer = SkillData.Cooldown;
    }
}

public class PetSkillData
{
    public string SkillId;
    public string DisplayName;

    public EPetSkillType SkillType;
    public EPetSkillTrigger Trigger;

    public float Cooldown;
    public float CastRange;

    public List<PetSkillEffectData> Effects;
}

public class PetSkillEffectData
{
    public EPetSkillEffectType EffectType;

    public float Value;
    public float Coefficient;
    public float Radius;
    public float Angle;
    public float Duration;

    public EDamageType DamageType;
    public ETargetSelectType TargetSelectType;

    public string StatusId;
    public string ProjectileId;
    public string VfxId;
    public float KnockbackPower;
}

public enum EPetSkillEffectType
{
    Damage,
    ConeDamage,
    AreaDamage,
    Projectile,
    Heal,
    Shield,
    Buff,
    Taunt,
    ApplyStatus,
    Knockback,
    Slow,
    COUNT
}

public enum ETargetSelectType
{             
    
}