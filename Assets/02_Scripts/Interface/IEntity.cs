
public interface IPlayer : IDamageable { }

public interface IEnemy : IDamageable
{
    public void ApplyTaunt(IPet taunter, float duration);
}

public interface IPet : IDamageable { }

public interface IWagon : IDamageable { }