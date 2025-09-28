using System;
using UnityEngine;

public class ArrowView : MonoBehaviour
{
    [SerializeField] private GameObject _arrowHead;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Camera _camera;

    private Vector3 _startPosition = new Vector3(-20f, 0f, 0f);

    internal void SetupArrow(Vector3 startPosition)
    {
        _startPosition = startPosition;
        _lineRenderer.SetPosition(0, _lineRenderer.transform.InverseTransformPoint(startPosition));
        _lineRenderer.SetPosition(1, _lineRenderer.transform.InverseTransformPoint(startPosition));
    }

    private void Update()
    {
        Vector3 endPosition = MouseUtil.GetMouseWorldPositionForCard();
        Vector3 dir = endPosition - _startPosition;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f) dir = Vector3.right;
        Vector3 dirNormalized = dir.normalized;

        Vector3 rightDir = -dirNormalized;

        Vector3 forwardDir = Vector3.Cross(Vector3.up, rightDir);
        if (forwardDir.sqrMagnitude < 0.0001f) forwardDir = Vector3.forward;

        Quaternion targetRot = Quaternion.LookRotation(forwardDir, Vector3.up);

        Quaternion offset = Quaternion.Euler(90f, 0f, 0f);

        _arrowHead.transform.rotation = targetRot * offset;

        _lineRenderer.SetPosition(1,
            _lineRenderer.transform.InverseTransformPoint(endPosition - dirNormalized * 0.5f));
        _arrowHead.transform.position = endPosition;
    }


}
