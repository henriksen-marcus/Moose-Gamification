using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputProvider
{
    private static readonly InputActions _input = new();

    public void Enable()
    {
        _input.FlyingCamera.KeyboardMove.Enable();
        _input.FlyingCamera.Select.Enable();
        _input.FlyingCamera.MouseDrag.Enable();
    }

    public void Disable()
    {
        _input.FlyingCamera.KeyboardMove.Disable();
        _input.FlyingCamera.Select.Disable();
        _input.FlyingCamera.MouseDrag.Disable();
    }
    
    public event Action<InputAction.CallbackContext> SelectPerformed
    {
        add => _input.FlyingCamera.Select.performed += value;
        remove => _input.FlyingCamera.Select.performed -= value;
    }

    public Vector3 MovementInput() => _input.FlyingCamera.KeyboardMove.ReadValue<Vector3>();
    
}