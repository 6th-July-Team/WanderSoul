using UnityEngine;
using static UnityEditor.Profiling.HierarchyFrameDataView;

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
    private Vector3 _dashDirection;
    [SerializeField] private float _dashSpeed = 15f;
    [SerializeField] private float _dashDuration = 0.2f;
    private float _dashTimer;
    private bool _isDashing;

    // Components
    private PlayerInputHandle _inputHandle;
    private CharacterController _characterController;
    private PlayerAnimationController _animationController;

    // 캐싱
    private Vector3 _horizontalVelocity;
    private PlayerViewModel _viewModel;

    private void Awake()
    {
        _inputHandle = GetComponent<PlayerInputHandle>();
        _characterController = GetComponent<CharacterController>();
        _animationController = GetComponent<PlayerAnimationController>();

        _viewModel = GameManager.Network.RequestCreatePlayer();
        _isDashing = false;
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
        if (_isDashing || _viewModel.GetDashCount <= 0)
            return;

        if (!_viewModel.TryDash())
            return;

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

    private void UpdateMoveAnimation()
    {
        //_horizontalVelocity = _characterController.velocity;
        //_horizontalVelocity.y = 0f;

        //float normalizedSpeed = _horizontalVelocity.magnitude / _moveSpeed;

        float moveSpeed = _inputHandle.MoveInputV3.sqrMagnitude > 0.01f ? 1f : 0f;

        _animationController.SetMoveSpeed(moveSpeed);
    }
}
