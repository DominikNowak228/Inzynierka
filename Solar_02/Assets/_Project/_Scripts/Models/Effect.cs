using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Effect
{
    public enum TargetManualType
    {
        Enemy, Ally, Any, EmptyTile, None
    }
    public abstract GameAction GetGameAction(List<Tile> targets);
}
