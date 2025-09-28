using UnityEngine;
using UnityEngine.UI;
public class ManualTargetSystem : Singleton<ManualTargetSystem>
{
    [SerializeField] private ArrowView _arrowView;
    [SerializeField] private LayerMask _targetLayerMask;

    public void StartTargerting(Vector3 startPosition)
    {
        _arrowView.gameObject.SetActive(true);
        _arrowView.SetupArrow(startPosition);
    }
    public Tile EndTargeting(Vector3 endPosition)
    {
        _arrowView.gameObject.SetActive(false);
        if (Physics.Raycast(endPosition, Vector3.down, out RaycastHit hit, _targetLayerMask)
            && hit.collider != null)
        {
            if (hit.collider.TryGetComponent<Tile>(out Tile tile))
            {
                return tile;
            }
            else if (hit.collider.TryGetComponent<Piece>(out Piece piece))
            {
                return piece.CurrentTile;
            }
        }
        return null;
    }
    public Tile EndTargeting(Tile tile)
    {
        _arrowView.gameObject.SetActive(false);
        return tile;
    }

}