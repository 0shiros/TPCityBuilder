using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayer : MonoBehaviour
{
    public static Action<Vector3> OnMove;
    public static Action<float> OnZoom;
    public static Action<Vector2> OnMousePosition;
    public static Action OnChoose;
    public static Action OnCancel;
    
    public void Move(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>());
    }
    
    public void Zoom(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnZoom?.Invoke(context.ReadValue<float>());
        }
    }
    
    public void MousePosition(InputAction.CallbackContext context)
    {
        OnMousePosition?.Invoke(context.ReadValue<Vector2>());
    }

    public void Choose(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnChoose?.Invoke();
        }
    }

    public void Cancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnCancel?.Invoke();
        }
    }
}
