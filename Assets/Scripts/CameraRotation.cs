using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float zoomSpeed = 5;
    public float rotationSpeed = 0.01f;
    public float minZoomDistance = 12;
    public float maxZoomDistance = 35;
    public float height;
    Vector3 lastMousePosition;

    Vector3 previousPosition;
    public Camera cam;

    private void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        Zoom();

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

            //cam.transform.position = new Vector3();

            cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            //cam.transform.Translate(new Vector3(0, 0, -height));

            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }
        cam.transform.position = new Vector3();
        cam.transform.Translate(new Vector3(0, 0, -height));
    }

    void Zoom() {
        float mouseScrollDelta = -Input.mouseScrollDelta.y;
        height += mouseScrollDelta * zoomSpeed;
        if (height < minZoomDistance) {
            height = minZoomDistance;
        }
        if (height > maxZoomDistance)
        {
            height = maxZoomDistance;
        }
    }
}
