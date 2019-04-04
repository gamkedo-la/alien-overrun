/**
 * Description: Faces objects towards the camera but retains their X rotation.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class Billboard : MonoBehaviour
{
	private Camera cam = null;

	void Start ()
	{
		cam = Camera.main;
		Assert.IsNotNull( cam );
	}

	void LateUpdate( )
	{
		transform.LookAt
		(
			transform.position + cam.transform.rotation * Vector3.back,
			cam.transform.rotation * Vector3.down
		);

		Vector3 angles = transform.eulerAngles;
		angles.x = 0;
		transform.eulerAngles = angles;
	}
}
