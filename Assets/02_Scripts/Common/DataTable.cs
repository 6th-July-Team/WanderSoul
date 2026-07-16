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
    public Dictionary<string, EnemySpawnData> EnemySpawnDataTable { get; private set; } = new();
    public Dictionary<string, EnemyData> EnemyDataTable { get; private set; } = new();
    public Dictionary<string, PetStatData> PetStatDataTable { get; private set; } = new();
    public Dictionary<string, PlayerSkillData> PlayerSkillDataTable { get; private set; } = new();

    #endregion

    [Serializable]
    class SerializationWrapper<T>
    {
        public List<T> items;
    }

    public void LoadAllData()
    {
        //TownDataTable = LoadData<TownData>("Town");
        //RegionDataTable = LoadData<RegionData>("Region");
        //ReputationGradeDataTable = LoadData<ReputationGradeData>("ReputationGrade");
        //ItemDataTable = LoadData<ItemData>("Item");
        //CharacterDataTable = LoadData<CharacterData>("Character");
        //PoolDataTable = LoadData<PoolData>(nameof(PoolData));
        PlayerStatDataTable = LoadData<PlayerStatData>("PlayerStatData");
        //EnemySpawnDataTable = LoadData<EnemySpawnData>("EnemySpawnData");
        EnemyDataTable = LoadData<EnemyData>(nameof(EnemyData));
        PetStatDataTable = LoadData<PetStatData>(nameof(PetStatData));
        PlayerSkillDataTable = LoadData<PlayerSkillData>(nameof(PlayerSkillData));
    }

    #region Getters

    public TownData GetTownData(string townId)
    {
        if (string.IsNullOrEmpty(townId))
        {
            return null;
        }
        if (TownDataTable.ContainsKey(townId) == false)
        {
            Debug.LogWarning($"마을 데이터를 찾을 수 없습니다: {townId}");
            return null;
        }
        return TownDataTable[townId];
    }

    public RegionData GetRegionData(string regionId)
    {
        if (string.IsNullOrEmpty(regionId))
        {
            return null;
        }
        if (RegionDataTable.ContainsKey(regionId) == false)
        {
            Debug.LogWarning($"지역 데이터를 찾을 수 없습니다: {regionId}");
            return null;
        }
        return RegionDataTable[regionId];
    }

    public ItemData GetItemData(string itemId)
    {
        if (string.IsNullOrEmpty(itemId))
        {
            return null;
        }
        if (ItemDataTable.ContainsKey(itemId) == false)
        {
            Debug.LogWarning($"아이템 데이터를 찾을 수 없습니다: {itemId}");
            return null;
        }
        return ItemDataTable[itemId];
    }

    public CharacterData GetCharacterData(string characterId)
    {
        if (string.IsNullOrEmpty(characterId))
        {
            return null;
        }
        if (CharacterDataTable.ContainsKey(characterId) == false)
        {
            Debug.LogWarning($"캐릭터 데이터를 찾을 수 없습니다: {characterId}");
            return null;
        }
        return CharacterDataTable[characterId];
    }

    public ReputationGradeData GetReputationGradeByValue(int reputation)
    {
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
    
    public PlayerSkillData GetPlayerSkillData(string id)
    {
        if (null == PlayerSkillDataTable || string.IsNullOrEmpty(id)) return null;
        return PlayerSkillDataTable.TryGetValue(id, out var data) ? data : null;
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
