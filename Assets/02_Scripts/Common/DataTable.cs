using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataTable
{
    #region Variables

    public Dictionary<string, PreLoadAssetData> PreLoadAssetDataTable { get; private set; } = new();
    public Dictionary<string, PoolData> PoolDataTable { get; private set; } = new();
    public Dictionary<string, TownData> TownDataTable { get; private set; } = new();
    public Dictionary<string, RegionData> RegionDataTable { get; private set; } = new();
    public Dictionary<string, ReputationGradeData> ReputationGradeDataTable { get; private set; } = new();
    public Dictionary<string, ItemData> ItemDataTable { get; private set; } = new();
    public Dictionary<string, CharacterData> CharacterDataTable { get; private set; } = new();
    public Dictionary<string, PlayerStatData> PlayerStatDataTable { get; private set; } = new();
    public Dictionary<string, PetStatData> PetStatDataTable { get; private set; } = new();
    public Dictionary<string, QuestData> QuestDataTable { get; private set; } = new();
    public Dictionary<string, StageData> StageDataTable { get; private set; } = new();
    public Dictionary<string, AutoSpawnData> AutoSpawnDataTable { get; private set; } = new();
    public Dictionary<string, EnemyData> EnemyDataTable { get; private set; } = new();
    public Dictionary<string, PlayerSkillData> PlayerSkillDataTable { get; private set; } = new();
    public Dictionary<string, PetData> PetDataTable { get; private set; } = new();
    public Dictionary<string, LevelUpOptionData> LevelUpOptionDataTable { get; private set; } = new();

    public Dictionary<string, PetActiveSkillData> PetActiveSkillDataTable { get; private set; } = new();
    public Dictionary<string, StatusEffectData> StatusEffectDataTable { get; private set; } = new();
    public Dictionary<string, SkillModifierData> SkillModifierDataTable { get; private set; } = new();
    public Dictionary<string, PetPassiveSkillData> PetPassiveSkillDataTable { get; private set; } = new();

    public Dictionary<string, WagonData> WagonDataTable { get; private set; } = new();
    public Dictionary<string, WagonSlowRuleData> WagonSlowRuleDataTable { get; private set; } = new();
    #endregion

    [Serializable]
    class SerializationWrapper<T>
    {
        public List<T> items;
    }

    public void LoadAllData()
    {
        TownDataTable = LoadData<TownData>(nameof(TownData));
        RegionDataTable = LoadData<RegionData>(nameof(RegionData));
        //ReputationGradeDataTable = LoadData<ReputationGradeData>("ReputationGrade");
        //ItemDataTable = LoadData<ItemData>("Item");
        //CharacterDataTable = LoadData<CharacterData>("Character");
        PoolDataTable = LoadData<PoolData>(nameof(PoolData));
        PlayerStatDataTable = LoadData<PlayerStatData>("PlayerStatData");;
        QuestDataTable = LoadData<QuestData>(nameof(QuestData));
        StageDataTable = LoadData<StageData>(nameof(StageData));
        //EnemySpawnDataTable = LoadData<EnemySpawnData>("EnemySpawnData");
        EnemyDataTable = LoadData<EnemyData>(nameof(EnemyData));
        PetStatDataTable = LoadData<PetStatData>(nameof(PetStatData));
        PlayerSkillDataTable = LoadData<PlayerSkillData>(nameof(PlayerSkillData));
        //PetDataTable = LoadData<PetData>(nameof(PetData));
        LevelUpOptionDataTable = LoadData<LevelUpOptionData>(nameof(LevelUpOptionData));
        PetDataTable = LoadData<PetData>(nameof(PetData));
        PetActiveSkillDataTable = LoadData<PetActiveSkillData>(nameof(PetActiveSkillData));
        StatusEffectDataTable = LoadData<StatusEffectData>(nameof(StatusEffectData));
        //SkillModifierDataTable = LoadData<SkillModifierData>(nameof(SkillModifierData));
        PetPassiveSkillDataTable = LoadData<PetPassiveSkillData>(nameof(PetPassiveSkillData));
        WagonDataTable = LoadData<WagonData>(nameof(WagonData));
        WagonSlowRuleDataTable = LoadData<WagonSlowRuleData>(nameof(WagonSlowRuleData));
        AutoSpawnDataTable = LoadData<AutoSpawnData>(nameof(AutoSpawnData));
        PreLoadAssetDataTable = LoadData<PreLoadAssetData>(nameof(PreLoadAssetData));
    }

    #region Getters

    public TownData GetTownData(string townId)
    {
        if (null == TownDataTable || string.IsNullOrEmpty(townId)) return null;
        return TownDataTable.TryGetValue(townId, out var data) ? data : null;
    }

    public RegionData GetRegionData(string regionId)
    {
        if (null == RegionDataTable || string.IsNullOrEmpty(regionId)) return null;
        return RegionDataTable.TryGetValue(regionId, out var data) ? data : null;
    }

    public ItemData GetItemData(string itemId)
    {
        if (null == ItemDataTable || string.IsNullOrEmpty(itemId)) return null;
        return ItemDataTable.TryGetValue(itemId, out var data) ? data : null;
    }

    public CharacterData GetCharacterData(string characterId)
    {
        if (null == CharacterDataTable || string.IsNullOrEmpty(characterId)) return null;
        return CharacterDataTable.TryGetValue(characterId, out var data) ? data : null;
    }

    public QuestData GetQuestData(string questId)
    {
        if (null == QuestDataTable || string.IsNullOrEmpty(questId)) return null;
        return QuestDataTable.TryGetValue(questId, out var data) ? data : null;
    }

    public StageData GetStageData(string stageId)
    {
        if (null == StageDataTable || string.IsNullOrEmpty(stageId)) return null;
        return StageDataTable.TryGetValue(stageId, out var data) ? data : null;
    }

    public ReputationGradeData GetReputationGradeByValue(int reputation)
    {
        if (null == ReputationGradeDataTable) return null;

        foreach (var kv in ReputationGradeDataTable)
        {
            var grade = kv.Value;
            if (reputation >= grade.MinValue && reputation <= grade.MaxValue)
            {
                return grade;
            }
        }

        Debug.LogWarning($"평판 {reputation}에 해당하는 등급을 찾을 수 없습니다.");
        return null;
    }

    public PlayerStatData GetPlayerStatData(string id)
    {
        if (null == PlayerStatDataTable || string.IsNullOrEmpty(id)) return null;
        return PlayerStatDataTable.TryGetValue(id, out var data) ? data : null;
    }
    
    public AutoSpawnData GetAutoSpawnData(string id)
    {
        if (null == AutoSpawnDataTable || string.IsNullOrEmpty(id)) return null;
        return AutoSpawnDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public PetStatData GetPetStatData(string id)
    {
        if (null == PetStatDataTable || string.IsNullOrEmpty(id)) return null;
        return PetStatDataTable.TryGetValue(id, out var data) ? data : null;
    }
    
    public PlayerSkillData GetPlayerSkillData(string id)
    {
        if (null == PlayerSkillDataTable || string.IsNullOrEmpty(id)) return null;
        return PlayerSkillDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public PetData GetPetData(string id)
    {
        if (null == PetDataTable || string.IsNullOrEmpty(id)) return null;
        return PetDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public PetActiveSkillData GetPetActiveSkillData(string id)
    {
        if (null == PetActiveSkillDataTable || string.IsNullOrEmpty(id)) return null;
        return PetActiveSkillDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public StatusEffectData GetStatusEffectData(string id)
    {
        if (null == StatusEffectDataTable || string.IsNullOrEmpty(id)) return null;
        return StatusEffectDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public SkillModifierData GetSkillModifierData(string id)
    {
        if (null == SkillModifierDataTable || string.IsNullOrEmpty(id)) return null;
        return SkillModifierDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public PetPassiveSkillData GetPetPassiveSkillData(string id)
    {
        if (null == PetPassiveSkillDataTable || string.IsNullOrEmpty(id)) return null;
        return PetPassiveSkillDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public LevelUpOptionData GetLevelUpOptionData(string id)
    {
        if (null == LevelUpOptionDataTable || string.IsNullOrEmpty(id)) return null;
        return LevelUpOptionDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public WagonData GetWagonData(string id)
    {
        if (null == WagonDataTable || string.IsNullOrEmpty(id)) return null;
        return WagonDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public WagonSlowRuleData GetWagonSlowRuleData(string id)
    {
        if (null == WagonSlowRuleDataTable || string.IsNullOrEmpty(id)) return null;
        return WagonSlowRuleDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public PreLoadAssetData GetPreLoadAssetData(string id)
    {
        if (null == PreLoadAssetDataTable || string.IsNullOrEmpty(id)) return null;
        return PreLoadAssetDataTable.TryGetValue(id, out var data) ? data : null;
    }

    #endregion

    Dictionary<string, T> LoadData<T>(string tableNmae) where T : BaseData
    {
        string resourcePath = $"JsonOutput/{tableNmae}";
        TextAsset textAsset = Utils.ResourcesLoad<TextAsset>(resourcePath);
        if (null == textAsset)
        {
            Debug.LogError($"리소스를 찾을 수 없습니다: Resources/{resourcePath}");
            return new Dictionary<string, T>();
        }

        try
        {
            string jsonString = textAsset.text;

            string wrappedJson = "{\"items\":" + jsonString + "}";

            SerializationWrapper<T> wrapper = JsonUtility.FromJson<SerializationWrapper<T>>(wrappedJson);

            if (wrapper == null || wrapper.items == null)
            {
                Debug.LogError($"[{typeof(T).Name}] JSON 파싱 결과가 비어 있습니다.");
            }

            if (null != wrapper && null != wrapper.items)
            {
                Debug.Log($"{typeof(T).Name} 데이터를 {wrapper.items.Count}개 로드했습니다.");
                return wrapper.items.ToDictionary(value => value.Id.ToString());
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[{typeof(T).Name} JSON 로드 오류] {ex.Message}");
        }

        return new Dictionary<string, T>();
    }
}
