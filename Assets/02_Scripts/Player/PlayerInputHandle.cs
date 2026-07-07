using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandle : MonoBehaviour
{
    public Vector2 MoveInput => _moveInput;
    public Vector2 PointInput { get; private set; }

    public event Action OnLeftClickEvent;
    
    // Click
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _rayDistance = 5f;

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
        _inputSystem.Player.ActiveSkill.started += OnActiveSkill;
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
        _inputSystem.Player.ActiveSkill.started -= OnActiveSkill;
        _inputSystem.Player.Point.performed -= OnPoint;

        _inputSystem.Player.Move.performed -= OnMove;

        _inputSystem.Player.Move.canceled -= StopMove;
    }

    public void OnLeftClick(InputAction.CallbackContext context) 
    {
        OnLeftClickEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context) 
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnPoint(InputAction.CallbackContext context) 
    {
        PointInput = context.ReadValue<Vector2>();
    }

    private void StopMove(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }





    public void OnRightClick(InputAction.CallbackContext context) { }
    public void OnDash(InputAction.CallbackContext context) { }
    public void OnInteract(InputAction.CallbackContext context) { }
    public void OnActiveSkill(InputAction.CallbackContext context) { }
}
