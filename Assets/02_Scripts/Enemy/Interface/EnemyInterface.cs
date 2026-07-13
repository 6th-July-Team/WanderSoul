public interface IDamageable
{
    void TakeDamage(int damage);
}

public interface ISensorListener
{
    void OnSensorChanged();
}

public interface IEnemyView
{
    void AttackTarget();
}
