using UnityEngine;

[RequireComponent(typeof(PlayerInputHandle))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 15f;
    private float _verticalVelocity;

    [Header("Gravity")]
    [SerializeField] private float _gravity = -30f;
    [SerializeField] private float _groundedGravity = -2f;

    [Header("Dash")]
    [SerializeField] private float _dashSpeed = 15f;
    [SerializeField] private float _dashDuration = 0.2f;
    [SerializeField] private float _dashCoolTime = 1f;
    private float _dashTimer;
    private float _dashChargeTimer;
    private int _dashMaxCount = 2;
    private int _dashCount;
    private bool _isDashing;
    private bool _isDashCharging;
    private Vector3 _dashDirection;

    // Components
    private PlayerInputHandle _inputHandle;
    private CharacterController _characterController;
    private PlayerAnimationController _animationController;

    private void Awake()
    {
        _inputHandle = GetComponent<PlayerInputHandle>();
        _characterController = GetComponent<CharacterController>();
        _animationController = GetComponent<PlayerAnimationController>();

        //_dashMaxCount = DataTable.GetPlayerData.DashMaxCount
        _dashCount = _dashMaxCount;
        _isDashing = false;
        _isDashCharging = false;
    }

    // TODO(김익환): 바인더 클래스를 만드는 것을 고려
    private void OnEnable()
    {
        _inputHandle.OnDashEvent += OnDash;
    }

    private void OnDisable()
    {
        _inputHandle.OnDashEvent -= OnDash;
    }

    private void Update()
    {
        if (GameManager.Time.IsPaused)
            return;

        UpdateDash();
        UpdateChargeDash();
        Move();
        UpdateMoveAnimation();
    }

    private void Move()
    {
        Vector3 moveDirection = _inputHandle.MoveInputV3;

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

        Vector3 velocity;

        if (_isDashing)
        {
            velocity = _dashDirection * _dashSpeed;
        }
        else
        {
            velocity = moveDirection * _moveSpeed;
        }

        velocity.y = _verticalVelocity;

        _characterController.Move(velocity * Time.deltaTime);

        if (_isDashing)
        {
            RotateToMoveDirection(_dashDirection);
        }
        else
        {
            RotateToMoveDirection(moveDirection);
        }
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

    private void OnDash()
    {
        if (_isDashing || _dashCount <= 0)
            return;

        _dashCount--;

        if (_inputHandle.MoveInputV3.sqrMagnitude > 0.001f)
        {
            _dashDirection = _inputHandle.MoveInputV3.normalized;
        }
        else
        {
            _dashDirection = transform.forward;
            _dashDirection.y = 0f;

            if (_dashDirection.sqrMagnitude <= 0.001f)
                _dashDirection = Vector3.forward;

            _dashDirection.Normalize();
        }

        _isDashing = true;
        _dashTimer = _dashDuration;

        if (!_isDashCharging)
        {
            _isDashCharging = true;
            _dashChargeTimer = _dashCoolTime;
        }
    }

    private void UpdateDash()
    {
        if (!_isDashing)
            return;

        _dashTimer -= Time.deltaTime;

        if (_dashTimer <= 0f)
        {
            _isDashing = false;
        }
    }

    private void UpdateChargeDash()
    {
        if (!_isDashCharging)
            return;

        if (_dashCount >= _dashMaxCount)
        {
            _isDashCharging = false;
            _dashChargeTimer = 0f;
            return;
        }

        _dashChargeTimer -= Time.deltaTime;

        if (_dashChargeTimer > 0f)
            return;

        _dashCount++;

        if (_dashCount < _dashMaxCount)
        {
            _dashChargeTimer = _dashCoolTime;
        }
        else
        {
            _isDashCharging = false;
            _dashChargeTimer = 0f;
        }
    }

    private void UpdateMoveAnimation()
    {
        bool isMove = _inputHandle.MoveInputV3.sqrMagnitude > 0.01f;
        _animationController.SetMove(isMove);
    }
}
