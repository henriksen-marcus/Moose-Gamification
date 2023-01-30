using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    public float dragSpeed = 2;
    private Vector3 dragOrigin;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = Input.mousePosition;
            return;
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(-pos.x * dragSpeed, 0, -pos.y * dragSpeed);

            transform.Translate(move, Space.World);

            dragOrigin = Input.mousePosition;
        }

        // Scrolling

        Vector3 scroll = transform.position;
        scroll.y -= (Input.mouseScrollDelta.y * 2);
        scroll.z += (Input.mouseScrollDelta.y * 2);
        scroll.y = Mathf.Clamp(scroll.y, 0, 60);
        transform.position = scroll;

    }
}
