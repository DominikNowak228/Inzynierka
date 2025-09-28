using System;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InteractSystem : Singleton<InteractSystem>
{
    //Evnety dla zaznaczenia nowej bierki
    public static event EventHandler<NewTileSelectedEventArgs> NewTileSelected;
    public class NewTileSelectedEventArgs : EventArgs
    {
        public Tile SelectedTile { get; set; }
        public NewTileSelectedEventArgs(Tile selectedTile)
        {
            SelectedTile = selectedTile;
        }
    }

    // property 
    private Tile _selectedTile = null;
    private Tile _currentFocusTile = null;
    private Tile _lastCurrentFocusedTile = null;


    private void Awake()
    {
        base.Awake();
        InputSystem.MouseInteractEvent += HandleMouseInteract;
    }

    private void OnDestroy()
    {
        InputSystem.MouseInteractEvent -= HandleMouseInteract;
    }


    private void Update()
    {
        HandleHover();
    }

    // Obs³uga klikniêcia mysz¹
    private void HandleMouseInteract(object sender, EventArgs e)
    {
        _selectedTile?.SetActiveView(false);
        _selectedTile?.Piece?.SetAcitveView(false);

        _selectedTile = _currentFocusTile != null ? _currentFocusTile : null;

        NewTileSelected?.Invoke(this, new NewTileSelectedEventArgs(_selectedTile));
    }

    // uproszczona logika HandleHover
    private void HandleHover()
    {
        IInteractable interactable = GetInteractableUnderMouse();
        Tile newFocusTile = null;

        if (interactable != null)
        {
            var go = interactable.transform.gameObject;
            if (go.TryGetComponent<Tile>(out Tile tile))
                newFocusTile = tile;
            else if (go.TryGetComponent<Piece>(out Piece piece))
                newFocusTile = piece.CurrentTile;
        }

        if (_lastCurrentFocusedTile != null && _lastCurrentFocusedTile != _selectedTile && _lastCurrentFocusedTile != newFocusTile)
        {
            _lastCurrentFocusedTile.SetActiveView(false);
            _lastCurrentFocusedTile.Piece?.SetAcitveView(false);
        }

        if (newFocusTile != null && newFocusTile != _selectedTile)
        {
            newFocusTile.SetActiveView(true);
            newFocusTile.Piece?.SetAcitveView(true);
        }

        _currentFocusTile = newFocusTile;
        _lastCurrentFocusedTile = newFocusTile;
    }

    private IInteractable GetInteractableUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
        {
            return hitInfo.collider.GetComponent<IInteractable>();
        }
        return null;
    }

    public bool PlayerIsDragging { get; set; } = false;
    public bool PlayerCanInteract()
    {
        if (!ActionSystem.Instance.isPerforming) return true;
        return false;
    }
    public bool PlayerCanHover()
    {
        if (!PlayerIsDragging) return true;
        return false;
    }
}

public class MouseUtil
{
    private static Camera _secondaryCamera = Camera.allCameras.Length > 1 ? Camera.allCameras[1] : Camera.main;
    private static Camera _mainCamera = Camera.main;

    public static Vector3 GetMouseWorldPositionForCard(float zValue = 0)
    {
        Plane plane = new Plane(-_secondaryCamera.transform.forward, new Vector3(0, 0, zValue));
        Ray ray = _secondaryCamera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    public static Vector3 GetMouseWorldPositionForPiece(float zValue = 0)
    {
        Plane plane = new Plane(-_mainCamera.transform.forward, new Vector3(0, 0, zValue));
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    public static Vector3 GetWorldPositionWithMainCamera()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.point;
        }
        return Vector3.positiveInfinity;
    }

    public static Tile GetTileUnderCursor()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
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
}
