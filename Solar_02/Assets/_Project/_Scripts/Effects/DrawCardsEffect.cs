using System.Collections.Generic;
using UnityEngine;

public class DrawCardsEffect : Effect
{
    [SerializeField] private int drawAmount;
    public override GameAction GetGameAction(List<Tile> targets)
    {
        DrawCardsGA drawCardsGA = new DrawCardsGA(drawAmount);
        return drawCardsGA;
    }
}
