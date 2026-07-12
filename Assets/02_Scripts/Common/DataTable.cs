using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataTable
{
    #region Variables

    public Dictionary<string, PreLoadAssetData> PreLoadAssetDataTable { get; private set; } = new();
    public Dictionary<string, PoolData> PoolDataTable { get; private set; } = new();

    #endregion


    [Serializable]
    class SerializationWrapper<T>
    {
        public List<T> items;
    }
    private Dictionary<string, TownData> _townDataTable = new Dictionary<string, TownData>();
    private Dictionary<string, RegionData> _regionDataTable = new Dictionary<string, RegionData>();
    private Dictionary<string, ReputationGradeData> _reputationGradeDataTable = new Dictionary<string, ReputationGradeData>();
    private Dictionary<string, ItemData> _itemDataTable = new Dictionary<string, ItemData>();
    private Dictionary<string, CharacterData> _characterDataTable = new Dictionary<string, CharacterData>();


    public void LoadAllData()
    {
        _townDataTable = LoadData<TownData>("Town");
        _regionDataTable = LoadData<RegionData>("Region");
        _reputationGradeDataTable = LoadData<ReputationGradeData>("ReputationGrade");
        _itemDataTable = LoadData<ItemData>("Item");
        _characterDataTable = LoadData<CharacterData>("Character");
        PoolDataTable = LoadData<PoolData>("Pool");

    }

    #region Getters




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

    public TownData GetTownData(string townId)
    {
        if (string.IsNullOrEmpty(townId))
        {
            return null;
        }
        if (_townDataTable.ContainsKey(townId) == false)
        {
            Debug.LogWarning($"마을 데이터를 찾을 수 없습니다: {townId}");
            return null;
        }
        return _townDataTable[townId];
    }

    public RegionData GetRegionData(string regionId)
    {
        if (string.IsNullOrEmpty(regionId))
        {
            return null;
        }
        if (_regionDataTable.ContainsKey(regionId) == false)
        {
            Debug.LogWarning($"지역 데이터를 찾을 수 없습니다: {regionId}");
            return null;
        }
        return _regionDataTable[regionId];
    }

    public ItemData GetItemData(string itemId)
    {
        if (string.IsNullOrEmpty(itemId))
        {
            return null;
        }
        if (_itemDataTable.ContainsKey(itemId) == false)
        {
            Debug.LogWarning($"아이템 데이터를 찾을 수 없습니다: {itemId}");
            return null;
        }
        return _itemDataTable[itemId];
    }

    public CharacterData GetCharacterData(string characterId)
    {
        if (string.IsNullOrEmpty(characterId))
        {
            return null;
        }
        if (_characterDataTable.ContainsKey(characterId) == false)
        {
            Debug.LogWarning($"캐릭터 데이터를 찾을 수 없습니다: {characterId}");
            return null;
        }
        return _characterDataTable[characterId];
    }

    public ReputationGradeData GetReputationGradeByValue(int reputation)
    {
        foreach (var kv in _reputationGradeDataTable)
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
}
