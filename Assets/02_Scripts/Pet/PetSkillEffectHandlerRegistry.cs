using System.Collections.Generic;
using UnityEngine;

/// 스킬 추가
/// Register 필요 없음
///   ㄴ 데이터만 추가
/// 
/// 새로운 효과 타입 추가
///   ㄴ Register 필요
///   ㄴ 코드도 추가해야 하므로 정상
///   필요한 부분임
   
public class PetSkillEffectHandlerRegistry
{
    private readonly Dictionary<EPetSkillEffectType, IPetSkillEffectHandler> _handlers = new();

    public PetSkillEffectHandlerRegistry()
    {
        // 위에서 말하는 부분
        Register(new DamageEffectHandler());
        Register(new ConeDamageEffectHandler());
        //Register(new AreaTauntEffectHandler());
        //Register(new ShieldEffectHandler());
        //Register(new HealEffectHandler());
        //Register(new ApplyStatusEffectHandler());
        
    }

    private void Register(IPetSkillEffectHandler handler)
    {
        if (_handlers.ContainsKey(handler.EffectType))
        {
            Debug.LogError($"중복 등록된 펫 스킬 효과입니다. EffectType: {handler.EffectType}");
            return;
        }

        _handlers.Add(handler.EffectType, handler);
    }

    public bool TryGetHandler(
        EPetSkillEffectType effectType,
        out IPetSkillEffectHandler handler)
    {
        return _handlers.TryGetValue(effectType, out handler);
    }

}


// 아래 효과 Handler 예시
// 1. 단일 피해
public class DamageEffectHandler : IPetSkillEffectHandler
{
    public EPetSkillEffectType EffectType => EPetSkillEffectType.Damage;

    public bool CanApply(
        PetSkillEffectData effectData,
        PetCombatRuntime runtime,
        PetCombatContext context)
    {
        return context.Target != null && context.Target.IsAlive;
    }

    public void Apply(
        PetSkillEffectData effectData,
        PetCombatRuntime runtime,
        PetCombatContext context)
    {
        Vector3 direction =
            (context.Target.Position - context.Pet.Position).normalized;

        float damageAmount =
            effectData.Value +
            context.Pet.AttackPower * effectData.Coefficient;

        DamageInfo damageInfo = new DamageInfo(
            attacker: context.Pet,
            sourceId: runtime.SkillData.SkillId,
            damageAmount: damageAmount,
            damageType: effectData.DamageType,
            hitDirection: direction,
            knockbackPower: effectData.KnockbackPower
        );

        if (context.Target is IDamageable damageable)
        {
            damageable.TakeDamage(damageInfo);
        }
    }
}

// 2. 부채꼴
public class ConeDamageEffectHandler : IPetSkillEffectHandler
{
    public EPetSkillEffectType EffectType => EPetSkillEffectType.ConeDamage;

    public bool CanApply(
        PetSkillEffectData effectData,
        PetCombatRuntime runtime,
        PetCombatContext context)
    {
        return context.Pet != null && context.Pet.IsAlive;
    }

    public void Apply(
        PetSkillEffectData effectData,
        PetCombatRuntime runtime,
        PetCombatContext context)
    {
        ITargetable target = SearchUtil.FindNearestTarget(
            context.Pet.Position,
            effectData.Radius,
            context.TargetLayerMask,
            context.SearchBuffer
        );

        if (target == null)
            return;

        Vector3 direction = (target.Position - context.Pet.Position).normalized;

        float damageAmount =
            effectData.Value +
            context.Pet.AttackPower * effectData.Coefficient;

        DamageInfo damageInfo = new DamageInfo(
            attacker: context.Pet,
            sourceId: runtime.SkillData.SkillId,
            damageAmount: damageAmount,
            damageType: effectData.DamageType,
            hitDirection: direction,
            knockbackPower: effectData.KnockbackPower
        );

        if (target is IDamageable damageable)
        {
            damageable.TakeDamage(damageInfo);
        }
    }
}

//// 3. 도발
//public class AreaTauntEffectHandler : IPetSkillEffectHandler
//{
//    public EPetSkillEffectType EffectType => EPetSkillEffectType.Taunt;

//    public bool CanApply(
//        PetSkillEffectData effectData,
//        PetCombatRuntime runtime,
//        PetCombatContext context)
//    {
//        return context.Pet != null && context.Pet.IsAlive;
//    }

//    public void Apply(
//        PetSkillEffectData effectData,
//        PetCombatRuntime runtime,
//        PetCombatContext context)
//    {
//        var targets = context.TargetQueryService.QuerySphere(
//            origin: context.Pet.Position,
//            radius: effectData.Radius
//        );

//        foreach (var target in targets)
//        {
//            if (target is not EnemyController enemy)
//                continue;

//            enemy.AggroController.ApplyTaunt(
//                tauntTarget: context.Pet,
//                duration: effectData.Duration
//            );
//        }
//    }
//}

public readonly struct DamageInfo
{
    public readonly object Attacker;
    public readonly string SourceId;

    public readonly float DamageAmount;
    public readonly EDamageType DamageType;

    public readonly Vector3 HitDirection;
    public readonly float KnockbackPower;

    public DamageInfo(
        object attacker,
        string sourceId,
        float damageAmount,
        EDamageType damageType,
        Vector3 hitDirection,
        float knockbackPower = 0f)
    {
        Attacker = attacker;
        SourceId = sourceId;
        DamageAmount = damageAmount;
        DamageType = damageType;
        HitDirection = hitDirection;
        KnockbackPower = knockbackPower;
    }
}