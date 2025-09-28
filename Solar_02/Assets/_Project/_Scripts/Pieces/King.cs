using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private List<Vector2Int> direction = new List<Vector2Int> {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
    new Vector2Int(1,1),
    new Vector2Int(1,-1),
    new Vector2Int(-1,1),
    new Vector2Int(-1,-1)

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

            if (pieceAtNextPos != null && pieceAtNextPos != this)
            {
                if (!IsFromSameTeam(pieceAtNextPos))
                    TryAddMove(GridSystem.Instance.GetTileAtCoords(nextPos));
            }
        }
        return AvailableMoves;
    }
}
