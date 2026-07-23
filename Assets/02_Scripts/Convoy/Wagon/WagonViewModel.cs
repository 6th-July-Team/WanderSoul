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
    public string GetWagonName => _model.Name;
    public int GetWagonCapacity => _model.Capacity;
    public float GetProgress => _model.Progress;
    public float GetTime => _model.ConvoyTimer;

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

    public void SetWagonName(string name)
    {
        _model.Name = name;
    }

    public void SetWagonCapacity(int capacity)
    {
        _model.Capacity = capacity;
    }

    public void SetProgress(float progress)
    {
        _model.Progress = progress;
    }

    public void Updata(float deltaTime)
    {
        _model.ConvoyTimer += deltaTime;
    }
}
