using UnityEditor;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileView _tileView;
    [SerializeField] private TileHighlightView _highlightView;
    [SerializeField] private GameObject _activeView;
    [SerializeField] private Transform _slotForPiece;

    private TileData _tileData;

    public bool IsActiveTile { get; private set; }

    public void Setup(TileData _tileData)
    {
        this._tileData = _tileData;
        ChangeColor();
    }
   

    public void SetActiveView(bool isActive)
    {
        _activeView.SetActive(isActive);
        
        
    }

    public void SetPiece(Piece piece) => _tileData.SetCurrentPiece(piece);
    private void ChangeColor() => _tileView.SetTileColor(_tileData.IsOdd ? TileView.TileColor.White : TileView.TileColor.Black);
    public void SetHighlight(TileHighlightView.HighlightState highlightState) => _highlightView.SetHighlight(highlightState);
    public void ClearHighlight() => _highlightView.SetHighlight(TileHighlightView.HighlightState.nullTile);
    public Vector3 GetPositionForPiece() => _slotForPiece.position;
    public Piece Piece => _tileData.CurrentPiece;
    public Vector2Int Position => _tileData.Position;

}
public class TileData
{
    public Vector2Int Position { get; private set; }
    public Piece CurrentPiece { get; private set; }
    public bool IsOdd{ get; private set; }

    public TileData(Vector2Int position, bool isOdd, Piece currentPiece =null)
    {
        Position = position;
        CurrentPiece = currentPiece;
        IsOdd = isOdd;
    }

    public void SetCurrentPiece(Piece piece)
    {
        CurrentPiece = piece;
    }
}
