using System;
using System.Collections.Generic;
using UnityEngine;
using static InteractSystem;

public class PiecePlayerSystem : Singleton<PiecePlayerSystem>
{
    private TeamColor _teamColor;
    private List<Piece> _activePieces = new List<Piece>();

    private Piece _selectedPiece;
    private List<Tile> _posibleMoves;

    private void Awake()
    {
        base.Awake();
        InteractSystem.NewTileSelected += OnNewTileSelected;
    }

    private void OnDestroy()
    {
        InteractSystem.NewTileSelected -= OnNewTileSelected;
    }

    private void OnNewTileSelected(object sender, NewTileSelectedEventArgs e)
    {
        GridSystem.Instance.GetTiles().ForEach(t => t.ClearHighlight());

        var selectedTile = e.SelectedTile;
        var tilePiece = selectedTile?.Piece;

        if (tilePiece != null)
        {
            if (tilePiece.TeamColor == _teamColor)
            {
                _selectedPiece = tilePiece;
                _selectedPiece.AvailableMoves.Clear();
                _posibleMoves = _selectedPiece.SelectAvailableTiles();
                ShowPosibleMoves();
            }
            else
            {
                if (_selectedPiece != null && _selectedPiece.CanMoveToTile(selectedTile))
                {
                    // Tutaj mo¿na dodaæ logikê ataku
                    AttackMovePieceGA attackMovePieceGA = new(_selectedPiece, tilePiece);
                    ActionSystem.Instance.Perform(attackMovePieceGA);

                    _selectedPiece = null;
                    _posibleMoves = null;
                }
                // else: nie robimy nic, mo¿na dodaæ dŸwiêk b³êdu
            }
        }
        else
        {
            if (_selectedPiece != null && _selectedPiece.CanMoveToTile(selectedTile))
            {
                // Ruch na wolne pole
                MovePieceGA movePieceGA = new(_selectedPiece, selectedTile);
                ActionSystem.Instance.Perform(movePieceGA);
            }
            // else: nie robimy nic, mo¿na dodaæ dŸwiêk b³êdu

            _selectedPiece = null;
            _posibleMoves = null;
        }
    }
    private void ShowPosibleMoves()
    {
        foreach (var tile in _posibleMoves)
        {
            if (tile.Piece != null && tile.Piece.TeamColor != _teamColor)
                tile.SetHighlight(TileHighlightView.HighlightState.EnemyTile);
            else
                tile.SetHighlight(TileHighlightView.HighlightState.FreeTile);
        }
    }

    public void Setup(TeamColor teamColor)
    {
        _teamColor = teamColor;
        FindAllPiecesAtColor(_teamColor);
    }

    public void AddPiece(Piece piece)
    {
        if (!_activePieces.Contains(piece))
            _activePieces.Add(piece);
    }
    public void RemovePiece(Piece piece)
    {
        if (_activePieces.Contains(piece))
            _activePieces.Remove(piece);
    }

    private void FindAllPiecesAtColor(TeamColor teamColor)
    {
        var Board = GameObject.Find("Board");
        if (Board != null)
        {
            var pieces = Board.GetComponentsInChildren<Piece>();
            foreach (var piece in pieces)
            {
                if (piece.TeamColor == teamColor)
                {
                    _activePieces.Add(piece);
                }
            }
        }
        Debug.Log($"Found {_activePieces.Count} pieces for team {teamColor}");
    }




}


