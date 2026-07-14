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
    public Dictionary<string, PetData> PetDataTable { get; private set; } = new();
    public Dictionary<string, QuestData> QuestDataTable { get; private set; } = new();
    public Dictionary<string, StageData> StageDataTable { get; private set; } = new();
    public Dictionary<string, EnemySpawnData> EnemySpawnDataTable { get; private set; } = new();
    public Dictionary<string, EnemyData> EnemyDataTable { get; private set; } = new();
    public Dictionary<string, PetStatData> PetStatDataTable { get; private set; } = new();

    #endregion

    [Serializable]
    class SerializationWrapper<T>
    {
        public List<T> items;
    }

    public void LoadAllData()
    {
        TownDataTable = LoadData<TownData>("TownData");
        RegionDataTable = LoadData<RegionData>("RegionData");
        ReputationGradeDataTable = LoadData<ReputationGradeData>("ReputationGradeData");
        ItemDataTable = LoadData<ItemData>("ItemData");
        CharacterDataTable = LoadData<CharacterData>("CharacterData");
        PoolDataTable = LoadData<PoolData>("Pool");
        PlayerStatDataTable = LoadData<PlayerStatData>("PlayerStatData");
        PetDataTable = LoadData<PetData>("PetData");
        QuestDataTable = LoadData<QuestData>("QuestData");
        StageDataTable = LoadData<StageData>("StageData");
        //EnemySpawnDataTable = LoadData<EnemySpawnData>("EnemySpawnData");
        EnemyDataTable = LoadData<EnemyData>(nameof(EnemyData));
        PetStatDataTable = LoadData<PetStatData>(nameof(PetStatData));
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

    public PetData GetPetData(string id)
    {
        if (null == PetDataTable || string.IsNullOrEmpty(id)) return null;
        return PetDataTable.TryGetValue(id, out var data) ? data : null;
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
    
    public EnemySpawnData GetEnemySpawnData(string id)
    {
        if (null == EnemySpawnDataTable || string.IsNullOrEmpty(id)) return null;
        return EnemySpawnDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public PetStatData GetPetStatData(string id)
    {
        if (null == PetStatDataTable || string.IsNullOrEmpty(id)) return null;
        return PetStatDataTable.TryGetValue(id, out var data) ? data : null;
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
