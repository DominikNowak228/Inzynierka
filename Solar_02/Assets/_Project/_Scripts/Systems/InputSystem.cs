using System;
using UnityEngine;
using Unity;


public class InputSystem : Singleton<InputSystem>
{
    public static EventHandler MouseInteractEvent;

    private InputSystem_Actions _inputActions;

    private void Awake()
    {
        base.Awake();
        _inputActions = new InputSystem_Actions();
        _inputActions.Enable();
        _inputActions.Player.MouseInteract.performed += OnMouseInteractPerformed;


    }

    private void Update()
    {
        float cameraMoveValue = _inputActions.Player.CameraMove.ReadValue<float>();
        if (cameraMoveValue != 0)
        {
            Debug.Log("Camera Move Input Detected: " + cameraMoveValue);
            CameraViewSystem.Instance.MoveCamera(cameraMoveValue);
        }
    }

    private void OnMouseInteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        MouseInteractEvent?.Invoke(this, EventArgs.Empty);
    }

}
