using System;
using UnityEngine;
using DG.Tweening;

public class SimpleTweener : MonoBehaviour, IObjectTweener
{
    public Tween MoveToPosition(Vector3 position, Action onComplete = null)
    {
        return transform.DOMove(position, 0.25f);
    }
}