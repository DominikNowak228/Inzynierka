using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceCreator : MonoBehaviour
{
    [SerializeField] private GameObject[] _piecePrefabs;
    [SerializeField] private Material _whiteMaterial;
    [SerializeField] private Material _blackMaterial;

    private Dictionary<string, GameObject> _piecePrefabDict = new();

    private void Awake()
    {
        foreach (var prefab in _piecePrefabs)
        {
            var piece = prefab.GetComponent<Piece>();
            if (piece != null)
            {
                _piecePrefabDict[piece.GetType().Name] = prefab;
            }
        }
    }

    public GameObject CreatePiece(PieceType pieceType)
    {
        string key = pieceType.ToString();
        if (!_piecePrefabDict.ContainsKey(key))
            throw new ArgumentException($"PieceCreator: No prefab found for type {key}");

        return Instantiate(_piecePrefabDict[key], transform);
    }

    public Material GetTeamMaterial(TeamColor teamColor)
    {
        return teamColor == TeamColor.White ? _whiteMaterial : _blackMaterial;
    }

}
