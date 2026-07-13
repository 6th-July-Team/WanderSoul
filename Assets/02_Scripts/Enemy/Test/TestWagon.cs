using UnityEngine;

public class TestWagon : MonoBehaviour, IDamageable
{
    private int Hp;

    private void Awake()
    {
        Hp = 1000;
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;

        Debug.Log($"남은 체력 : {Hp}");

        if(Hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
