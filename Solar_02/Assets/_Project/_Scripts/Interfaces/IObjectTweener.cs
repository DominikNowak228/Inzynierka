using System;
using UnityEngine;
using DG.Tweening;
public interface IObjectTweener
{
    Tween MoveToPosition(Vector3 position, Action onComplete = null);
}
