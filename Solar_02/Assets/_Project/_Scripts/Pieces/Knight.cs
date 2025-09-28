using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    List<Vector2Int> direction = new List<Vector2Int>
    {
        new Vector2Int(1, 2),
        new Vector2Int(1, -2),
        new Vector2Int(-1, 2),
        new Vector2Int(-1, -2),
        new Vector2Int(2, 1),
        new Vector2Int(2, -1),
        new Vector2Int(-2, 1),
        new Vector2Int(-2, -1)
    };
    public override List<Tile> SelectAvailableTiles()
    {
        foreach (var dir in direction)
        {
            var nextPos = CurrentTile.Position + dir;

            if (!GridSystem.Instance.CheckIfCoordsAreValid(nextPos))
                continue;

            Piece pieceAtNextPos = GridSystem.Instance.GetPieceAtCoords(nextPos);
            if (pieceAtNextPos == null)
                TryAddMove(GridSystem.Instance.GetTileAtCoords(nextPos));

            if (pieceAtNextPos != null && !IsFromSameTeam(pieceAtNextPos))
                TryAddMove(GridSystem.Instance.GetTileAtCoords(nextPos));

        }
        return AvailableMoves;
    }
}
