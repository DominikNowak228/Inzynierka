using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using DG.Tweening;

public class CameraViewSystem : Singleton<CameraViewSystem>
{
    private enum CameraPosition
    {
        Left, Center, Right
    }

    //Chodzi o czas potrzebny na przejscie kamery miedzy dowama punktami
    [SerializeField, Min(0.05f)] private float _timeToMoveBetweenStage = 1f;
    [SerializeField] private GameObject _cameraPivot;

    private bool _isMoving = false;
    private CameraPosition _currentPosition = CameraPosition.Center;

    private void Awake()
    {
        base.Awake();
    }

    public void MoveCamera(float axis)
    {
        if (_isMoving || axis == 0) return;

        bool moveLeft = axis < 0;
        if (moveLeft)
        {
            if (_currentPosition == CameraPosition.Left) return;
            _currentPosition--;
        }
        else
        {
            if (_currentPosition == CameraPosition.Right) return;
            _currentPosition++;
        }
        StartCoroutine(MoveCameraToPosition(_currentPosition));
        _isMoving = true;

    }

    private IEnumerator MoveCameraToPosition(CameraPosition cameraPosition)
    {

        float targetRotationY = cameraPosition switch
        {
            CameraPosition.Left => -90f,
            CameraPosition.Center => 0f,
            CameraPosition.Right => 90f,
            _ => 0f
        };

        var tween = _cameraPivot.transform.DORotate(new Vector3(0, targetRotationY, 0), _timeToMoveBetweenStage);
        yield return tween.WaitForCompletion();
        _isMoving = false;
    }

}
