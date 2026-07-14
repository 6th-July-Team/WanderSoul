using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float BasicAttackPower;
    public float Defense;
    public float FireResistance;
    public float ColdResistance;
    public float ElectricResistance;
    public float ElementalResistance;
    public float MoveSpeed;
    public float HealthRegeneration;
    public float ManaRegeneration;
    public float CooldownReduction;
    public float MaxMana;

    public float GetBaseValue(PlayerStatType statType)
    {
        return statType switch
        {
            PlayerStatType.MaxHealth => MaxHealth,
            PlayerStatType.BasicAttackPower => BasicAttackPower,
            PlayerStatType.Defense => Defense,
            PlayerStatType.FireResistance => FireResistance,
            PlayerStatType.ColdResistance => ColdResistance,
            PlayerStatType.ElectricResistance => ElectricResistance,
            PlayerStatType.ElementalResistance => ElementalResistance,
            PlayerStatType.MoveSpeed => MoveSpeed,
            PlayerStatType.HealthRegeneration => HealthRegeneration,
            PlayerStatType.ManaRegeneration => ManaRegeneration,
            PlayerStatType.CooldownReduction => CooldownReduction,
            PlayerStatType.MaxMana => MaxMana,
            _ => 0f
        };
    }
}

[Serializable]
public class EnemySpawnData : BaseData
{
    public float SpawnInterval;
    public List<string> EnemyIds;
}

[Serializable]
public class EnemyData : BaseData, ISerializationCallbackReceiver
{
    public string TargetPolicy; // PriortyBased == 기본 타입 // WagonOnly == 마차만 공격 // PlayerOnly == 플레이어만 공격
    public string EnemyAttackType; // 공격 타입 // Melee == 근접 // Projectile == 투사체발사(원거리) // AreaDelayed == 고정 포대형(예고 후 장판 공격) // Steal == 공격 없이 훔치기 기능만

    [NonSerialized] public TargetPolicy Policy;
    [NonSerialized] public EnemyAttackType AttackType;

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
    }

    public void OnBeforeSerialize() { }

    public string Name;
    public string Description;

    public int MaxHp;

    public float DetectRange; // 몬스터를 기준으로 탐색하는 범위
    public float LeashRange; // 마차를 기준으로 탐색하는 범위

    public int Attack;
    public float AttackSpeed;
    public float AttackRange;

    public float SoulDropChance;
    public int SoulDropAmount;

    public float ExpDropChance;
    public int ExpDropAmount;

    public float PreferredDistance; // [저격형 전용, 다른 타입의 경우 0으로 할 것] 저격형 몬스터가 도망치는 최소 범위

    public bool CanMove; // 고정형인지 아닌지
    public float MoveSpeed; // 이동속도
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

    public float GetBaseValue(PetStatType statType)
    {
        return statType switch
        {
            PetStatType.MaxHealth => MaxHealth,
            PetStatType.BasicPower => BasicPower,
            PetStatType.Defense => Defense,
            PetStatType.FireResistance => FireResistance,
            PetStatType.ColdResistance => ColdResistance,
            PetStatType.ElectricResistance => ElectricResistance,
            PetStatType.MagicResistance => MagicResistance,
            PetStatType.MoveSpeed => MoveSpeed,
            PetStatType.HealthRegeneration => HealthRegeneration,
            PetStatType.CooldownReduction => CooldownReduction,
            PetStatType.CritChance => CritChance,
            PetStatType.CritMultiplier => CritMultiplier,
            PetStatType.LifeSteal => LifeSteal,
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
    public float ProjectileSpeed;
    public int Pierce;
    public int MaxProjectileCount;
    public string SkillType;
    public string SkillElementType;
    public string SkillOwnerType;
    public string VFXPath;
    public string SFXPath;
    public string IconPath;
}