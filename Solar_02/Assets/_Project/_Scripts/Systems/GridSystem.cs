using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class GridSystem : Singleton<GridSystem>
{
    public static float CellSize = 1f;
    [SerializeField] private GameObject _tilePrefab;

    public int Width { get; private set; }
    public int Height { get; private set; }

    private Dictionary<Vector2Int, Tile> _tilesDictionary;

    private void Awake()
    {
        base.Awake();
        _tilesDictionary = new();
    }

    public void Setup(int width, int height)
    {
        Width = width;
        Height = height;

        CreateBoard();
    }

    private void CreateBoard()
    {
        var board = new GameObject("Board");
        var viewContainer = GameObject.Find("--- Views ---");
        board.transform.parent = viewContainer.transform;
        board.transform.position = new Vector3(-CellSize * Width / 2 + CellSize / 2, 0, -CellSize * Height / 2 + CellSize / 2);

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Vector2Int coords = new Vector2Int(i, j);

                var tilePrefab = Instantiate(_tilePrefab, board.transform.position + new Vector3(i * CellSize, 0, j * CellSize), Quaternion.identity);
                tilePrefab.name = $"Tile_{i}_{j}";
                tilePrefab.transform.parent = board.transform;

                bool isOdd = (i % 2 == 0 && j % 2 == 1) || (j % 2 == 0 && i % 2 == 1);
                
                TileData tileData = new TileData(coords, isOdd);
                Tile tile = tilePrefab.GetComponent<Tile>();

                tile.Setup(tileData);

                _tilesDictionary[coords] = tile;
            }
        }
    }

    public Tile GetTileAtCoords(Vector2Int squereCoords)
    {
        if (!CheckIfCoordsAreValid(squereCoords))
            throw new ArgumentException($"GridSystem: Coords {squereCoords} are out of bounds");

        var tile = _tilesDictionary.GetValueOrDefault(squereCoords);
        if (tile == null)
            throw new ArgumentException($"GridSystem: No tile found at coords {squereCoords}");

        return tile;
    }

    public Vector2Int GetVector2IntAtTile(Tile tile)
    {
        foreach (var kvp in _tilesDictionary)
        {
            if (kvp.Value == tile)
                return kvp.Key;
        }
        throw new ArgumentException("GridSystem: Tile not found in dictionary");
    }

    public Piece GetPieceAtCoords(Vector2Int squereCoords)
    {
        var tile = GetTileAtCoords(squereCoords);
        return tile.Piece;
    }

    internal bool CheckIfCoordsAreValid(Vector2Int nextPos)
    {
        if (_tilesDictionary.ContainsKey(nextPos))
            return true;
        return false;
    }

    public List<Tile> GetTiles()
    {
        return new List<Tile>(_tilesDictionary.Values);
    }
}
