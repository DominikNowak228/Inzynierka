using UnityEngine;

public class MovePieceGA : GameAction
{
    public Piece Piece { get; private set; }
    public Tile TargetTile { get; private set; }

    public MovePieceGA(Piece piece, Tile targetTile)
    {
        Piece = piece;
        TargetTile = targetTile;
    }
}
