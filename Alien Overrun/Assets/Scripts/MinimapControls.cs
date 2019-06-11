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
    public GameObject minimapCameraObj;
    public float camZoom;
    public float zoom = 1f;
    //Toggle
    private CanvasGroup cg;
    //Pan
    private bool cameraMoving;
    private Vector3 mouseLastPos;
    private Vector3 camFirstPos;
    public float sensitivity;
    //Reset
    //private int clickCount = 0;

    private void Start()
    {
        camFirstPos = minimapCamera.transform.position;
        sensitivity = 0.002f;
        cameraMoving = false;
        cg = GetComponent<CanvasGroup>();
        cameraSlider.value = camZoom;

    }
    void Update()
    {
        ControlCameraZoom();
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMinimap();
        }
        if (Input.GetMouseButtonDown(0))
        {
            mouseLastPos = Input.mousePosition;
        }
        if (cameraMoving == true)
        {
			Vector3 mousePos = Input.mousePosition;
            Vector3 vectorResult = mouseLastPos - mousePos;
            minimapCameraObj.transform.Translate(-vectorResult.x * sensitivity, -vectorResult.y * sensitivity, -vectorResult.z * sensitivity);
            if(minimapCamera.transform.position.z > 100 || minimapCamera.transform.position.x < -100 || minimapCamera.transform.position.x > 200 || minimapCamera.transform.position.z < -130)
            {
                ResetMinimapCam();
            }
        }
    }

	void ControlCameraZoom()
    {
        minimapCamera.orthographicSize = cameraSlider.value;
    }

	public void ControlCameraZoomIn( )
	{
		cameraSlider.value -= zoom;
		minimapCamera.orthographicSize = cameraSlider.value;
	}

	public void ControlCameraZoomOut( )
	{
		cameraSlider.value += zoom;
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
        cameraMoving = true;

		SelectionBoxScript.instance.enabled = false;
	}
    public void StopMovingCamera()
    {
        cameraMoving = false;

		SelectionBoxScript.instance.enabled = true;
		SelectionBoxScript.instance.ResetSelectBox();
	}
    public void ResetMinimapCam()
    {
        minimapCamera.transform.position = camFirstPos;
    }

}