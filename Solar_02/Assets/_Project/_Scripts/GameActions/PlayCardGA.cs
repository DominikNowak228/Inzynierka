using UnityEngine;

public class PlayCardGA : GameAction
{
    public Card Card { get; private set; }
    public Tile TileTarget { get; private set; }
    public PlayCardGA(Card card)
    {
        Card = card;
        TileTarget = null;
    }

    public PlayCardGA(Card card, Tile tileTarget)
    {
        Card = card;
        TileTarget = tileTarget;
    }
}
