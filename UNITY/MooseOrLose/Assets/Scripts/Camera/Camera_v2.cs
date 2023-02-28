using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Camera_v2 : MonoBehaviour
{
    private InputProvider _inputProvider;
    private Camera _mainCamera;
    
    private float _acceleration = 0.1f;
    private float _maxKeyboardVelocity = 1f;
    private float _maxMouseVelocity = 10f;
    
    /* How much the camera will brake each tick.
     * Lower = more braking. */
    private float _brakeFactor = 0.98f;
    private float _defaultBrakeFactor = 0.97f;
    private float _highBrakeFactor = 0.8f;

    private float _dragSpeed = 55f;

    private Vector3 _velocity = Vector3.zero;

    private bool _isPressingMouse;
    
    /* If the mouse drag movement has been used, wait
     * for the velocity given by the mouse to decrease
     * naturally before clamping it again. This is to
     * enable snappy dragging of the mouse, like dragging
     * your finger fast when swiping on your phone. */
    private bool _waitForVelocity;
    
    GameObject _gameObjectInfo;
    //public GameObject InfoBar;

    private void OnEnable()
    {
        _inputProvider = new InputProvider();
        _inputProvider.SelectPerformed += Select;
        _inputProvider.MousePressed += x => _isPressingMouse = true;
        
        _inputProvider.Enable();
    }

    private void OnDisable()
    {
        _inputProvider.SelectPerformed -= Select;
        
        _inputProvider.Disable();
    }

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        var transform1 = _mainCamera.transform;
        transform1.rotation = Quaternion.Euler(45f, 0f, 0f);
        transform1.position = new Vector3(0f, 150f, -160f);
    }
    
    void Start()
    {
        //_gameObjectInfo = GameObject.Find("UI_Canvas").transform.Find("GameObjectInfo").gameObject;
        //_targetPos = transform.position;
    }
    
    void Update()
    {
        if (_isPressingMouse) MouseDrag();
        else Move();
    }

    /* When the mouse is released. */
    private void Select(InputAction.CallbackContext context)
    {
        // Raycast etc..
        
        /* When 'performed' has finished we know that the mouse has been
         * released because the input mode is set to 'on release'. */
        _isPressingMouse = false;
    }

    /* Handles movement using keyboard keys. */
    private void Move()
    {
        var newVelocity = _velocity + _inputProvider.MovementInput() * _acceleration;

        // Make sure there is a smooth transition from mouse to keyboard
        if (_waitForVelocity) _velocity = newVelocity;
        else if (_velocity.magnitude <= _maxKeyboardVelocity)
        {
            _velocity = newVelocity;
            _waitForVelocity = false;
        }
        else _velocity = Vector3.ClampMagnitude(newVelocity, _maxKeyboardVelocity);
        
        _brakeFactor = _defaultBrakeFactor;

        ApplyMovement();
    }
    
    /* Handles movement using mouse dragging on the screen. */
    private void MouseDrag()
    {
        // Using new input system to get mouse delta
        Vector3 delta = _inputProvider.MouseDelta();
        var clickWorldPos = _mainCamera.ScreenToViewportPoint(delta);
        var moveOffset = new Vector3(-clickWorldPos.x, 0, -clickWorldPos.y) * _dragSpeed;

        // Dragging fast gives an extra boost
        _velocity += moveOffset * (delta.magnitude * 0.45f * 0.02f);
        _velocity = Vector3.ClampMagnitude(_velocity, _maxMouseVelocity);
        _waitForVelocity = true;

        // Brake when holding mouse in a static position
        _brakeFactor = delta.magnitude < 1f ? _highBrakeFactor : _defaultBrakeFactor;

        ApplyMovement();
    }

    void ApplyMovement()
    {
        transform.position += _velocity * (Time.deltaTime * 45f);
        _velocity *= _brakeFactor;
    }
}
