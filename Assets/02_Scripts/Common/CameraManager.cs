using Unity.Cinemachine;
using UnityEngine;

public class CameraManager
{
    private CinemachineCamera _cinemachineCamera;
    private Camera _mainCamera;


    public void InitInGame(Transform playerTS)
    {
        if(_mainCamera == null)
        {
            _mainCamera = GameObject.Instantiate(Utils.ResourcesLoad<Camera>("Camera/Main Camera"));
        }

        if (_cinemachineCamera == null)
        {
            _cinemachineCamera = GameObject.Instantiate(Utils.ResourcesLoad<CinemachineCamera>("Camera/InGameCamera"));
        }

        _cinemachineCamera.Follow = playerTS;
    }

    public void Release()
    {
        if (_cinemachineCamera != null)
        {
            _cinemachineCamera.Follow = null;

            GameObject.Destroy(_cinemachineCamera.gameObject);
            _cinemachineCamera = null;
        }

        if (_mainCamera != null)
        {
            GameObject.Destroy(_mainCamera.gameObject);
            _mainCamera = null;
        }
    }
}
