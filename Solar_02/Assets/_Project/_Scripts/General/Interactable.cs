using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _displayName = "Interact";
    [SerializeField] private bool _isEnabled = true;
    [SerializeField] private GameObject _focuseGFX;
    [SerializeField] private UnityEvent onInteract;
    

    private void Awake()
    {
        _focuseGFX.SetActive(false);
    }

    public string DisplayName => _displayName;
    public bool CanInteract() => _isEnabled;
    public void Interact()
    {
        onInteract?.Invoke();
    }
    public void OnFocusGained()
    {
        _focuseGFX.SetActive(true);
    }
    public void OnFocusLost()
    {
        _focuseGFX.SetActive(false);
    }
}
