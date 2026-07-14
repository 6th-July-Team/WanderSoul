using UnityEngine;

public class EnemyAnimationAttackEvent : MonoBehaviour
{
    [SerializeField] private MoveableEnemyView View_Self;
    public void OnAttackHit()
    {
        View_Self.OnAttackHit();
    }
}