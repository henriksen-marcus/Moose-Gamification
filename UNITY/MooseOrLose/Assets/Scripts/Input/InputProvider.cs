using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputProvider
{
    private InputActions _input = new();
    
    public event Action<InputAction.CallbackContext> Selected
    {
        add => _input.FlyingCamera.Select.performed += value;
        remove => _input.FlyingCamera.Select.performed -= value;
    }

    public Vector2 MovementInput() => _input.FlyingCamera.KeyboardMove.ReadValue<Vector2>();
    
}