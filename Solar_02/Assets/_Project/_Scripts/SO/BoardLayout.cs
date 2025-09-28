using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardLayout", menuName = "ScriptableObjects/Board/Layout")]
public class BoardLayout : ScriptableObject
{
    [Serializable]
    private class BoardSquareSetup
    {
        public Vector2Int position;
        public PieceType pieceType;
        public TeamColor teamColor;
    }
    [SerializeField] private BoardSquareSetup[] boardSetups;

    public int GetPieceCount()
    {
        return boardSetups.Length;
    }

    public Vector2Int GetSquareCoordsAtIndex(int index)
    {
        if (index < 0 || index >= boardSetups.Length)
        {
            throw new IndexOutOfRangeException("Index out of range in BoardLayout.GetSquareCoordsAtIndex");
        }
        return boardSetups[index].position;
    }

    public PieceType GetPieceTypeAtIndex(int index)
    {
        if (index < 0 || index >= boardSetups.Length)
        {
            throw new IndexOutOfRangeException("Index out of range in BoardLayout.GetPieceTypeAtIndex");
        }
        return boardSetups[index].pieceType;
    }

    public TeamColor GetTeamColorAtIndex(int index)
    {
        if (index < 0 || index >= boardSetups.Length)
        {
            throw new IndexOutOfRangeException("Index out of range in BoardLayout.GetTeamColorAtIndex");
        }
        return boardSetups[index].teamColor;
    }
}
