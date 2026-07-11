using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTargetSelector
{
    private readonly TargetPolicy _policy;
    private readonly GameObject _caravan;
    private readonly GameObject _player;

    private readonly float _leashRange;

    private GameObject _currentTarget;

    // 생성자의 _leashRange, leashRange => 마차가 중심인 어그로 범위 // IReadOnlyList<GameObject> => 몬스터의 콜라이더가 탐지한 오브젝트 ( 일단은 몬스터, 펫만 넣어둠! )
    public EnemyTargetSelector(TargetPolicy policy, GameObject caravan, GameObject player, float leashRange)
    {
        _policy = policy;
        _caravan = caravan;
        _player = player;
        _leashRange = leashRange;
    }

    public GameObject SelectTarget(IReadOnlyList<GameObject> candidates)
    {
        _currentTarget = FindTarget(candidates);
        return _currentTarget;
    }

    private GameObject FindTarget(IReadOnlyList<GameObject> candidates)
    {
        if (_policy == TargetPolicy.PlayerOnly)
        {
            if (_player != null)
            {
                return _player;
            }
        }

        if (_policy == TargetPolicy.CaravanOnly)
        {
            if (_caravan != null)
            {
                return _caravan;
            }
        }

        if (IsPlayerTargetable(candidates))
        {
            return _player;
        }

        GameObject pet = FindTargetablePet(candidates);

        if (pet != null)
        {
            return pet;
        }

        return _caravan;
    }

    private bool IsPlayerTargetable(IReadOnlyList<GameObject> candidates)
    {
        if (IsInsideLeash(_player) == false)
        {
            return false;
        }

        bool isPlayerTargetable = (_currentTarget == _player || candidates.Contains(_player));

        return isPlayerTargetable;
    }

    private GameObject FindTargetablePet(IReadOnlyList<GameObject> candidates)
    {
        if (IsPet(_currentTarget) && IsInsideLeash(_currentTarget))
        {
            return _currentTarget;
        }

        foreach (GameObject candidate in candidates)
        {
            if (IsPet(candidate) && IsInsideLeash(candidate))
            {
                return candidate;
            }
        }

        return null;
    }

    private bool IsInsideLeash(GameObject target)
    {
        if (IsAlive(target) == false)
        {
            return false;
        }

        float distanceFromCaravan = Vector3.Distance(target.transform.position, _caravan.transform.position);

        bool isInsideLeash = (distanceFromCaravan <= _leashRange);

        return isInsideLeash;

    }

    private bool IsAlive(GameObject target)
    {
        bool isAlive = (target != null && target.activeInHierarchy);

        return isAlive;
    }

    private bool IsPet(GameObject target)
    {
        bool isPet = (target != null && target != _player && target.CompareTag("Pet"));

        return isPet;
    }
}