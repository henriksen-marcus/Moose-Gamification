using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
enum CameraMode
{
    Normal,
    Orbit
}

enum CursorMode
{
    Normal,
    Point,
    Grab
}

public class Camera_v2 : MonoBehaviour
{
    public static Camera_v2 Instance;
    private InputProvider _inputProvider;
    private GameObject _rotationPoint;
    private GameObject _cameraSocket;
    private Camera _mainCamera;

    /* The velocity of the _rotationPoint the camera is attached to. */
    private Vector2 _velocity = Vector2.zero;
    private float _acceleration = 0.1f;
    private float _maxKeyboardVelocity = 3f;
    private float _maxMouseVelocity = 12f;
    private float _rotationAcceleration = 0.19f;
    private Vector2 _rotationVelocity;
    private float _maxRotationKeyboardVelocity = 2.5f;
    private float _maxRotationMouseVelocity = 12f;
    private float _lerpSpeed = 4f;

    /* If the mouse drag movement has been used, wait
     * for the velocity given by the mouse to decrease
     * naturally before clamping it again. This is to
     * enable snappy dragging of the mouse, like dragging
     * your finger fast when swiping on your phone. */
    private bool _waitForVelocity;
    private bool _waitForRotationalVelocity;

    private float _cameraDistance = 150f;
    private float _cameraTargetDistance = 150f;
    private float _lastCameraDistance = 150f;
    private float _minCameraDistance = 20f;
    private float _maxCameraDistance = 400f;

    private float _cameraAngle = 45f;
    private float _defaultCameraAngle = 45f;
    private float _minCamAngle = 15f;
    private float _maxCamAngle = 89.9f;

    /* How far the camera distance changes per scroll. */
    private float _scrollDistance = 32f;

    /* How much the camera movement will brake each tick.
     * Lower = more braking. */
    private float _brakeFactor = 0.97f;
    private float _defaultBrakeFactor = 0.97f;
    private float _highBrakeFactor = 0.81f;
    private float _rotationBrakeFactor = 0.958f;

    private float _dragSpeed = 70f;
    private float _defaultDragSpeed = 70f;

    /* The location of the ground directly beneath the camera. */
    private Vector3 _lastGroundPoint = Vector3.zero;
    private Vector3 _lastForwardGroundPoint = Vector3.zero;

    private float _lastGroundDistance;
    private float _lastForwardGroundDistance;

    /* The camera cannot move outside this xz area. (+-) */
    private Vector2 _cameraBounds;
    /* How far outside the map's border the camera can move. */
    private float _cameraBoundDistance = 30f;

    /* How far away from the ground directly beneath us before
     * a 'collision' triggers. */
    private float _defaultGroundCollisionRange = 15f;
    private float _groundCollisionRange = 15f;
    /* How much mouse delta until the click counts as a drag. */
    private float _mouseDeltaForDrag = 1f;

    /* How far away horizontally the player can click to select a clickable object. */
    private float _selectDistance = 15f;

    private bool _hasUsedMouseDrag;
    private bool _isPressingLMB;
    private bool _isPressingRMB;

    private ClickableObject _selectedObject;
    private bool _isObjectSelected;

    public GameObject _gameObjectInfo;
    private ObjectInfo _infoBar;

    /* The forest we are currently orbiting. */
    private GameObject _selectedForest;
    private Vector3 _selectedForestPosition;

    private CameraMode _cameraMode = CameraMode.Normal;

    private Timer _clickTimer;
    private float _doubleClickTime = 0.3f;
    bool _isPointerOverGameObject;

    /* Layermasks */
    private int _mapLm;
    private int _movableObjectsLm;
    private int _forestLm;
    private int _UILm;


    [SerializeField] public Texture2D MouseNormalTexture;
    [SerializeField] public Texture2D MousePointTexture;
    [SerializeField] public Texture2D MouseGrabTexture;
    

    private void OnEnable()
    {
        _inputProvider = new InputProvider();

        _inputProvider.SelectPressed += SetPressingLMB;
        _inputProvider.SelectPerformed += Select;

        _inputProvider.RightclickPressed += SetPressingRMB;
        _inputProvider.RightclickPerformed += Rightclick;

        _inputProvider.Pause += Pause;
        _inputProvider.Back += Back;

        _inputProvider.Enable();
    }

    private void OnDisable()
    {
        _inputProvider.SelectPressed -= SetPressingLMB;
        _inputProvider.SelectPerformed -= Select;

        _inputProvider.RightclickPressed -= SetPressingRMB;
        _inputProvider.RightclickPerformed -= Rightclick;

        _inputProvider.Pause -= Pause;
        _inputProvider.Back -= Back;

        _inputProvider.Disable();
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        _rotationPoint = GameObject.Find("RotationPoint");
        _cameraSocket = GameObject.Find("CameraSocket");
        _mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
        _infoBar = _gameObjectInfo.GetComponent<ObjectInfo>();
        _clickTimer = new Timer();

        var camTransform = _mainCamera.transform;
        camTransform.rotation = Quaternion.Euler(45f, 0f, 0f);
        camTransform.position = new Vector3(0f, 150f, -160f);

        _mapLm = 1 << LayerMask.NameToLayer("Map");
        _movableObjectsLm = 1 << LayerMask.NameToLayer("Moveable Objects");
        _forestLm = 1 << LayerMask.NameToLayer("Forest");
        _UILm = LayerMask.NameToLayer("UI");
        _isPointerOverGameObject = false;

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
        
        /*s = GameObject.Find("Sphere (1)").gameObject;
        o = s.AddComponent<Outline>();*/
    }

    void Update()
    {
        switch (_cameraMode)
        {
            case CameraMode.Normal:
                if (_isPressingLMB) MouseDrag();
                else if (_isPressingRMB) MouseRotationalDrag();
                else Move();
                _cameraAngle = Mathf.Lerp(_cameraAngle, _defaultCameraAngle, Time.deltaTime * 6f);
                break;
            case CameraMode.Orbit:
                if (_isPressingLMB || _isPressingRMB) MouseRotationalDrag();
                else OrbitMove();
                _rotationPoint.transform.position = Vector3.Lerp(_rotationPoint.transform.position,
                    _selectedForestPosition, Time.deltaTime * 5f);
                break;
        }
        UpdateCameraDistance();
        CameraUpdate();
    }
    
    private void FixedUpdate()
    {
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
                position = Input.mousePosition
            };
            
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            _isPointerOverGameObject = false;
            if (results.Count <= 0) return;
            foreach (var t in results.Where(t => t.gameObject.layer == _UILm))
                _isPointerOverGameObject = true;
        }
        
    }

    private void LateUpdate()
    {
        HoverCheck();
    }
    
    private void Pause(InputAction.CallbackContext context)
    {
        
    }
    
    public void Deselect()
    {
        if (_selectedObject)
        {
            _selectedObject.SetOutlineSelected(false);
        }
        _selectedObject = null;
    }

    private void Back(InputAction.CallbackContext context)
    {
        if (_cameraMode == CameraMode.Orbit)
        {
            _cameraTargetDistance = 
                Mathf.Abs(_lastCameraDistance - _cameraDistance) < 8f ? 
                    _lastCameraDistance + 12f : _lastCameraDistance;
            _cameraMode = CameraMode.Normal;
            _rotationVelocity.y = 0f;
            // Make sure we can still zoom the same amount after looking at a forest
            _groundCollisionRange = _defaultGroundCollisionRange + _rotationPoint.transform.position.y;
        }
    }

    public void SetMovementEnabled(bool enabled) => _dragSpeed = enabled ? _defaultDragSpeed : 0f; 
    
    private void SetCursor(CursorMode newCursor)
    {
        switch (newCursor)
        {
            case CursorMode.Normal:
                Cursor.SetCursor(MouseNormalTexture, Vector2.zero, UnityEngine.CursorMode.ForceSoftware);
                break;
            case CursorMode.Point:
                Cursor.SetCursor(MousePointTexture, Vector2.zero, UnityEngine.CursorMode.ForceSoftware);
                break;
            case CursorMode.Grab:
                Cursor.SetCursor(MouseGrabTexture, Vector2.zero, UnityEngine.CursorMode.ForceSoftware);
                break;
        }
    }
    
    /* Checks if a clickable object is under the cursor,
     * and if so outlines that object. */
    private void HoverCheck()
    {
        if (_isPressingLMB) return;
        
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _mapLm) && !_isPointerOverGameObject)
        {
            /* If we want mouse cursor to change on hover over a forest here,
             * we can make a layermask including both map and forest and check
             * each hit object's layer. If forest => cursor = pointer. */
            
            var closestObject = GetSphereOverlap(hit.point);
            if (!closestObject) return;
            var clickComponent = closestObject.GetComponent<ClickableObject>();
            if (clickComponent)
            {
                // This is the white outline when hovering over a clickable object
                clickComponent.ToggleOutline(true);
            }
        }
    }
    
    /* Returns the closest movable object to the given position. */
    private GameObject GetSphereOverlap(Vector3 position)
    {
        var hits = Physics.OverlapSphere(position, _selectDistance, _movableObjectsLm);
        var minDistance = float.MaxValue;
        GameObject closestObject = null;
        foreach (var obj in hits)
        {
            var distance = Vector3.Distance(position, obj.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                closestObject = obj.gameObject;
            }
        }
        return closestObject;
    }

    /* In this function we simulate a camera spring arm. */
    private void CameraUpdate()
    {
        var rotPointTransform = _rotationPoint.transform;
        var cameraSocketTransform = _cameraSocket.transform;
        
        // Set the socket location
        var rotPointPos = rotPointTransform.position;
        _cameraSocket.transform.position =
            rotPointPos - rotPointTransform.forward * _cameraDistance;
        _cameraSocket.transform.RotateAround(_rotationPoint.transform.position, _rotationPoint.transform.right, _cameraAngle);
        
        _mainCamera.transform.position = _cameraSocket.transform.position;
        
        //Debug.DrawLine(rotPointTransform.position, cameraSocketTransform.position, Color.red, 0.1f);

        var direction = rotPointPos - cameraSocketTransform.position;
        var rotation = Quaternion.LookRotation(direction);
        _mainCamera.transform.rotation = rotation;
    }

    /* When the mouse is released. */
    private void Select(InputAction.CallbackContext context)
    {
        /* When 'performed' has finished we know that the mouse has been
         * released because the input mode is set to 'on release'. */
        _isPressingLMB = false;

        /* We don't want to click when the user releases the mouse button
         * after having dragged on the screen. That would be annoying. */
        if (!_hasUsedMouseDrag && !_isPointerOverGameObject)
        {
            var time = _clickTimer.GetTime();
            if (time <= _doubleClickTime && time != 0f) // Double click
                ForestRaycast();
            else
                ObjectRaycast();
            
            _clickTimer.Start();
        }
        
        _hasUsedMouseDrag = false;
    }

    private void Rightclick(InputAction.CallbackContext context)
    {
        _isPressingRMB = false;
    }
    
    /* Raycast for clickable objects. */
    private void ObjectRaycast()
    {
        // Raycast for info bar when clicking on objects
        Vector3 mousePos = Mouse.current.position.ReadValue();
        ;
        var ray = _mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _mapLm) && !_isPointerOverGameObject)
        {
            var closestObject = GetSphereOverlap(hit.point);
            if (!closestObject) return;

            var clickComponent = closestObject.GetComponent<ClickableObject>();
            if (!clickComponent) return;

            _gameObjectInfo.SetActive(true);
            _infoBar.SpawnInfobar(clickComponent.GetClickInfo());

            Deselect();
            _selectedObject = clickComponent;
            _selectedObject.SetOutlineSelected(true);
        }
        else
        {
            _gameObjectInfo.SetActive(false);
            Deselect();
        }
    }

    private void ForestRaycast()
    {
        // Raycast for info bar when clicking on objects
        Vector3 mousePos = Mouse.current.position.ReadValue();
        var ray = _mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _forestLm) && !_isPointerOverGameObject)
        {
            var forest = hit.transform.GetComponent<Forest>();
            if (forest)
            {
                var forestPos = forest.transform.position;
                _selectedForestPosition = new Vector3(forestPos.x, forestPos.y, forestPos.z);
                _velocity = Vector3.zero;
                _lastCameraDistance = _cameraTargetDistance;
                _cameraTargetDistance = Mathf.Max(_minCameraDistance, 25f);
                
                // Find out if object is on right of left side of screen
                var screenPosition = _mainCamera.WorldToViewportPoint(_selectedForestPosition);
                var rotAmount = Mathf.Lerp(0.1f, 2.9f, Mathf.Abs(screenPosition.x - 0.5f) + 0.5f);
                _rotationVelocity.x += screenPosition.x > 0.5f ? rotAmount : -rotAmount;
                _cameraMode = CameraMode.Orbit;
            }
        }
    }
    
    /* Handles movement using keyboard keys. */
    private void Move()
    {
        var newVelocity = _velocity + _inputProvider.MovementInput() * _acceleration;

        // Make sure there is a smooth transition from mouse to keyboard
        if (_waitForVelocity)
        {
            if (_velocity.magnitude <= _maxKeyboardVelocity)
            {
                _velocity = newVelocity;
                _waitForVelocity = false;
            }
            else _velocity = newVelocity;
        }
        else _velocity = Vector3.ClampMagnitude(newVelocity, _maxKeyboardVelocity);
        

        _rotationVelocity.x += _inputProvider.RotationInput() * _rotationAcceleration;
        _brakeFactor = _defaultBrakeFactor;
        ApplyMovement();
    }
    
    private void OrbitMove()
    {
        var keyboardInput = _inputProvider.MovementInput();
        var rotationInput = _inputProvider.RotationInput();
        
        var xInput = Mathf.Abs(keyboardInput.x) > Mathf.Abs(rotationInput) ? -keyboardInput.x : rotationInput;
        var newXVelocity = xInput * _rotationAcceleration;
        _rotationVelocity.y += keyboardInput.y * _rotationAcceleration * 0.4f;
        
        // Make sure there is a smooth transition from mouse to keyboard
        if (_waitForRotationalVelocity)
        {
            if (_rotationVelocity.x <= _maxRotationKeyboardVelocity)
            {
                _rotationVelocity.x += newXVelocity;
                _waitForVelocity = false;
            }
            else _rotationVelocity.x += newXVelocity;
        }
        else _rotationVelocity.x = Mathf.Clamp(_rotationVelocity.x, -_maxRotationKeyboardVelocity, _maxRotationKeyboardVelocity);
        
        ApplyMovement();
    }
    
    /* Handles movement using mouse dragging on the screen. */
    private void MouseDrag()
    {
        // Using new input system to get mouse delta
        Vector3 delta = _inputProvider.MouseDelta();
        //var deltaMag = delta.magnitude;

        var temp = Mathf.Abs(delta.x) + Mathf.Abs(delta.y);
        if (temp > _mouseDeltaForDrag) _hasUsedMouseDrag = true;

        var clickWorldPos = _mainCamera.ScreenToViewportPoint(delta);
        var moveOffset = new Vector2(-clickWorldPos.x, -clickWorldPos.y) * _dragSpeed;

        // Dragging fast gives an extra boost
        _velocity += moveOffset * (delta.magnitude * 0.5f * 0.02f);
        _velocity = Vector2.ClampMagnitude(_velocity, _maxMouseVelocity);
        _waitForVelocity = true;

        // Brake when holding mouse in a static position
        _brakeFactor = temp < 1f ? _highBrakeFactor : _defaultBrakeFactor;

        ApplyMovement();
    }

    private void MouseRotationalDrag()
    {
        // Using new input system to get mouse delta
        Vector3 delta = _inputProvider.MouseDelta();
        var xMagnitude = Mathf.Abs(delta.x);
        var yMagnitude = Mathf.Abs(delta.y);
        
        _hasUsedMouseDrag = xMagnitude > _mouseDeltaForDrag || yMagnitude > _mouseDeltaForDrag;
        
        var clickWorldPos = _mainCamera.ScreenToViewportPoint(delta);
        var xRotationOffset = clickWorldPos.x * _dragSpeed;
        var yRotationOffset = _cameraMode == CameraMode.Orbit ? clickWorldPos.y * _dragSpeed : 0f;

        // Dragging fast gives an extra boost
        _rotationVelocity.x += xRotationOffset * (xMagnitude * 0.5f * 0.05f);
        _rotationVelocity.y += -yRotationOffset * (yMagnitude * 0.5f * 0.015f);
        _rotationVelocity.x = Mathf.Clamp(_rotationVelocity.x, -_maxRotationMouseVelocity, _maxRotationMouseVelocity);
        _rotationVelocity.y = Mathf.Clamp(_rotationVelocity.y, -_maxRotationMouseVelocity, _maxRotationMouseVelocity);
        _waitForRotationalVelocity = true;

        ApplyMovement();
    }

    /* Get the current camera distance based on input and 
     * collision in front of, under and the max camera distance
     * settings. Uses trigonometry with ray casting. */
    private void UpdateCameraDistance()
    {
        var camTransform = _mainCamera.transform;

        // Find position on ground beneath + a buffer
        var ray = new Ray(camTransform.position, Vector3.down);
        if (Physics.Raycast(ray, out var hit, _mapLm))
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
                * (_groundCollisionRange * 1.3f);
            _lastForwardGroundDistance = Vector3.Distance(rotPointPos, _lastForwardGroundPoint);
        }

        // Limit the minimum camera distance based on what is closest
        var localMinDistance = Mathf.Max(_lastForwardGroundDistance, _lastGroundDistance);
        localMinDistance = Mathf.Max(localMinDistance, _minCameraDistance);
        
        // Smooth interpolation when changing height with scroll
        _cameraTargetDistance = Mathf.Clamp(_cameraTargetDistance + GetScroll(), localMinDistance, _maxCameraDistance);
        _cameraDistance = Mathf.Lerp(_cameraDistance, _cameraTargetDistance, Time.deltaTime * _lerpSpeed);
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
        _rotationPoint.transform.Translate(new Vector3(_velocity.x, 0f, _velocity.y) * (Time.deltaTime * 45f));
        _rotationPoint.transform.Rotate(new Vector3(0f, _rotationVelocity.x, 0f) * (Time.deltaTime * 45f));
        _cameraAngle = Mathf.Clamp(_cameraAngle + _rotationVelocity.y, _minCamAngle, _maxCamAngle);
        _velocity *= _brakeFactor;
        _rotationVelocity *= _rotationBrakeFactor;

        // Constrain camera to bounds
        var pos = _rotationPoint.transform.position;
        pos.x = Mathf.Clamp(pos.x, -_cameraBounds.x, _cameraBounds.x);
        pos.z = Mathf.Clamp(pos.z, -_cameraBounds.y, _cameraBounds.y);
        _rotationPoint.transform.position = pos;
    }

    private void SetPressingLMB(InputAction.CallbackContext context) => _isPressingLMB = true;
    
    private void SetPressingRMB(InputAction.CallbackContext context) => _isPressingRMB = true;
}
