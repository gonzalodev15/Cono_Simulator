using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomWithScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 10f;
    private float maxZoom = 100f;
    private float minZoom = 60f;

    private Camera zoomCamera;
    // Start is called before the first frame update
    void Start()
    {
        zoomCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(zoomCamera.orthographic)
        {
            zoomCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        } else
        {
            zoomCamera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            zoomCamera.fieldOfView = Mathf.Clamp(zoomCamera.fieldOfView, minZoom, maxZoom);
        }

    }
}
