using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandle : MonoBehaviour
{
    public event Action<Vector3> OnMoveClickEvent;

    private InputSystem_Default _inputSystem;
    
    // Click Move
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _rayDistance = 5f;
    private Vector3 _movePosition;
    private Vector2 _screenPosition;


    void Awake()
    {
        _inputSystem = new InputSystem_Default();
    }

    void OnEnable()
    {
        _inputSystem.Player.Enable();

        _inputSystem.Player.MoveClick.started += OnMoveClick;
    }

    void OnDisable()
    {
        _inputSystem.Player.Disable();

        _inputSystem.Player.MoveClick.started -= OnMoveClick;
    }

    public void OnMoveClick(InputAction.CallbackContext context)
    {
        _screenPosition = _inputSystem.Player.Point.ReadValue<Vector2>();

        // TODO(김익환): 캐싱 생각해봐야함.
        Ray ray = Camera.main.ScreenPointToRay(_screenPosition);

        // TODO(김익환): 만약 히트가 잘못되어 position이 설정이 이상할 경우를 대비해 젤 처음 플레이어 위치를 넣어줘야 할 듯.
        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _groundLayer))
        {
            _movePosition = hit.point;
        }

        OnMoveClickEvent?.Invoke(_movePosition);
    }


    public void OnAttack(InputAction.CallbackContext context) { }

    public void OnInteract(InputAction.CallbackContext context) { }

    public void OnSkill1(InputAction.CallbackContext context) { }

    public void OnSkill2(InputAction.CallbackContext context) { }

    public void OnSkill3(InputAction.CallbackContext context) { }

    public void OnSkill4(InputAction.CallbackContext context) { }
}
