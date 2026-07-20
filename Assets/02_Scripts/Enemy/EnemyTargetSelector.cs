using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetSelector
{
    private readonly TargetPolicy _policy;
    private readonly ITargetable _wagon;
    private readonly ITargetable _player;

    private readonly float _leashRange;

    public ITargetable CurrentTarget { get; private set; }

    private readonly Dictionary<ITargetable, float> _excludedTargets = new();

    private IPet _forcedTarget;
    private float _forcedTargetEndTime;

    public EnemyTargetSelector(TargetPolicy policy, ITargetable wagon, ITargetable player, float leashRange)
    {
        _policy = policy;
        _wagon = wagon;
        _player = player;
        _leashRange = leashRange;
    }

    public ITargetable SelectTarget(IReadOnlyList<ITargetable> candidates)
    {
        CurrentTarget = FindTarget(candidates);
        return CurrentTarget;
    }

    private ITargetable FindTarget(IReadOnlyList<ITargetable> candidates)
    {
        if (IsForcedTargetValid())
        {
            return _forcedTarget;
        }

        if (_policy == TargetPolicy.PlayerOnly)
        {
            if (IsAlive(_player))
            {
                return _player;
            }

            return null;
        }

        if (_policy == TargetPolicy.WagonOnly)
        {
            if (IsAlive(_wagon))
            {
                return _wagon;
            }

            return null;
        }

        if (IsPlayerTargetable(candidates))
        {
            return _player;
        }

        ITargetable pet = FindTargetablePet(candidates);

        if (pet != null)
        {
            return pet;
        }

        if (IsAlive(_wagon) == false)
        {
            return null;
        }

        return _wagon;
    }

    private bool IsPlayerTargetable(IReadOnlyList<ITargetable> candidates)
    {
        if (IsExcluded(_player))
        {
            return false;
        }

        if (IsInsideLeash(_player) == false)
        {
            return false;
        }

        bool isPlayerTargetable = (CurrentTarget == _player || Contains(candidates, _player));

        return isPlayerTargetable;
    }

    private ITargetable FindTargetablePet(IReadOnlyList<ITargetable> candidates)
    {
        if (IsPet(CurrentTarget) && IsInsideLeash(CurrentTarget))
        {
            if (IsExcluded(CurrentTarget) == false)
            {
                return CurrentTarget;
            }
        }

        foreach (ITargetable candidate in candidates)
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

    private bool IsInsideLeash(ITargetable target)
    {
        if (IsAlive(target) == false)
        {
            return false;
        }

        if (IsAlive(_wagon) == false)
        {
            return false;
        }

        float distanceFromWagon = Vector3.Distance(target.Position, _wagon.Position);

        bool isInsideLeash = (distanceFromWagon <= _leashRange);

        return isInsideLeash;
    }

    // 인터페이스 참조는 Unity의 "파괴된 오브젝트 == null" 처리를 타지 않으므로
    // Component로 캐스팅한 뒤 파괴/비활성 여부를 검사해야 함
    private bool IsAlive(ITargetable target)
    {
        if (target == null)
        {
            return false;
        }

        Component targetComponent = target as Component;

        if (targetComponent == null || targetComponent.gameObject.activeInHierarchy == false)
        {
            return false;
        }

        return target.IsAlive;
    }

    private bool IsPet(ITargetable target)
    {
        bool isPet = (target != null && target.EntityType == EntityType.Pet);

        return isPet;
    }

    private bool Contains(IReadOnlyList<ITargetable> candidates, ITargetable target)
    {
        for (int i = 0; i < candidates.Count; i++)
        {
            if (candidates[i] == target)
            {
                return true;
            }
        }

        return false;
    }

    public ITargetable ExcludeCurrentAndReselect(IReadOnlyList<ITargetable> candidates, float excludeDuration)
    {
        if (CurrentTarget != null)
        {
            _excludedTargets[CurrentTarget] = Time.time + excludeDuration;
        }

        return SelectTarget(candidates);
    }

    private bool IsExcluded(ITargetable target)
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

    public void SetForcedTarget(IPet target, float duration)
    {
        _forcedTarget = target;
        _forcedTargetEndTime = Time.time + duration;
    }

    public void ClearForcedTarget()
    {
        _forcedTarget = null;
    }

    private bool IsForcedTargetValid()
    {
        if (_forcedTarget == null || Time.time >= _forcedTargetEndTime)
        {
            return false;
        }

        bool forcedTargetIsAlive = IsAlive(_forcedTarget);

        return forcedTargetIsAlive;
    }
}
