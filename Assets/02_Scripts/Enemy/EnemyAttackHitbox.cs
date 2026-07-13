using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class EnemyAttackHitbox : MonoBehaviour
{
    private BoxCollider _collider;

    private readonly List<GameObject> _targetList = new();
    public IReadOnlyList<GameObject> TargetList => _targetList;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }

    public void SetRange(float range)
    {
        Vector3 size = _collider.size;
        size.z = range;
        _collider.size = size;

        Vector3 center = _collider.center;
        center.z = range * 0.5f;
        _collider.center = center;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_targetList.Contains(other.gameObject))
        {
            return;
        }

        if(other.CompareTag("Player") || other.CompareTag("Pet") || other.CompareTag("Wagon"))
        {
            _targetList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _targetList.Remove(other.gameObject);
    }
}
