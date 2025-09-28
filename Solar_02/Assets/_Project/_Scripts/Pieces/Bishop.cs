using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    private List<Vector2Int> direction = new List<Vector2Int> {
    new Vector2Int(1, 1),
    new Vector2Int(1, -1),
    new Vector2Int(-1, 1),
    new Vector2Int(-1, -1)
    };
    
    public override List<Tile> SelectAvailableTiles()
    {
        int range = GridSystem.Instance.Width > GridSystem.Instance.Height ? GridSystem.Instance.Width : GridSystem.Instance.Height;

        foreach (var dir in direction)
        {
            for (int i = 0; i < range; i++)
            {
                var nextPos = CurrentTile.Position + dir * i;

                if (!GridSystem.Instance.CheckIfCoordsAreValid(nextPos))
                    break;

                Piece pieceAtNextPos = GridSystem.Instance.GetPieceAtCoords(nextPos);
                if (pieceAtNextPos == null)
                    TryAddMove(GridSystem.Instance.GetTileAtCoords(nextPos));
                if (pieceAtNextPos != null && pieceAtNextPos != this)
                {
                    if (!IsFromSameTeam(pieceAtNextPos))
                        TryAddMove(GridSystem.Instance.GetTileAtCoords(nextPos));
                    break;
                }
            }
        }
        return AvailableMoves;
    }
}
