using System.Collections.Generic;
using UnityEngine;

public class ModifierEnemyManaRegenEffect : Effect
{
    [SerializeField] private float modifierAmount;
    [SerializeField] private float modifierTime;
    [SerializeField] private CardOwner Owner;
    public override GameAction GetGameAction(List<Tile> targets)
    {
        ModifierEnemyManaRegenGameAciton modGA = new(modifierAmount, modifierTime, Owner);
        return modGA;
    }
}
