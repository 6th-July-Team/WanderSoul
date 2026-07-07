using UnityEngine;

[RequireComponent(typeof(PlayerInputHandle))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 15f;
    private float _verticalVelocity;

    [Header("Gravity")]
    [SerializeField] private float _gravity = -30f;
    [SerializeField] private float _groundedGravity = -2f;

    private PlayerInputHandle _inputHandle;
    private CharacterController _characterController;
    private PlayerAimHandler _aimHandler;
    private InputBinder _inputBinder;


    // TEST: 일반 공격 방향 체크
    private void OnBasicAttack()
    {
        var direction =_aimHandler.AimDirection;

        BasicAttackTestProjectile a = Instantiate(Utils.ResourcesLoad<BasicAttackTestProjectile>("BasicAttackTestProjectile"), transform.position, Quaternion.LookRotation(direction));
        a.Init(direction);
    }

    private void Awake()
    {
        _inputHandle = GetComponent<PlayerInputHandle>();
        _characterController = GetComponent<CharacterController>();
        _aimHandler = GetComponent<PlayerAimHandler>();

        _inputBinder = new InputBinder(_inputHandle);
    }

    // TODO(김익환): 바인더 클래스를 만드는 것을 고려
    private void OnEnable()
    {
        _inputHandle.OnLeftClickEvent += OnBasicAttack;

        _inputBinder.Bind();
    }

    private void OnDisable()
    {
        _inputHandle.OnLeftClickEvent -= OnBasicAttack;

        _inputBinder.UnBind();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveDirection = new Vector3(_inputHandle.MoveInput.x, 0f, _inputHandle.MoveInput.y);

        if (moveDirection.sqrMagnitude < 0.01f)
        {
            ApplyGravity();

            Vector3 gravityVelocity = Vector3.up * _verticalVelocity;
            _characterController.Move(gravityVelocity * Time.deltaTime);
            return;
        }

        if (moveDirection.sqrMagnitude > 1f)
            moveDirection.Normalize();

        ApplyGravity();

        Vector3 velocity = moveDirection * _moveSpeed;
        velocity.y = _verticalVelocity;

        _characterController.Move(velocity * Time.deltaTime);

        RotateToMoveDirection(moveDirection);
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = _groundedGravity;
        }
        else
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }
    }

    private void RotateToMoveDirection(Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude <= 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}
