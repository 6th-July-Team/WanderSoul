using Unity.Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] [Range(0f, 90f)] private float _rotationX;
    [SerializeField] private float _lens;

    private CinemachineCamera _chinemachineCamera;

    private void Awake()
    {
        _rotationX = transform.rotation.eulerAngles.x;

        _chinemachineCamera = GetComponent<CinemachineCamera>();

        _lens = _chinemachineCamera.Lens.FieldOfView;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(_rotationX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        _chinemachineCamera.Lens.FieldOfView = _lens;
    }
}
