using System;

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
    public string QuestType;
    public int Difficulty;
    public int RequiredReputation;
    public string StageId;
    public int GoldReward;
    public int ReputationReward;
}

[Serializable]
public class StageData : BaseData
{
    public string Name;
    public string StagePrefabPath;
    public string WagonId;
    public string TerrainType;
    public string Description;
    public string StartTownId;
    public string ArrivalTownId;
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
public class SkillDefinition : BaseData
{
    public string Name;
    public string Description;
    public string SkillType;
    public float ManaCost;
    public float Cooldown;
    public string IconPath;
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