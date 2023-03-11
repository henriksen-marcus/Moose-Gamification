using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider
{
    private static readonly InputActions _input = new();

     // We need to enable each input method for them to work.
    public void Enable()
    {
        _input.FlyingCamera.KeyboardMove.Enable();
        _input.FlyingCamera.Select.Enable();
        _input.FlyingCamera.MouseDrag.Enable();
        _input.FlyingCamera.KeyboardRotate.Enable();
        _input.FlyingCamera.ZoomCamera.Enable();
        _input.FlyingCamera.Pause.Enable();
        _input.FlyingCamera.Back.Enable();
    }

    public void Disable()
    {
        _input.FlyingCamera.KeyboardMove.Disable();
        _input.FlyingCamera.Select.Disable();
        _input.FlyingCamera.MouseDrag.Disable();
        _input.FlyingCamera.KeyboardRotate.Disable();
        _input.FlyingCamera.ZoomCamera.Disable();
        _input.FlyingCamera.Pause.Disable();
        _input.FlyingCamera.Back.Disable();
    }
    
    public event Action<InputAction.CallbackContext> SelectPressed
    {
        add => _input.FlyingCamera.Select.started += value;
        remove => _input.FlyingCamera.Select.started -= value;
    }
    
    public event Action<InputAction.CallbackContext> SelectPerformed
    {
        add => _input.FlyingCamera.Select.performed += value;
        remove => _input.FlyingCamera.Select.performed -= value;
    }
    
    public event Action<InputAction.CallbackContext> Pause
    {
        add => _input.FlyingCamera.Pause.performed += value;
        remove => _input.FlyingCamera.Pause.performed -= value;
    }
    
    public event Action<InputAction.CallbackContext> Back
    {
        add => _input.FlyingCamera.Back.performed += value;
        remove => _input.FlyingCamera.Back.performed -= value;
    }

    public Vector2 MouseDelta() => _input.FlyingCamera.MouseDrag.ReadValue<Vector2>();

    public Vector2 MovementInput() => _input.FlyingCamera.KeyboardMove.ReadValue<Vector2>();

    public float RotationInput() => _input.FlyingCamera.KeyboardRotate.ReadValue<float>();

    public float ScrollInput() => _input.FlyingCamera.ZoomCamera.ReadValue<float>();
}