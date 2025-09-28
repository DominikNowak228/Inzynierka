using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : Singleton<EffectSystem>
{
    private void OnEnable()
    {
        ActionSystem.AttachCoroutinePerformer<PerformEffectGA>(PerformEffectPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<PerformEffectGA>();
    }

    //Peroformeres
    private IEnumerator PerformEffectPerformer(PerformEffectGA performEffectGA)
    {
        GameAction gameAction = performEffectGA.Effect.GetGameAction(performEffectGA.Targets);
        ActionSystem.Instance.AddReaction(gameAction);
        yield return null;
    }
}
