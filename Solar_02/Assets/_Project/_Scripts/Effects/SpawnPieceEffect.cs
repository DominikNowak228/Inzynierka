using System.Collections.Generic;
using UnityEngine;

public class SpawnPieceEffect : Effect
{
    [SerializeField] private PieceType pieceType;
    [SerializeField] private TeamColor teamColor;
    public override GameAction GetGameAction(List<Tile> targets)
    {
        Vector2Int position = targets[0].Position;
        SpawnPieceGA spawnPieceGA = new SpawnPieceGA(pieceType, teamColor, position);
        return spawnPieceGA;
    }
}
