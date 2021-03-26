using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;

    private float minZoom = 16;
    private float maxZoom = 6;

    private float defualtZoom = 10;

    private float currentZoom;
    
    public float zoomSpeed = 1;
    
    private float maxX = 15;
    private float minX = -15;

    private float maxY = 10;
    private float minY = -10;

    private Vector2 lastMousePosition;
    public float panSpeed = 1;
    
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera.orthographicSize = defualtZoom;
        currentZoom = defualtZoom;
        //TODO: edit bounding box for translation relative to map size.
    }

    // Update is called once per frame
    void Update()
    {
        currentZoom -= Input.mouseScrollDelta.y * zoomSpeed;
        if (currentZoom > minZoom)
            currentZoom = minZoom;
        if (currentZoom < maxZoom)
            currentZoom = maxZoom;

        mainCamera.orthographicSize = currentZoom;

        
        if (Input.GetMouseButtonDown((int) MouseButton.MiddleMouse))
        {
            lastMousePosition = Input.mousePosition;
        } 
        else if (Input.GetMouseButton((int) MouseButton.MiddleMouse))
        {
            TranslateCamera(Input.mousePosition);
        }
    }

    public void TranslateCamera(Vector2 newMousePosition)
    {
        Vector2 offset = mainCamera.ScreenToViewportPoint(lastMousePosition - newMousePosition);
        panSpeed = currentZoom * 2;
        transform.Translate(new Vector3(offset.x * (panSpeed / 9 * 16), offset.y * panSpeed), Space.World);
        lastMousePosition = newMousePosition;

        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(transform.position.x, minX, maxX);
        currentPosition.y = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = currentPosition;
    }
}
