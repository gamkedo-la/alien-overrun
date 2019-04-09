/**
 * Description: Tank for testing.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class Tank : MonoBehaviour
{
	[SerializeField] private Rigidbody rb = null;
	[SerializeField] private Transform cannonPivot = null;
	[SerializeField] private Transform turret = null;
	[SerializeField] private Vector3 turretRotation = Vector3.zero;
	[SerializeField] private float speed = 10f;
	[SerializeField] private float turnSpeed = 180f;
	[SerializeField] private float turretTurnSpeed = 120f;
	[SerializeField] private float turretElevationDeltaMax = 20f;

	private float forward = 0;
	private float turn = 0;

	void Start ()
	{
		Assert.IsNotNull( rb );
		Assert.IsNotNull( cannonPivot );
		Assert.IsNotNull( turret );
	}

	void Update ()
	{
		forward = 0;

		if ( Input.GetKey( KeyCode.W ) )
			forward += 1.0f;
		if ( Input.GetKey( KeyCode.S ) )
			forward -= 1.0f;

		if ( Input.GetKey( KeyCode.A ) )
			turn -= turnSpeed * Time.deltaTime;
		if ( Input.GetKey( KeyCode.D ) )
			turn += turnSpeed * Time.deltaTime;

		if ( Input.GetKey( KeyCode.UpArrow ) )
			turretRotation.x -= turretTurnSpeed * Time.deltaTime * 0.3f;
		if ( Input.GetKey( KeyCode.DownArrow ) )
			turretRotation.x += turretTurnSpeed * Time.deltaTime * 0.3f;

		turretRotation.x = Mathf.Clamp( turretRotation.x, -turretElevationDeltaMax, turretElevationDeltaMax );

		if ( Input.GetKey( KeyCode.LeftArrow ) )
			turretRotation.y -= turretTurnSpeed * Time.deltaTime;
		if ( Input.GetKey( KeyCode.RightArrow ) )
			turretRotation.y += turretTurnSpeed * Time.deltaTime;

		cannonPivot.localRotation = Quaternion.Euler( turretRotation.x, 0, 0 );
		turret.localRotation = Quaternion.Euler( 0, turretRotation.y, 0 );
	}

	void FixedUpdate( )
	{
		Vector3 newPos = rb.transform.forward * forward * speed * Time.fixedDeltaTime;

		rb.MoveRotation( Quaternion.Euler( 0, turn, 0 ) );
		rb.MovePosition( rb.transform.position + newPos );
	}
}
