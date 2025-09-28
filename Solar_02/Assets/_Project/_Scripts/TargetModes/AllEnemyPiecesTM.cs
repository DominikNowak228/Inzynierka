using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllEnemyPiecesTM : TargetMode
{
    public override List<Tile> GetTargets()
    {
        return GridSystem.Instance.GetTiles().Where(x => x.Piece != null && x.Piece.TeamColor == TeamColor.Black).ToList();
    }
}
