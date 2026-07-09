using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandle : MonoBehaviour
{
    public Vector2 MoveInputV2 => _moveInput;
    public Vector3 MoveInputV3 => new Vector3(_moveInput.x, 0f, _moveInput.y);
    public Vector2 PointInput { get; private set; }

    public event Action OnUltimateSkillEvent;
    public event Action OnLeftClickEvent;
    public event Action OnRightClickEvent;
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

        _inputSystem.Player.LeftClick.started += OnLeftClick;
        _inputSystem.Player.RightClick.started += OnRightClick;
        _inputSystem.Player.Dash.started += OnDash;
        _inputSystem.Player.Interact.started += OnInteract;
        _inputSystem.Player.UltimateSkill.started += OnUltimateSkill;
        _inputSystem.Player.Point.performed += OnPoint;

        _inputSystem.Player.Move.performed += OnMove;

        _inputSystem.Player.Move.canceled += StopMove;
    }

    void OnDisable()
    {
        _inputSystem.Player.Disable();

        _inputSystem.Player.LeftClick.started -= OnLeftClick;
        _inputSystem.Player.RightClick.started -= OnRightClick;
        _inputSystem.Player.Dash.started -= OnDash;
        _inputSystem.Player.Interact.started -= OnInteract;
        _inputSystem.Player.UltimateSkill.started -= OnUltimateSkill;
        _inputSystem.Player.Point.performed -= OnPoint;

        _inputSystem.Player.Move.performed -= OnMove;

        _inputSystem.Player.Move.canceled -= StopMove;
    }

    public void OnLeftClick(InputAction.CallbackContext context) => OnLeftClickEvent?.Invoke();

    public void OnMove(InputAction.CallbackContext context) => _moveInput = context.ReadValue<Vector2>();

    public void OnPoint(InputAction.CallbackContext context) => PointInput = context.ReadValue<Vector2>();

    private void StopMove(InputAction.CallbackContext context) => _moveInput = Vector2.zero;

    public void OnDash(InputAction.CallbackContext context) => OnDashEvent?.Invoke();
    public void OnRightClick(InputAction.CallbackContext context) => OnRightClickEvent?.Invoke();
    public void OnUltimateSkill(InputAction.CallbackContext context) => OnUltimateSkillEvent?.Invoke();




    public void OnInteract(InputAction.CallbackContext context) { }
}
