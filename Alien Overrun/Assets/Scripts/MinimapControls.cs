using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MinimapControls : MonoBehaviour
{
    //Zoom controls
    public Slider cameraSlider;
    public Camera minimapCamera;
    public float startZoom;
    //Toggle
    private CanvasGroup cg;
    //Pan
    public RectTransform minimapRect;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        cameraSlider.value = startZoom;
    }
    void Update()
    {
        ControlCameraZoom();
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMinimap();
        }
    }

    void ControlCameraZoom()
    {
        minimapCamera.orthographicSize = cameraSlider.value;
    }
    void ToggleMinimap()
    {
        if(cg.alpha == 1)
        {
            cg.alpha = 0;
            cg.interactable = false;

        } else
        {
            cg.alpha = 1;
            cg.interactable = true;
        }
    }
    public void MoveCamera()
    {
        Vector3 mousePos = Input.mousePosition;
        Rect tempRect = minimapRect.rect;
        //tempRect.y = Screen.height - tempRect.y;
        float distanceX = mousePos.x - tempRect.x;

        minimapCamera.transform.position = new Vector3(distanceX, minimapCamera.transform.position.y, minimapCamera.transform.position.z);

    }
}
