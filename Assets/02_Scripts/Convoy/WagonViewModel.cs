using UnityEngine;

public class WagonViewModel : BaseViewModel<WagonModel>
{
    public WagonViewModel(WagonModel model) : base(model)
    {
    }


    public float GetDurability => _model.Durability;
    public float GetMoveSpeed => _model.MoveSpeed;
    public int GetEnemyCount => _model.EnemyCount;
    public float GetWarningTime => _model.WarningTime;

    public void ReduceDurability(float amount)
    {
        _model.Durability -= amount;
    }

    public void SetMoveSpeed(float speed)
    {
        _model.MoveSpeed = speed;
    }

    public void ReduceEnemyCount()
    {
        _model.EnemyCount--;
    }

    public void IncreaseEnemyCount()
    {
        _model.EnemyCount++;
    }

    public void SetWarningTime(float time)
    {
        _model.WarningTime = time;
    }
}
