using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;
using System.Xml.Serialization;
// Piece

public abstract class Piece : MonoBehaviour
{
    [SerializeField] private MaterialSetter _materialSetter;
    [SerializeField] private GameObject _activeView;
    [SerializeField] private TimerBarUI _timerBarUI;

    private PieceData _pieceData;
    public Tile CurrentTile => _pieceData.CurrentTile;
    public TeamColor TeamColor => _pieceData.TeamColor;
    public bool HasMoved => _pieceData.HasMoved;
    public List<Tile> AvailableMoves => _pieceData.AvailableMoves;

    private IObjectTweener _tweener;

    public abstract List<Tile> SelectAvailableTiles();

    private void Awake()
    {
        _tweener = GetComponent<IObjectTweener>();
    }

    public void SetMaterial(Material material)
    {
        _materialSetter.SetSingleMaterial(material);
    }

    public void SetAcitveView(bool isActive) => _activeView.SetActive(isActive);

    public bool IsFromSameTeam(Piece otherPiece) => otherPiece != null && otherPiece.TeamColor == TeamColor;

    public bool CanMoveToTile(Tile tile) => AvailableMoves.Contains(tile);

    public bool TryAddMove(Tile tile)
    {
        if (!GridSystem.Instance.CheckIfCoordsAreValid(tile.Position)) return false;
        
        AvailableMoves.Add(tile);
        return true;
    }

    public void Initialize(PieceData pieceData)
    {
        _pieceData = pieceData;
        _pieceData.CurrentTile?.SetPiece(this);
        transform.position = CurrentTile.GetPositionForPiece();
        transform.parent = CurrentTile.transform;
    }
    public void SetNewTile(Tile tile)
    {
        transform.parent = tile.transform;
        _pieceData.SetCurrentTile(tile);
    }

    public int GetManaCost() => _pieceData.ManaCost;
    public float GetTimeDeltaBetweenMoves() => _pieceData.TimeDelataBetweenMoves;
    public bool CanMove() => _pieceData.CanMove;
    public void SetCanMove(bool canMove) => _pieceData.CanMove = canMove;
    public Tween MoveToTile(Tile targetTile) => _tweener.MoveToPosition(targetTile.GetPositionForPiece());
    public void UpdateTimerBarUI(float progress) => _timerBarUI.SetProgressBar(progress);
    internal void HideTimerUI() => _timerBarUI.Hide();

}

public class PieceData
{
    public TeamColor TeamColor { get; private set; }
    public PieceType PieceType { get; private set; }
    public Tile CurrentTile { get; private set; }
    public bool HasMoved { get; private set; }
    public bool CanMove { get; set; }

    public int ManaCost
    {
        get
        {
            return PieceType switch
            {
                PieceType.Pawn => 1,
                PieceType.Knight => 3,
                PieceType.Bishop => 3,
                PieceType.Rook => 5,
                PieceType.Queen => 7,
                PieceType.King => 0,
                _ => 0
            };
        }
    }

    public float TimeDelataBetweenMoves
    {
        get
        {
            return PieceType switch
            {
                PieceType.Pawn => 0.4f,
                PieceType.Knight => 0.6f,
                PieceType.Bishop => 0.6f,
                PieceType.Rook => 0.8f,
                PieceType.Queen => 1f,
                PieceType.King => 0.2f,
                _ => 0.1f
            };
        }
    }

    public List<Tile> AvailableMoves { get; private set; } = new List<Tile>();

    public PieceData(TeamColor teamColor, PieceType pieceType, Tile currentTile)
    {
        TeamColor = teamColor;
        PieceType = pieceType;
        CurrentTile = currentTile;
        HasMoved = false;
        CanMove = true;
    }
    public void SetCurrentTile(Tile newTile)
    {
        CurrentTile = newTile;
        HasMoved = true;
    }
}

