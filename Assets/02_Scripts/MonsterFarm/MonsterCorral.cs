using UnityEngine;
using System.Collections.Generic;

public class MonsterCorral : MonoBehaviour
{
    private readonly List<SOPetDefinition> _monsters = new();

    public IReadOnlyList<SOPetDefinition> Monsters => _monsters;

    public void AddMonster(SOPetDefinition monster)
    {
        _monsters.Add(monster);
        Debug.Log($"획득 몬스터: {monster.Name}");
    }

    public void RemoveLastMonsters(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int lastIndex = _monsters.Count - 1;
            _monsters.RemoveAt(lastIndex);
        }
    }
    public void PrintStoredMonsters()
    {
        if (_monsters.Count == 0)
        {
            Debug.Log("보관된 몬스터가 없습니다.");
            return;
        }

        for (int i = 0; i < _monsters.Count; i++)
        {
            Debug.Log($"보관 몬스터: {_monsters[i].Name}");
        }
    }
}
