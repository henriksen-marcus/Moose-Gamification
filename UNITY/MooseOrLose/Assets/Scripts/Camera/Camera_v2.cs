using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camera_v2 : MonoBehaviour
{
    private InputProvider _inputProvider;
    private GameObject _rotationPoint;
    private GameObject _cameraSocket;
    private Camera _mainCamera;
    
    private float _acceleration = 0.1f;
    private float _maxKeyboardVelocity = 1f;
    private float _maxMouseVelocity = 12f;
    private float _rotationSpeed = 0.4f;
    private float _rotationVelocity;
    private float _lerpSpeed = 4f;

    private float _cameraDistance = 150f;
    private float _cameraTargetDistance = 150f;
    private float _minCameraDistance = 10f;
    private float _maxCameraDistance = 400f;
    
    /* How far the camera distance changed per scroll. */
    private float _scrollDistance = 35f;
    
    /* How much the camera movement will brake each tick.
     * Lower = more braking. */
    private float _brakeFactor = 0.98f;
    private float _defaultBrakeFactor = 0.97f;
    private float _highBrakeFactor = 0.8f;

    private float _dragSpeed = 55f;

    private Vector3 _velocity = Vector3.zero;
    
    /* The location of the ground directly beneath the camera. */
    private Vector3 _lastGroundPoint = Vector3.zero;
    private Vector3 _lastForwardGroundPoint = Vector3.zero;

    private float _lastGroundDistance;
    private float _lastForwardGroundDistance;

    /* The camera cannot move outside this xz area. (+-) */
    private Vector2 _cameraBounds;
    /* How far outside the map's border the camera can move. */
    private float _cameraBoundDistance = 40f;
    
    /* How far away from the ground directly beneath us before
     * a 'collision' triggers. */
    private float _groundCollisionRange = 20f;
    /* How much mouse delta until the click counts as a drag. */
    private float _mouseDeltaForDrag = 0.5f;

    /* How far away the player can click to select a clickable object. */
    private float _selectDistance = 20f;
    
    private bool _hasUsedMouseDrag;
    private bool _isPressingMouse;
    
    /* If the mouse drag movement has been used, wait
     * for the velocity given by the mouse to decrease
     * naturally before clamping it again. This is to
     * enable snappy dragging of the mouse, like dragging
     * your finger fast when swiping on your phone. */
    private bool _waitForVelocity;
    
    public GameObject _gameObjectInfo;
    private ObjectInfo _infoBar;

    //private GameObject _sphereMesh = null;

    private void OnEnable()
    {
        _inputProvider = new InputProvider();
        _inputProvider.SelectPerformed += Select;
        _inputProvider.MousePressed += SetPressingMouse;
        
        _inputProvider.Enable();
    }

    private void OnDisable()
    {
        _inputProvider.SelectPerformed -= Select;
        _inputProvider.MousePressed -= SetPressingMouse;
        
        _inputProvider.Disable();
    }

    private void Awake()
    {
        _rotationPoint = GameObject.Find("RotationPoint");
        _cameraSocket = GameObject.Find("CameraSocket");
        _mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
        _infoBar = _gameObjectInfo.GetComponent<ObjectInfo>();
        
        var camTransform = _mainCamera.transform;
        camTransform.rotation = Quaternion.Euler(45f, 0f, 0f);
        camTransform.position = new Vector3(0f, 150f, -160f);
        //_mainCamera.enabled = false;
    }
    
    void Start()
    {
        // Default values
        _cameraBounds.x = _cameraBoundDistance * 2;
        _cameraBounds.y = _cameraBoundDistance * 2;
        
        // NOTE: This is unstable code.
        var mesh = GameObject.Find("KirkesdalenMesh");
        if (mesh)
        {
            var meshFilter = mesh.GetComponent<MeshFilter>();
            if (meshFilter)
            {
                var meshSize = meshFilter.mesh.bounds.extents;
                meshSize = Vector3.Scale(meshSize, mesh.transform.localScale);
                var meshPos = mesh.transform.position;
                // Set the 'look at' position of the camera to the center of the map
                _rotationPoint.transform.position = meshPos;
                _cameraBounds.x = meshPos.x + meshSize.x + _cameraBoundDistance;
                _cameraBounds.y = meshPos.y + meshSize.y + _cameraBoundDistance;
            } 
        }
        
        //_gameObjectInfo = GameObject.Find("Screen_Canvas").transform.Find("GameObjectInfo").gameObject;
    }
    
    void Update()
    {
        if (_isPressingMouse) MouseDrag();
        else Move();
        CameraUpdate();
    }

    /* In this function we simulate a camera spring arm. */
    private void CameraUpdate()
    {
        var rotPointTransform = _rotationPoint.transform;
        var cameraSocketTransform = _cameraSocket.transform;
        
        // Set the socket location
        _cameraSocket.transform.position =
            rotPointTransform.position - rotPointTransform.forward * _cameraDistance;
        _cameraSocket.transform.RotateAround(_rotationPoint.transform.position, _rotationPoint.transform.right, 45f);
        
        _mainCamera.transform.position = _cameraSocket.transform.position;
        
        //Debug.DrawLine(rotPointTransform.position, cameraSocketTransform.position, Color.red, 0.1f);

        var direction = rotPointTransform.position - cameraSocketTransform.position;
        var rotation = Quaternion.LookRotation(direction);
        _mainCamera.transform.rotation = rotation;
    }

    /* When the mouse is released. */
    private void Select(InputAction.CallbackContext context)
    {
        /* When 'performed' has finished we know that the mouse has been
         * released because the input mode is set to 'on release'. */
        _isPressingMouse = false;

        /* We don't want to click when the user releases the mouse button
         * after having dragged on the screen. That would be annoying. */
        if (!_hasUsedMouseDrag)
        {
            // Raycast for info bar when clicking on objects
            Vector3 mousePos = Mouse.current.position.ReadValue();
            var layerMask = 1 << LayerMask.NameToLayer("Map");
            Ray ray = _mainCamera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
            {
                // TODO: Make this use a layermask as well
                var hits = Physics.OverlapSphere(hit.point, _selectDistance);
                var minDistance = float.MaxValue;
                GameObject closestObject = null;
                
                foreach (var obj in hits) {
                    var distance = Vector3.Distance(hit.point, obj.transform.position);
                    if (distance < minDistance) {
                        minDistance = distance;
                        closestObject = obj.gameObject;
                    }
                }
                
                if (closestObject != null)
                {
                    var clickComponent = closestObject.GetComponent<ClickableObject>();
                    if (clickComponent)
                    {
                        _gameObjectInfo.SetActive(true);
                        _infoBar.SpawnInfobar(clickComponent.GetClickInfo());
                    }
                }
                // Debug
                /*if (_sphereMesh)
                {
                    _sphereMesh.transform.position = mouseWorldPos;
                }
                else
                {
                    _sphereMesh = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    _sphereMesh.transform.position = mouseWorldPos;
                    _sphereMesh.transform.localScale = new Vector3(10f, 10f, 10f);
                    Color sphereColor = Color.blue;
                    Material sphereMaterial = new Material(Shader.Find("Standard"));
                    sphereMaterial.color = sphereColor;
                    _sphereMesh.GetComponent<MeshRenderer>().material = sphereMaterial;
                }*/
            } 
            else _gameObjectInfo.SetActive(false);
        }
        _hasUsedMouseDrag = false;
    }
    
    /* Handles movement using keyboard keys. */
    private void Move()
    {
        var inputVector = Get3DMovement();
        var newVelocity = _velocity + inputVector * _acceleration;

        // Make sure there is a smooth transition from mouse to keyboard
        if (_waitForVelocity) _velocity = newVelocity;
        else if (_velocity.magnitude <= _maxKeyboardVelocity)
        {
            _velocity = newVelocity;
            _waitForVelocity = false;
        }
        else _velocity = Vector3.ClampMagnitude(newVelocity, _maxKeyboardVelocity);

        UpdateCameraDistance();

        _rotationVelocity += _inputProvider.RotationInput() * _rotationSpeed;
        _brakeFactor = _defaultBrakeFactor;
        ApplyMovement();
    }
    
    /* Handles movement using mouse dragging on the screen. */
    private void MouseDrag()
    {
        // Using new input system to get mouse delta
        Vector3 delta = _inputProvider.MouseDelta();
        var deltaMag = delta.magnitude;

        var temp = Mathf.Abs(delta.x) + Mathf.Abs(delta.y);
        
        _hasUsedMouseDrag = temp > _mouseDeltaForDrag;
        
        var clickWorldPos = _mainCamera.ScreenToViewportPoint(delta);
        var moveOffset = new Vector3(-clickWorldPos.x, 0, -clickWorldPos.y) * _dragSpeed;

        // Dragging fast gives an extra boost
        _velocity += moveOffset * (delta.magnitude * 0.45f * 0.02f);
        _velocity = Vector3.ClampMagnitude(_velocity, _maxMouseVelocity);
        _waitForVelocity = true;

        // Brake when holding mouse in a static position
        _brakeFactor = deltaMag < 1f ? _highBrakeFactor : _defaultBrakeFactor;

        ApplyMovement();
    }

    //private Vector3 linePosition;
    
    /* Get the current camera distance based on input and 
     * collision in front of, under and the max camera distance
     * settings. Uses trigonometry with ray casting. */
    private void UpdateCameraDistance()
    {
        var camTransform = _mainCamera.transform;

        // Find position on ground beneath + a buffer
        var ray = new Ray(camTransform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            _lastGroundPoint = hit.point;
            _lastGroundPoint.y += _groundCollisionRange;
            _lastGroundDistance = Vector3.Distance(camTransform.position, _lastGroundPoint);
        }

        // Find position on ground in front + a buffer
        ray = new Ray(camTransform.position, camTransform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            var rotPointPos = _rotationPoint.transform.position;
            
            _lastForwardGroundPoint = hit.point;
            _lastForwardGroundPoint -= 
                (rotPointPos - camTransform.position).normalized 
                * (_groundCollisionRange * 1.4f);
            _lastForwardGroundDistance = Vector3.Distance(rotPointPos, _lastForwardGroundPoint);
        }

        // Limit the minimum camera distance based on what is closest
        var localMinDistance = Mathf.Max(_lastForwardGroundDistance, _lastGroundDistance);
        localMinDistance = Mathf.Max(localMinDistance, _minCameraDistance);

        // Smooth interpolation when changing height with scroll
        _cameraTargetDistance = Mathf.Clamp(_cameraTargetDistance + GetScroll(), localMinDistance, _maxCameraDistance);
        _cameraDistance = Mathf.Lerp(_cameraDistance, _cameraTargetDistance, Time.deltaTime * _lerpSpeed);
        
        
        
        /*print("actual: " + _cameraDistance);
        Debug.DrawLine(camTransform.position + new Vector3(5f, 0f, 0f), camTransform.position + new Vector3(-5f, 0f, 0f));
        Debug.DrawLine(camTransform.position + new Vector3(0f, 5f, 0f), camTransform.position + new Vector3(0f, -5f, 0f));
        
        Debug.DrawLine(_currentGroundPoint + new Vector3(5f, 0f, 0f), _currentGroundPoint + new Vector3(-5f, 0f, 0f), Color.magenta);
        Debug.DrawLine(_currentGroundPoint + new Vector3(0f, 0f, 5f), _currentGroundPoint + new Vector3(0f, 0f, -5f), Color.magenta);
        
        Debug.DrawLine(_currentForwardGroundPoint + new Vector3(5f, 0f, 0f), _currentForwardGroundPoint + new Vector3(-5f, 0f, 0f), Color.yellow, 0.01f, true);
        Debug.DrawLine(_currentForwardGroundPoint + new Vector3(0f, 5f, 0f), _currentForwardGroundPoint + new Vector3(0f, -5f, 0f), Color.yellow, 0.01f, true);*/

        /*Debug.DrawLine(linePosition + new Vector3(5f, 0f, 0f), linePosition + new Vector3(-5f, 0f, 0f));
        Debug.DrawLine(linePosition + new Vector3(0f, 5f, 0f), linePosition + new Vector3(0f, -5f, 0f));*/

        return;
        // Calculate direction vector from camera to rotation point
        /*var direction = (_rotationPoint.transform.position - camTransform.position).normalized;

        // Calculate distance between camera position and rotation point in the y-axis direction
        float distanceY = _rotationPoint.transform.position.y - camTransform.position.y;

        // Calculate ratio between the y-distance and the direction vector's y-component
        float ratio = distanceY / direction.y;

        // Calculate the point on the line at the desired y-value
        linePosition = camTransform.position + direction * ratio;
        linePosition.y = _currentGroundPoint.y;

        // Transform the position by the rotation quaternion
        linePosition = camTransform.rotation * (linePosition - _rotationPoint.transform.position) + _rotationPoint.transform.position;

        return;
        var a = Vector3.Distance(camTransform.position, _currentGroundPoint);
        var b = Vector3.Distance(_currentGroundPoint, _rotationPoint.transform.position);
        var max_x = Mathf.Sqrt(a * a + b * b); // Pythagorean theorem
        
        /* The max_x variable gives us the maximum allowed cam length
         * based on collision on the ground beneath us. #1#
        
        // Smooth interpolation when changing height with scroll.
        _cameraTargetDistance = Mathf.Clamp(_cameraTargetDistance + GetScroll(), _minCameraDistance, _maxCameraDistance);
        _cameraDistance = Mathf.Lerp(_cameraDistance, _cameraTargetDistance, Time.deltaTime * _lerpSpeed);*/
    }

    /* Flips the movement plane from xy to xz. */
    private Vector3 Get3DMovement()
    {
        Vector2 input = _inputProvider.MovementInput();
        return new Vector3(input.x, 0f, input.y);
    }

    private float GetScroll() => -_inputProvider.ScrollInput() / 120 * _scrollDistance;
    
    private void ApplyMovement()
    {
        _rotationPoint.transform.Translate(_velocity * (Time.deltaTime * 45f));
        _rotationPoint.transform.Rotate(new Vector3(0f, _rotationVelocity, 0f) * (Time.deltaTime * 45f));
        _velocity *= _brakeFactor;
        _rotationVelocity *= _highBrakeFactor;

        // Constrain camera to bounds
        var pos = _rotationPoint.transform.position;
        pos.x = Mathf.Clamp(pos.x, -_cameraBounds.x, _cameraBounds.x);
        pos.z = Mathf.Clamp(pos.z, -_cameraBounds.y, _cameraBounds.y);
        _rotationPoint.transform.position = pos;
    }

    private void SetPressingMouse(InputAction.CallbackContext context) => _isPressingMouse = true;
}
