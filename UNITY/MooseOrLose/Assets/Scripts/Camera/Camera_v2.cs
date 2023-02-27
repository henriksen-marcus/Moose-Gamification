using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Camera_v2 : MonoBehaviour
{
    private InputProvider _inputProvider;
    private Camera _mainCamera;
    
    private float _moveSpeed = 0.4f;
    
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _targetPos = Vector3.zero;
    
    GameObject _gameObjectInfo;
    //public GameObject InfoBar;

    private void OnEnable()
    {
        _inputProvider = new InputProvider();
        _inputProvider.SelectPerformed += Select;
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
    }

    // Start is called before the first frame update
    void Start()
    {
        //_gameObjectInfo = GameObject.Find("UI_Canvas").transform.Find("GameObjectInfo").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /* When a player clicks on something. */
    private void Select(InputAction.CallbackContext context)
    {
        // Raycast etc..
        print("Something happened!");
    }

    private void Move()
    {
        _targetPos += _inputProvider.MovementInput() * _moveSpeed;
        //transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _velocity, 0.5f);
        transform.position = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * 10);
    }
}
