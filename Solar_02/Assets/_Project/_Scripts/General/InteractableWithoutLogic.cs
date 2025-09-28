using UnityEngine;

public class InteractableWithoutLogic : MonoBehaviour, IInteractable
{
    [SerializeField] private string _displayName = "Interact";
    [SerializeField] private bool _isEnabled = true;
    public string DisplayName => _displayName;
    public bool CanInteract() => _isEnabled;
    public void Interact()
    {
        // No logic here
    }
    public void OnFocusGained()
    {
        
    }
    public void OnFocusLost()
    {
        
    }
}