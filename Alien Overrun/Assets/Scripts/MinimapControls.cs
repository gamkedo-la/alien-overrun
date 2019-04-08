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
    public float panSpeed;
    private Vector3 camFirstPos;
    private Vector3 camCurrentPos;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        cameraSlider.value = startZoom;
        camFirstPos = minimapCamera.transform.position;
        camCurrentPos = minimapCamera.transform.position;
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
        if(EventSystem.current.currentSelectedGameObject.name == "Left")
        {
            minimapCamera.transform.position = new Vector3(camCurrentPos.x - 5, camCurrentPos.y, camCurrentPos.z);
            camCurrentPos = minimapCamera.transform.position;
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Right")
        {
            minimapCamera.transform.position = new Vector3(camCurrentPos.x + 5, camCurrentPos.y, camCurrentPos.z);
            camCurrentPos = minimapCamera.transform.position;
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Top")
        {
            minimapCamera.transform.position = new Vector3(camCurrentPos.x, camCurrentPos.y, camCurrentPos.z + 5);
            camCurrentPos = minimapCamera.transform.position;
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Bottom")
        {
            minimapCamera.transform.position = new Vector3(camCurrentPos.x, camCurrentPos.y, camCurrentPos.z - 5);
            camCurrentPos = minimapCamera.transform.position;
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Reset")
        {
            minimapCamera.transform.position = camFirstPos;
            camCurrentPos = minimapCamera.transform.position;
        }
    }
}
