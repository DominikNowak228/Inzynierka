using UnityEngine;

public class SpawnPieceGA : GameAction
{
    public PieceType PieceType { get; private set; }
    public TeamColor TeamColor { get; private set; }
    public Vector2Int Position { get; private set; }

    public SpawnPieceGA(PieceType pieceType, TeamColor teamColor, Vector2Int position)
    {
        PieceType = pieceType;
        TeamColor = teamColor;
        Position = position;
    }

}
