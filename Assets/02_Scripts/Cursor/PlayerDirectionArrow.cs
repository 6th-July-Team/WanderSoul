using UnityEngine;

public class PlayerDirectionArrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _arrowRoot;
    [SerializeField] private PlayerAimHandler _aimHandler;
    private Camera _mainCamera;

    [Header("Rotation")]
    [SerializeField] private float _yawOffset = 0f;

    private void Awake()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;

        _aimHandler = GetComponentInParent<PlayerAimHandler>();
    }

    private void LateUpdate()
    {
        if (!_aimHandler.HasValidAim)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(_aimHandler.AimDirection, Vector3.up);
        _arrowRoot.rotation = targetRotation * Quaternion.Euler(0f, _yawOffset, 0f);
    }
}
