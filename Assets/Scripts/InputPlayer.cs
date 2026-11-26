using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayer : MonoBehaviour
{
    [SerializeField] private InputAction inputAction;
    
    public static Action<Vector3> OnMove;
    public static Action<float> OnZoom;
    public static Action OnChoose;
    public static Action OnCancel;
    
    public void Move(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>());
    }
    
    public void Zoom(InputAction.CallbackContext context)
    {
        OnZoom?.Invoke(context.ReadValue<float>());
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
