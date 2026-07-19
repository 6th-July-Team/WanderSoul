using UnityEngine;

public enum DropObjectType
{
    Exp,
    Soul
}

[RequireComponent(typeof(Rigidbody))]
public class EnemyDropObject : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 10f;

    public DropObjectType ObjectType { get; private set; }
    public int Amount {  get; private set; }

    private Rigidbody _rigidbody;

    private IPlayer _targetPlayer;

    private void Awake()
    {
        InitRigidBody();
    }

    private void InitRigidBody()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
    }

    public void Init(DropObjectType type, int amount)
    {
        ObjectType = type;
        Amount = amount;
        _targetPlayer = null;
        enabled = false;
    }

    public void StartFollowPlayer(IPlayer player)
    {
        if(player.IsAlive == false)
        {
            return;
        }

        _targetPlayer = player;
        enabled = true;
    }

    private void FixedUpdate()
    {
        if (_targetPlayer != null && _targetPlayer.IsAlive == true)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = (_targetPlayer.Position - transform.position).normalized;
        _rigidbody.MovePosition(transform.position + direction * _followSpeed * Time.fixedDeltaTime);
    }

    public void OnCollected()
    {
        _targetPlayer = null;
        enabled = false;
        GameManager.Pool.DespawnToPool(this.gameObject);
    }
}
