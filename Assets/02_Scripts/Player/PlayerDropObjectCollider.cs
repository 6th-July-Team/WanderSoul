using UnityEngine;

public class PlayerDropObjectCollider : MonoBehaviour , IPositionProvider
{
    public Vector3 Position => this.transform.position;
    public Transform Transform => this.transform;

    public void TakeDamage(DamageInfo damageInfo)
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyDropObject>(out var enemyDropObject))
        {
            enemyDropObject.StartFollowPlayer(this);
        }
    }
}
