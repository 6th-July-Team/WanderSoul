using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyProjectileObject : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public event Action<IDamageable, Vector3> OnDamageableTargetHited;

    private void Awake()
    {
        GetAndSetRigidBody();
    }

    private void GetAndSetRigidBody()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
    }

    public void Launch(GameObject target, float speed, float lifeTime)
    {
        Vector3 direction = (target.transform.position + Vector3.up - transform.position).normalized;
        direction.y = 0f;

        _rigidbody.linearVelocity = direction * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageableTarget = other.GetComponentInParent<IDamageable>();

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
