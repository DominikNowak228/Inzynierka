using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }

    [SerializeField] private mode _mode = mode.LookAt;
    [SerializeField] private Camera _camera;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }


    private void LateUpdate()
    {
        switch (_mode)
        {
            case mode.LookAt:
                transform.LookAt(_camera.transform);
                break;
            case mode.LookAtInverted:
                Vector3 dir = transform.position - _camera.transform.position;
                transform.LookAt(transform.position + dir);
                break;
            case mode.CameraForward:
                transform.forward = _camera.transform.forward;
                break;
            case mode.CameraForwardInverted:
                transform.forward = -_camera.transform.forward;
                break;
        }
    }
}
