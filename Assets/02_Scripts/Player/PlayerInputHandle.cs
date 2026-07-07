using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandle : MonoBehaviour
{
    public Vector2 MoveInput => _moveInput;

    public event Action<Vector3> OnLeftClickEvent;
    
    // Click
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _rayDistance = 5f;
    private Vector3 _movePosition;

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

        _inputSystem.Player.Move.performed -= OnMove;

        _inputSystem.Player.Move.canceled -= StopMove;
    }

    public void OnLeftClick(InputAction.CallbackContext context) 
    {
        Vector2 mouseScreenPosition = _inputSystem.Player.Point.ReadValue<Vector2>();

        float distanceFromCameraToPlayer = Vector3.Dot(transform.position - Camera.main.transform.position, Camera.main.transform.forward);

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, distanceFromCameraToPlayer)
        );

        Vector3 direction = mouseWorldPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            direction.Normalize();
        }

        OnLeftClickEvent?.Invoke(direction);
    }

    public void OnRightClick(InputAction.CallbackContext context) { }
    public void OnDash(InputAction.CallbackContext context) { }
    public void OnInteract(InputAction.CallbackContext context) { }
    public void OnActiveSkill(InputAction.CallbackContext context) { }

    public void OnMove(InputAction.CallbackContext context) 
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void StopMove(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }
}
