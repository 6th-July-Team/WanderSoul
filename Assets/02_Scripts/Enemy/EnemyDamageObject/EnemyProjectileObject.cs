using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class EnemyProjectileObject : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider _collider;

    public event Action<IDamageable, Vector3> OnDamageableTargetHited;

    private void Awake()
    {
        GetAndSetRigidBody();
        GetAndSetCollider();
    }

    private void GetAndSetRigidBody()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
    }

    private void GetAndSetCollider()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    public void Launch(GameObject target, float speed, float lifeTime)
    {
        Vector3 direction = (target.transform.position - transform.position);
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
        {
            direction = transform.forward;
        }

        direction.Normalize();

        this.transform.rotation = Quaternion.LookRotation(direction);
        _rigidbody.linearVelocity = direction * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageableTarget = other.GetComponent<IDamageable>();

        if (damageableTarget == null || damageableTarget.IsAlive == false)
        {
            return;
        }

        if (damageableTarget.EntityType == EntityType.Enemy)
        {
            return;
        }

        Vector3 hitDirection = (damageableTarget.Position - this.transform.position).normalized;

        OnDamageableTargetHited?.Invoke(damageableTarget, hitDirection);

        Destroy(gameObject);
    }
}
