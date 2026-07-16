using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTargetSelector
{
    private readonly TargetPolicy _policy;
    private readonly GameObject _wagon;
    private readonly GameObject _player;

    private readonly float _leashRange;

    public GameObject CurrentTarget { get; private set; }

    private readonly Dictionary<GameObject, float> _excludedTargets = new();

    public EnemyTargetSelector(TargetPolicy policy, GameObject wagon, GameObject player, float leashRange)
    {
        _policy = policy;
        _wagon = wagon;
        _player = player;
        _leashRange = leashRange;
    }

    public GameObject SelectTarget(IReadOnlyList<GameObject> candidates)
    {
        CurrentTarget = FindTarget(candidates);
        return CurrentTarget;
    }

    private GameObject FindTarget(IReadOnlyList<GameObject> candidates)
    {
        if (_policy == TargetPolicy.PlayerOnly)
        {
            if(IsAlive(_player))
            {
                return _player;
            }

            return null;
        }

        if (_policy == TargetPolicy.WagonOnly)
        {
            if (_wagon != null)
            {
                return _wagon;
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

        if(_wagon == null)
        {
            return null;
        }

        return _wagon;
    }

    private bool IsPlayerTargetable(IReadOnlyList<GameObject> candidates)
    {
        if(IsExcluded(_player))
        {
            return false;
        }

        if (IsInsideLeash(_player) == false)
        {
            return false;
        }

        bool isPlayerTargetable = (CurrentTarget == _player || candidates.Contains(_player));

        return isPlayerTargetable;
    }

    private GameObject FindTargetablePet(IReadOnlyList<GameObject> candidates)
    {

        if (IsPet(CurrentTarget) && IsInsideLeash(CurrentTarget))
        {
            if(IsExcluded(CurrentTarget) == false)
            {
                return CurrentTarget;
            }
        }

        foreach (GameObject candidate in candidates)
        {
            if (IsPet(candidate) && IsInsideLeash(candidate))
            {
                if (IsExcluded(candidate) == false)
                {
                    return candidate;
                }
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

        if(_wagon == null)
        {
            return false;
        }

        float distanceFromwagon = Vector3.Distance(target.transform.position, _wagon.transform.position);

        bool isInsideLeash = (distanceFromwagon <= _leashRange);

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

    public GameObject ExcludeCurrentAndReselect(IReadOnlyList<GameObject> candidates, float ExcludeDuration)
    {
        if(CurrentTarget != null)
        {
            _excludedTargets[CurrentTarget] = Time.time + ExcludeDuration;
        }

        return SelectTarget(candidates);
    }

    private bool IsExcluded(GameObject target)
    {
        if (target == null)
        {
            return false;
        }

        if (_excludedTargets.TryGetValue(target, out float excludeUntil) == false)
        {
            return false;
        }

        if (Time.time < excludeUntil)
        {
            return true;
        }

        _excludedTargets.Remove(target);
        return false;
    }
}