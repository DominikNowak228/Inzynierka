using System.Collections.Generic;
using UnityEngine;

public class ModifierManaRegenEffect : Effect
{
    [SerializeField] private float modifierAmount;
    [SerializeField] private float modifierTime;
    [SerializeField] private CardOwner Owner;
    public override GameAction GetGameAction(List<Tile> targets)
    {
        ModifierManaRegenGameAciton modGA = new(modifierAmount, modifierTime, Owner);
        return modGA;
    }
}