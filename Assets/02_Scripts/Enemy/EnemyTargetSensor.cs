using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class EnemyTargetSensor : MonoBehaviour
{
    private ISensorListener _listener;
    private SphereCollider _collider;

    private readonly List<GameObject> _candidates = new();
    public IReadOnlyList<GameObject> Candidates => _candidates;

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
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
    }

    private void GetAndSetISensorListener()
    {
        _listener = GetComponentInParent<ISensorListener>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsTargetCandidate(other) == false)
        {
            return;
        }

        if (_candidates.Contains(other.gameObject))
        {
            return;
        }

        _candidates.Add(other.gameObject);
        _listener?.OnSensorChanged();
    }

    private void OnTriggerExit(Collider other)
    {
        if(_candidates.Remove(other.gameObject))
        {
            _listener?.OnSensorChanged();
        }
    }

    private bool IsTargetCandidate(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            return true;
        }

        if (other.CompareTag("Pet"))
        {
            return true;
        }

        return false;
    }
}
