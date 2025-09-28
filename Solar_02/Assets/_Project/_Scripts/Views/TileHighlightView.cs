using UnityEngine;

public class TileHighlightView: MonoBehaviour
{
    public enum HighlightState
    {
        nullTile,
        FreeTile,
        EnemyTile
    }

    [SerializeField] private Color _freeTileColor;
    [SerializeField] private Color _enemyTileColor;
    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetHighlight(HighlightState highlightState) 
    {
        _spriteRenderer.color = highlightState switch
        {
            HighlightState.nullTile => Color.clear,
            HighlightState.FreeTile => _freeTileColor,
            HighlightState.EnemyTile => _enemyTileColor,
            _ => _enemyTileColor,
        };
    }
}