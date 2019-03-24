/**
 * Description: Controls in-game camera (zoom, pan, etc.).
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class CamControl : MonoBehaviour
{
	[SerializeField] private Transform cam = null;
	[SerializeField] private float panSpeed = 8f;
	[SerializeField] private float zoomSpeed = 200f;
	[SerializeField] private float camRotation = -120f;
	[SerializeField] private float mouseScreenBoundOffset = 50f;
	[SerializeField] private bool useMouse = true;

	void Start ()
	{
		Assert.IsNotNull( cam );

		#if UNITY_EDITOR
			useMouse = false;
		#endif
	}

	void Update ()
	{
		if ( Input.GetKey( KeyCode.D ) || ( Input.mousePosition.x >= Screen.width - mouseScreenBoundOffset && useMouse ) )
		{
			Vector3 translation = Quaternion.Euler( 0, camRotation, 0 ) * Vector3.right * panSpeed * Time.deltaTime;
			transform.Translate( translation );
		}
		else if ( Input.GetKey( KeyCode.A ) || ( Input.mousePosition.x <= mouseScreenBoundOffset && useMouse ) )
		{
			Vector3 translation = Quaternion.Euler( 0, camRotation, 0 ) * Vector3.left * panSpeed * Time.deltaTime;
			transform.Translate( translation );
		}

		if ( Input.GetKey( KeyCode.W ) || ( Input.mousePosition.y >= Screen.height - mouseScreenBoundOffset && useMouse ) )
		{
			Vector3 translation = Quaternion.Euler( 0, camRotation, 0 ) * Vector3.forward * panSpeed * Time.deltaTime;
			transform.Translate( translation );
		}
		else if ( Input.GetKey( KeyCode.S ) || ( Input.mousePosition.y <= mouseScreenBoundOffset && useMouse ) )
		{
			Vector3 translation = Quaternion.Euler( 0, camRotation, 0 ) * Vector3.back * panSpeed * Time.deltaTime;
			transform.Translate( translation );
		}

		if ( Input.GetAxis( "Mouse ScrollWheel" ) > 0f )
		{
			cam.position += Vector3.down * zoomSpeed * Input.GetAxis( "Mouse ScrollWheel" ) * Time.deltaTime;
		}
		else if ( Input.GetAxis( "Mouse ScrollWheel" ) < 0f )
		{
			cam.position += Vector3.down * zoomSpeed * Input.GetAxis( "Mouse ScrollWheel" ) * Time.deltaTime;
		}
	}
}
