using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class EnemyTargetSensor : MonoBehaviour
{
    private ISensorListener _listener;
    private SphereCollider _collider;

    private readonly List<ITargetable> _candidates = new();
    public IReadOnlyList<ITargetable> Candidates => _candidates;

    public void SetRange(float range)
    {
        _collider.radius = range;
    }

    private void Awake()
    {
        GetAndSetCollider();
        GetAndSetISensorListener();
    }

    private void GetAndSetCollider()
    {
        if (_collider == null)
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
        }
    }

    private void GetAndSetISensorListener()
    {
        if (_listener == null)
        {
            _listener = GetComponentInParent<ISensorListener>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ITargetable targetable = other.GetComponentInParent<ITargetable>();

        if (IsTargetCandidate(targetable) == false)
        {
            return;
        }

        if (_candidates.Contains(targetable))
        {
            return;
        }

        _candidates.Add(targetable);
        _listener?.OnSensorChanged();
    }

    private void OnTriggerExit(Collider other)
    {
        ITargetable targetable = other.GetComponentInParent<ITargetable>();

        if (targetable == null)
        {
            return;
        }

        if (_candidates.Remove(targetable))
        {
            _listener?.OnSensorChanged();
        }
    }

    private bool IsTargetCandidate(ITargetable targetable)
    {
        if (targetable == null)
        {
            return false;
        }

        if (targetable.EntityType == EntityType.Player)
        {
            return true;
        }

        if (targetable.EntityType == EntityType.Pet)
        {
            return true;
        }

        return false;
    }
}
