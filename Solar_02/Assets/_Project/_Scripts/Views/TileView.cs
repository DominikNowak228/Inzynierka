using UnityEngine;

public class TileView : MonoBehaviour
{
    public enum TileColor
    {
        White,
        Black
    }

    [SerializeField] private Color _firstColor;
    [SerializeField] private Color _secondColor;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetTileColor(TileColor color)
    {
        _spriteRenderer.color = color == TileColor.White ? _firstColor : _secondColor;
    }
}
