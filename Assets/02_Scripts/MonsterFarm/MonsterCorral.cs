using UnityEngine;
using System.Collections.Generic;

public class MonsterCorral : MonoBehaviour
{
    private readonly List<string> _monsterNames = new();

    public IReadOnlyList<string> MonsterNames => _monsterNames;

    public void AddMonster(string monsterName)
    {
        _monsterNames.Add(monsterName);
        Debug.Log($"획득 몬스터: {monsterName}");
    }

    public void RemoveLastMonsters(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int lastIndex = _monsterNames.Count - 1;
            _monsterNames.RemoveAt(lastIndex);
        }
    }
    public void PrintStoredMonsters()
    {
        if (_monsterNames.Count == 0)
        {
            Debug.Log("보관된 몬스터가 없습니다.");
            return;
        }

        Debug.Log($"사육장 보관 몬스터: {string.Join(", ", _monsterNames)}");
    }
}
