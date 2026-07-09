using UnityEngine;

public class PlayerAimHandler : MonoBehaviour
{
    private PlayerInputHandle _inputHandle;
    private Camera _mainCamera;

    [Header("Ground")]
    [SerializeField] private float _groundY = 0f;

    public Vector3 AimWorldPoint { get; private set; }
    public Vector3 AimDirection { get; private set; } = Vector3.forward;
    public bool HasValidAim { get; private set; }

    private void Awake()
    {
        if (_inputHandle == null)
            _inputHandle = GetComponent<PlayerInputHandle>();

        if (_mainCamera == null)
            _mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateAim();
    }

    private void UpdateAim()
    {
        HasValidAim = false;

        if (_inputHandle == null || _mainCamera == null)
            return;

        if (!TryGetGroundPoint(_inputHandle.PointInput, out Vector3 groundPoint))
            return;

        Vector3 direction = groundPoint - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        AimWorldPoint = groundPoint;
        AimDirection = direction.normalized;
        HasValidAim = true;
    }

    private bool TryGetGroundPoint(Vector2 screenPosition, out Vector3 groundPoint)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, _groundY, 0f));

        if (groundPlane.Raycast(ray, out float distance))
        {
            groundPoint = ray.GetPoint(distance);
            return true;
        }

        groundPoint = default;
        return false;
    }
}
