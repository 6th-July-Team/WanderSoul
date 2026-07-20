using UnityEngine;

public enum DropObjectType
{
    Exp,
    Soul
}

public enum DropObjectDigit
{
    One,
    Ten,
    Hundred,
}

[RequireComponent(typeof(Rigidbody))]
public class EnemyDropObject : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 10f;

    public DropObjectType Type { get; private set; }
    public DropObjectDigit Digit { get; private set; }
    public int Value {  get; private set; }

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

    public void Init(DropObjectType type, DropObjectDigit digit)
    {
        Type = type;
        Digit = digit;
        Value = GetDigitValue();
        _targetPlayer = null;
        enabled = false;
    }

    private int GetDigitValue()
    {
        switch (Digit)
        {
            case DropObjectDigit.One:
                {
                    return 1;
                }
            case DropObjectDigit.Ten:
                {
                    return 10;
                }
            case DropObjectDigit.Hundred:
                {
                    return 100;
                }
            default:
                {
                    Debug.LogError("[ EnemyDropObject - GetDigitValue ] : 알 수 없는 값이 들어왔습니다!!");
                    return 0;
                }
        }
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
