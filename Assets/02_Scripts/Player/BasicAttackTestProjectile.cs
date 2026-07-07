using UnityEngine;

public class BasicAttackTestProjectile : MonoBehaviour
{
    private float speed = 10f;
    private Vector3 direction;

    public void Init(Vector3 direction)
    {
        this.direction = direction.normalized;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
