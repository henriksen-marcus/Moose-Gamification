using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICam : MonoBehaviour
{
    private Vector3 cameraPosition;
    private Vector3 mousePosition;
    private Vector3 mouseDelta;
    private float zoomAmount = 200;
    private float zoomSpeed = 10f;
    private float zoomMin = 0f;
    private float zoomMax = 2000f;

    void Start()
    {
        transform.position = new Vector3(0, zoomAmount, -10);
        transform.eulerAngles = new Vector3(50, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    void Update()
    {
        // Drag camera with mouse
        if (Input.GetMouseButtonDown(0))
        {
            cameraPosition = transform.position;
            mousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            mouseDelta = Input.mousePosition - mousePosition;
            mouseDelta = new Vector3(-mouseDelta.x, 0, -mouseDelta.y);
            transform.position = cameraPosition + mouseDelta;
        }

        // Zoom camera with mouse scroll wheel
        zoomAmount -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        zoomAmount = Mathf.Clamp(zoomAmount, zoomMin, zoomMax);
        transform.position = new Vector3(transform.position.x, zoomAmount, transform.position.z);
    }
}
