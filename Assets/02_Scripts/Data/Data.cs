using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseData
{
    public string Id;
}

[Serializable]
public class TownData : BaseData
{
    public string Name;
    public string RegionId;
    public string Description;
    public int StartReputation;
}

[Serializable]
public class RegionData : BaseData
{
    public string Name;
    public string Description;
    public string EmblemPath;
    public string HudFramePath;
}

[Serializable]
public class ReputationGradeData : BaseData
{
    public string Name;
    public int MinValue;
    public int MaxValue;
    public string IconPath;
}

[Serializable]
public class CharacterData : BaseData
{
    public string Name;
    public string PortraitPath;
    public string ElementType;
    public string ElementIconPath;
    public int MaxHp;
}

[Serializable]
public class ItemData : BaseData
{
    public string Name;
    public string Description;
    public string ItemType;
    public string Grade;
    public int MaxStackCount;
    public int SellingPrice;
    public string IconPath;
}

[Serializable]
public class QuestData : BaseData
{
    public string Name;
    public string Description;
    public string stringQuestType;
    public int Difficulty;
    public int RequiredReputation;
    public string StageId;
    public int GoldReward;
    public int ReputationReward;
    public string StartTownId;
    public string ArrivalTownId;
    public List<string> AutoSpawnIds;
}

[Serializable]
public class StageData : BaseData
{
    public string Name;
    public string StagePrefabPath;
    public string WagonId;
    public string TerrainType;
    public string Description;
}

[Serializable]
public class PoolData : BaseData
{
    public int InitSize;
}

[Serializable]
public class PreLoadAssetData : BaseData
{
    public string Address;
    public string AssetType;
}

[Serializable]
public class PlayerStatData : BaseData
{
    public float MaxHealth;
    public float AdditionalDamage;
    public float Defense;
    public float FireResistance;
    public float ColdResistance;
    public float ElectricResistance;
    public float MagicResistance;
    public float MoveSpeed;
    public float HealthRegeneration;
    public float ManaRegeneration;
    public float CooldownReduction;
    public float MaxMana;
    public float LifeSteal;
    public float CritChance;
    public float CritMultiplier;
    public float MagnetRadius;
    public int MaxReviveCount;
    public int MaxDashCount;

    public float GetBaseValue(StatType statType)
    {
        return statType switch
        {
            StatType.MaxHealth => MaxHealth,
            StatType.AdditionalDamage => AdditionalDamage,
            StatType.Defense => Defense,
            StatType.FireResistance => FireResistance,
            StatType.ColdResistance => ColdResistance,
            StatType.ElectricResistance => ElectricResistance,
            StatType.MagicResistance => MagicResistance,
            StatType.MoveSpeed => MoveSpeed,
            StatType.HealthRegeneration => HealthRegeneration,
            StatType.ManaRegeneration => ManaRegeneration,
            StatType.CooldownReduction => CooldownReduction,
            StatType.MaxMana => MaxMana,
            StatType.LifeSteal => LifeSteal,
            StatType.CritChance => CritChance,
            StatType.CritMultiplier => CritMultiplier,
            StatType.MaxReviveCount => MaxReviveCount,
            StatType.MaxDashCount => MaxDashCount,
            _ => 0f
        };
    }
}

[Serializable]
public class AutoSpawnData : BaseData
{
    public string Name;
    public string Description;
    public float StartTime;
    public float EndTime;
    public List<string> EnemyIds;
    public int MinPressureEnemyCount;     
    public float SpawnInterval;
    public int SpawnBatchCount;
}

[Serializable]
public class EnemyData : BaseData, ISerializationCallbackReceiver
{
    public string TargetPolicy; // PriortyBased == 기본 타입 // WagonOnly == 마차만 공격 // PlayerOnly == 플레이어만 공격
    public string EnemyAttackType; // 공격 타입 // Melee == 근접 // Projectile == 투사체발사(원거리) // AreaDelayed == 고정 포대형(예고 후 장판 공격) // Steal == 공격 없이 훔치기 기능만
    public string DamageType;

    [NonSerialized] public TargetPolicy Policy;
    [NonSerialized] public EnemyAttackType AttackType;
    [NonSerialized] public DamageType Type;

    public void OnAfterDeserialize()
    {
        if (Enum.TryParse(TargetPolicy, true, out Policy) == false)
        {
            DataLog.EnumParseFailed<TargetPolicy>(nameof(EnemyData), Id, TargetPolicy);
        }

        if (Enum.TryParse(EnemyAttackType, true, out AttackType) == false)
        {
            DataLog.EnumParseFailed<EnemyAttackType>(nameof(EnemyData), Id, EnemyAttackType);
        }

        if (Enum.TryParse(DamageType, true, out Type) == false)
        {
            DataLog.EnumParseFailed<EnemyAttackType>(nameof(EnemyData), Id, DamageType);
        }
    }

    public void OnBeforeSerialize() { }

    public string PrefabAddress;

    public string Name;
    public string Description;

    public int MaxHp;

    public float DetectRange;
    public float LeashRange;

    public int Attack;
    public float AttackSpeed;
    public float AttackRange;


    public float SoulDropChance;
    public int SoulDropAmount;

    public float ExpDropChance;
    public int ExpDropAmount;

    public bool CanMove;
    public float MoveSpeed;

    //투사체발사(원거리) 전용
    public float ProjectileSpeed;
    public float ProjectileLifeTime;
    public string ProjectilePrefabAddress;

    // 고정 포대형(예고 후 장판 공격) 전용
    public float AreaRadius;
    public float AreaDelayTime;
    public string AreaPrefabAddress;
}

[Serializable]
public class PetStatData : BaseData
{
    public float MaxHealth;
    public float BasicPower;
    public float Defense;
    public float FireResistance;
    public float ColdResistance;
    public float ElectricResistance;
    public float MagicResistance;
    public float MoveSpeed;
    public float HealthRegeneration;
    public float CooldownReduction;
    public float CritChance;
    public float CritMultiplier;
    public float LifeSteal;

    public float GetBaseValue(StatType statType)
    {
        return statType switch
        {
            StatType.MaxHealth => MaxHealth,
            StatType.BasicPower => BasicPower,
            StatType.Defense => Defense,
            StatType.FireResistance => FireResistance,
            StatType.ColdResistance => ColdResistance,
            StatType.ElectricResistance => ElectricResistance,
            StatType.MagicResistance => MagicResistance,
            StatType.MoveSpeed => MoveSpeed,
            StatType.HealthRegeneration => HealthRegeneration,
            StatType.CooldownReduction => CooldownReduction,
            StatType.CritChance => CritChance,
            StatType.CritMultiplier => CritMultiplier,
            StatType.LifeSteal => LifeSteal,
            _ => 0f
        };
    }
}
[Serializable]
public class PlayerSkillData : BaseData
{
    public string Name;
    public string Description;
    public float Power;
    public float Cooldown;
    public int ManaCost;
    public float Duration;
    public float ProjectileDuration;
    public float ProjectileSpeed;
    public int Pierce;
    public int MaxProjectileCount;
    public float Radius;
    public float CastRange;
    public float BarrierAbsorbAmount;
    public string stringTargetType;
    public string stringDamageType;
    public string StatusEffectId;
    public string SkillType;
    public string stringSkillDamageType;
    public string SkillOwnerType;
    public string VFXPath;
    public string SFXPath;
    public string IconPath;

    public DamageType GetDamageType()
        => Enum.TryParse<DamageType>(stringSkillDamageType, out var result) ? result : DamageType.Physical;

    public TargetType GetTargetType()
        => Enum.TryParse<TargetType>(stringTargetType, out var result) ? result : default;
}

[Serializable]
public class PetData : BaseData
{
    public string Name;
    public string Description;
    public List<string> ActiveSkillIds;
    public List<string> PassiveSkillIds;
    public string IconPath;

    public string stringPetGradeType;
    public string stringElementType;

    public PetElement GetElementType()
        => Enum.TryParse<PetElement>(stringElementType, out var result) ? result : PetElement.None;

    public Grade GetGrade()
        => Enum.TryParse<Grade>(stringPetGradeType, out var result) ? result : Grade.None;

}

[Serializable]
public class LevelUpOptionData : BaseData
{
    public string Name;
    public string Description;
    public string stringOptionCategory;
    public string stringGrade;
    public int MinLevel;
    public int MaxLevel;
    public string RequiredClassId;
    public string stringTargetStatType;
    public string stringTargetSkillSlot;
    public string stringEffectType;
    public string stringOperation;
    public float Value;
    public int MaxStack;
    public int Weight;
    public string IconPath;
}


[Serializable]
public class PetActiveSkillData : BaseData
{
    public string Name;
    public string Description;
    public string stringTargetType;
    public float Cooldown;
    public float Power;
    public float CooldownReduction;
    public float Duration;
    public float ProjectileSpeed;
    public int Pierce;                  // 투사체 관통력
    public int MaxProjectileCount;
    public string stringSkillType;      // 기본 공격? 특수 공격? 등
    public string ExecutionId;     // 급접이냐, 원거리냐 등
    public string stringSkillDamageType;
    public string VFXPath;
    public string SFXPath;
    public string IconPath;
    public string StatusEffectId;
    public float CastRange;
    public float Radius;

    public TargetType GetTargetType()
        => Enum.TryParse<TargetType>(stringTargetType, out var result) ? result : default;

    public SkillType GetSkillType()
        => Enum.TryParse<SkillType>(stringSkillType, out var result) ? result : SkillType.None;

    public DamageType GetDamageType()
        => Enum.TryParse<DamageType>(stringSkillDamageType, out var result) ? result : DamageType.None;
}

[Serializable]
public class PetPassiveSkillData : BaseData
{
    public string Name;
    public string Description;
    public string stringTargetType;
    public string StatusEffectId;
    public float ApllyChance;
    public string ExecutionId;
    public float Radius;

    public TargetType GetTargetType()
        => Enum.TryParse<TargetType>(stringTargetType, out var result) ? result : default;
}

[Serializable]
public class StatusEffectData : BaseData
{
    public string ExecutionId;
    public string StackPolicy;
    public float Duration;
    public float TickInterval;
    public int MaxStack;
    public float Value;

    public string stringStatType;
    public string stringOperation;

    public string SkillModifierId;

    public StatusEffectStackPolicy GetStackPolicy()
        => Enum.TryParse<StatusEffectStackPolicy>(StackPolicy, out var result) ? result : StatusEffectStackPolicy.Ignore;
    public StatType GetStat()
        => Enum.TryParse<StatType>(stringStatType, out var result) ? result : default;
    public StatModifierOperation GetOperation()
       => Enum.TryParse<StatModifierOperation>(stringOperation, out var result) ? result : default;
}

public class SkillModifierData : BaseData
{
    public string stringScope;
    public string SkillId;
    public string stringSkillSlot;
    public string stringValueType;
    public string stringOperation;
    public float Value;

    public SkillModifierScope GetScope()
        => Enum.TryParse<SkillModifierScope>(stringScope, out var result) ? result : default;

    public SkillSlot GetSkillSlot()
        => Enum.TryParse<SkillSlot>(stringSkillSlot, out var result) ? result : default;

    public SkillValueType GetValueType()
        => Enum.TryParse<SkillValueType>(stringValueType, out var result) ? result : default;

    public StatModifierOperation GetOperation()
        => Enum.TryParse<StatModifierOperation>(stringOperation, out var result) ? result : default;
}

[Serializable]
public class WagonData : BaseData
{
    public string Name;
    public int BaseHp;
    public int BaseCapacity;
    public float BaseMoveSpeed;
    public string SlowDataId;
}

[Serializable]
public class WagonSlowRuleData : BaseData
{
    public List<int> MinEnemyCount;
    public List<int> MaxEnemyCount;
    public List<float> MoveSpeedRate;
}