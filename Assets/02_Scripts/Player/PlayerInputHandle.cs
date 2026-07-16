using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandle : MonoBehaviour
{
    public Vector2 MoveInputV2 => _moveInput;
    public Vector3 MoveInputV3 => new Vector3(_moveInput.x, 0f, _moveInput.y);
    public Vector2 PointInput { get; private set; }

    public event Action OnBasicAttackEvent;

    public event Action OnSpecialAttackEvent;
    public event Action OnSpecialAttackCheckEvent;

    public event Action OnUltimateSkillEvent;
    public event Action OnUltimateSkillCheckEvent;

    public event Action OnDashEvent;

    // Move
    private Vector2 _moveInput;

    // Input System
    private InputSystem_Default _inputSystem;

    void Awake()
    {
        _inputSystem = new InputSystem_Default();
    }

    void OnEnable()
    {
        _inputSystem.Player.Enable();

        //_inputSystem.Player.BasicAttack.started += OnBasicAttack;

        _inputSystem.Player.SpecialAttak.started += OnSpecialAttackCheck;
        _inputSystem.Player.SpecialAttak.canceled += OnSpecialAttack;

        _inputSystem.Player.UltimateAttack.started += OnUltimateSkillCheck;
        _inputSystem.Player.UltimateAttack.canceled += OnUltimateAttack;

        _inputSystem.Player.Dash.started += OnDash;
        _inputSystem.Player.Interact.started += OnInteract;
        _inputSystem.Player.Point.started += OnPoint;
        _inputSystem.Player.Point.performed += OnPoint;

        _inputSystem.Player.Move.performed += OnMove;

        _inputSystem.Player.Move.canceled += StopMove;
    }

    void OnDisable()
    {
        _inputSystem.Player.Disable();

        //_inputSystem.Player.BasicAttack.started -= OnBasicAttack;

        _inputSystem.Player.SpecialAttak.started -= OnSpecialAttackCheck;
        _inputSystem.Player.SpecialAttak.canceled -= OnSpecialAttack;

        _inputSystem.Player.UltimateAttack.started -= OnUltimateSkillCheck;
        _inputSystem.Player.UltimateAttack.canceled -= OnUltimateAttack;

        _inputSystem.Player.Dash.started -= OnDash;
        _inputSystem.Player.Interact.started -= OnInteract;
        _inputSystem.Player.Point.started -= OnPoint;
        _inputSystem.Player.Point.performed -= OnPoint;

        _inputSystem.Player.Move.performed -= OnMove;

        _inputSystem.Player.Move.canceled -= StopMove;
    }

    private void Update()
    {
        if (_inputSystem.Player.BasicAttack.IsPressed())
        {
            OnBasicAttackEvent?.Invoke();
        }
    }

    public void OnBasicAttack(InputAction.CallbackContext context) => OnBasicAttackEvent?.Invoke();

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnPoint(InputAction.CallbackContext context) => PointInput = context.ReadValue<Vector2>();

    private void StopMove(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }

    public void OnDash(InputAction.CallbackContext context) => OnDashEvent?.Invoke();
    public void OnSpecialAttack(InputAction.CallbackContext context) => OnSpecialAttackEvent?.Invoke();
    public void OnUltimateAttack(InputAction.CallbackContext context) => OnUltimateSkillEvent?.Invoke();

    public void OnSpecialAttackCheck(InputAction.CallbackContext context) => OnSpecialAttackCheckEvent?.Invoke();
    public void OnUltimateSkillCheck(InputAction.CallbackContext context) => OnUltimateSkillCheckEvent?.Invoke();

    public void OnInteract(InputAction.CallbackContext context) { }
}
