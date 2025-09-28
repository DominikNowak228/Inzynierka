using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{

    public override List<Tile> SelectAvailableTiles()
    {
        Vector2Int direction = TeamColor == TeamColor.White ? Vector2Int.up : Vector2Int.down;
        int range = HasMoved ? 1 : 2;
        for (int i = 0; i <= range; i++)
        {
            var nextPos = CurrentTile.Position + direction * i;

            if (!GridSystem.Instance.CheckIfCoordsAreValid(nextPos))
                break;

            if(CheckAttack(direction, out var enemies))
                AvailableMoves.AddRange(enemies);

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
        return AvailableMoves;
    }

    private bool CheckAttack (Vector2Int dirMove, out List<Tile> enemyPosition)
    {
        enemyPosition = new();
        Vector2Int[] attackDirections = new Vector2Int[]
        {
            new Vector2Int(1, dirMove.y),
            new Vector2Int(-1, dirMove.y)
        };
        foreach (var item in attackDirections)
        {
            if(GridSystem.Instance.CheckIfCoordsAreValid(CurrentTile.Position + item))
            {
                Piece pieceAtPos = GridSystem.Instance.GetPieceAtCoords(CurrentTile.Position + item);
                if (pieceAtPos != null && !IsFromSameTeam(pieceAtPos))
                    enemyPosition.Add(GridSystem.Instance.GetTileAtCoords(CurrentTile.Position + item));

            }
        }
        return enemyPosition.Count != 0;
    }

    // Add special move for pawn (en passant, promotion, etc.)
}
