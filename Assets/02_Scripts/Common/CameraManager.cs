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
        _cinemachineCamera.Follow = null;
    }
}
