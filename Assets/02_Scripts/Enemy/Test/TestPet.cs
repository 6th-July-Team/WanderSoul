using UnityEngine;

public class TestPet : MonoBehaviour, IPet
{
    public bool IsAlive => true;
    public EntityType EntityType => EntityType.Pet;
    public Vector3 Position => this.transform.position;

    private int Hp;

    private void Awake()
    {
        Hp = 1000;
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        Hp -= Mathf.RoundToInt(damageInfo.DamageAmount);

        Debug.Log($"남은 체력 : {Hp}");

        if (Hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
