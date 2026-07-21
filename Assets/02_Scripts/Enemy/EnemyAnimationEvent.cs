using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    [SerializeField] private EnemyView View_Self;

    public void ExecuteAttack()
    {
        View_Self.ExecuteAttack();
    }

    public void ExecuteDead()
    {
        View_Self.ExecuteDead();
    }
}