/**
 * Description: Controls in-game camera (zoom, pan, etc.).
 * Authors: Kornel
 * Copyright: � 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class CamControl : MonoBehaviour
{
	[SerializeField] private Transform cam = null;
    [SerializeField] private Vector3 cameraPositionLock = new Vector3();
    [SerializeField] private float cameraYZoomInLock = 4;
    [SerializeField] private float cameraYZoomOutLock = 16;
    [SerializeField] private float cameraXMin = -50.0f;
    [SerializeField] private float cameraXMax = 30.0f;
    [SerializeField] private float cameraZMin = -30.0f;
    [SerializeField] private float cameraZMax = 40.0f;
    [SerializeField] private float panSpeed = 8f;
    [SerializeField] private float mousePanSpeed = 0.01f;
	[SerializeField] private float zoomSpeed = 200f;
	[SerializeField] private float camRotation = -120f;
	[SerializeField] private float camRotationSpeed = 10f;
	[SerializeField] private float mouseScreenBoundOffset = 50f;
	[SerializeField] private bool useMouse = true;
    [SerializeField] private bool zoomLockOn = true;
    [SerializeField] private bool useScaledTime = false;

    private bool panning = false;
	private Vector3 oldMousePos;

    void Start ()
	{
		Assert.IsNotNull( cam );

		#if UNITY_EDITOR
			useMouse = false;
		#endif
	}

	void Update ()
	{
		Vector3 oldPos = transform.position;
		Quaternion oldRot = transform.localRotation;

		if ( Input.GetKeyDown( KeyCode.Y ) )
		{
			useScaledTime = !useScaledTime;
			MessageService.Instance.ShowMessage( "Camera deltaTime mode toggled. Scaled time: " + useScaledTime );
		}

		float time = useScaledTime ? Time.deltaTime : Time.unscaledDeltaTime;

		if ( Input.GetKey( KeyCode.D ) || ( Input.mousePosition.x >= Screen.width - mouseScreenBoundOffset && useMouse ) )
		{
			Vector3 translation = Quaternion.Euler( 0, camRotation, 0 ) * Vector3.right * panSpeed * time * ( cam.position.y * 0.1f );
            transform.Translate( translation );
		}
		else if ( Input.GetKey( KeyCode.A ) || ( Input.mousePosition.x <= mouseScreenBoundOffset && useMouse ) )
		{
			Vector3 translation = Quaternion.Euler( 0, camRotation, 0 ) * Vector3.left * panSpeed * time * ( cam.position.y * 0.1f );
			transform.Translate( translation );
		}

		if ( Input.GetKey( KeyCode.W ) || ( Input.mousePosition.y >= Screen.height - mouseScreenBoundOffset && useMouse ) )
		{
			Vector3 translation = Quaternion.Euler( 0, camRotation, 0 ) * Vector3.forward * panSpeed * time * ( cam.position.y * 0.1f );
			transform.Translate( translation );
		}
		else if ( Input.GetKey( KeyCode.S ) || ( Input.mousePosition.y <= mouseScreenBoundOffset && useMouse ) )
		{
			Vector3 translation = Quaternion.Euler( 0, camRotation, 0 ) * Vector3.back * panSpeed * time * ( cam.position.y * 0.1f );
			transform.Translate( translation );
		}

		if ( Input.GetKey( KeyCode.Q ) )
		{
			float rotation = transform.localRotation.eulerAngles.y + camRotationSpeed * time;// * ( cam.position.y * 0.1f );
			transform.localRotation = Quaternion.Euler( 0, rotation, 0 );
		}
		else if ( Input.GetKey( KeyCode.E ) )
		{
			float rotation = transform.localRotation.eulerAngles.y - camRotationSpeed * time;// * ( cam.position.y * 0.1f );
			transform.localRotation = Quaternion.Euler( 0, rotation, 0 );
		}

		if ( Input.GetAxis( "Mouse ScrollWheel" ) > 0f )
		{
			cam.position += Vector3.down * zoomSpeed * Input.GetAxis( "Mouse ScrollWheel" ) * time * ( cam.position.y * 0.1f );
            if (zoomLockOn && cam.position.y <= cameraYZoomInLock)
            {
                cameraPositionLock.x = cam.position.x;
                cameraPositionLock.z = cam.position.z;
                cameraPositionLock.y = cameraYZoomInLock;
                cam.position = cameraPositionLock;
            }
        }
		else if ( Input.GetAxis( "Mouse ScrollWheel" ) < 0f )
		{
			cam.position += Vector3.down * zoomSpeed * Input.GetAxis( "Mouse ScrollWheel" ) * time * ( cam.position.y * 0.1f );
			if (zoomLockOn && cam.position.y >= cameraYZoomOutLock)
            {
                cameraPositionLock.x = cam.position.x;
                cameraPositionLock.z = cam.position.z;
                cameraPositionLock.y = cameraYZoomOutLock;
                cam.position = cameraPositionLock;
            }
        } // end of if MouseScrollWheel < 0

		if ( Input.GetMouseButtonDown( 1 ) )
		{
			oldMousePos = Input.mousePosition;
			panning = true;
		}
		if ( Input.GetMouseButtonUp( 1 ) )
			panning = false;

		if ( panning )
		{
			Vector3 mousePos = Input.mousePosition;
			Vector3 offset = oldMousePos - mousePos;
			offset.z = offset.y;
			offset.y = 0;

			Vector3 translation = Quaternion.Euler( 0, camRotation, 0 ) * offset * mousePanSpeed * ( cam.position.y * 0.1f );
			transform.Translate( translation );

			oldMousePos = Input.mousePosition;
		}

        // Clamp position within boundaries
        Vector3 check = transform.position;
        check.x = Mathf.Clamp(check.x, cameraXMin, cameraXMax);
        check.z = Mathf.Clamp(check.z, cameraZMin, cameraZMax);
        transform.position = check;

		var hits = Physics.OverlapSphere( cam.position, 1f);
		bool gotHit = false;
		foreach ( var hit in hits )
		{
			if ( hit.gameObject.CompareTag( Tags.Environment ) )
			{
				gotHit = true;
				break;
			}
		}

		if ( gotHit )
		{
			transform.position = oldPos;
			transform.localRotation = oldRot;
		}

		cam.LookAt( transform );
	} // end of Update
} // end of CamControlClass
