using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomEnemyPieceTM : TargetMode
{
    public override List<Tile> GetTargets()
    {
        var enemyPieces = GridSystem.Instance.GetTiles().Where(x => x.Piece != null && x.Piece.TeamColor == TeamColor.Black).ToList();
        if (enemyPieces.Count == 0) return new List<Tile>();

        var enemyTarget = enemyPieces[Random.Range(0, enemyPieces.Count)];
        return new List<Tile> { enemyTarget };
    }
}
