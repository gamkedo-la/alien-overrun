/**
 * Description: Shoots tank projectiles.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class TankShooter : MonoBehaviour
{
	[SerializeField] private GameObject projctile = null;
	[SerializeField] private float shootingForce = 100f;
	[SerializeField] private float dalayBetweenShots = 0.3f;
	[SerializeField] private float reloadTime = 1.5f;
	[SerializeField] private int magSize = 5;

	private float timeForNextShot = 0f;
	private int magSizeCurrent = 0;

	void Start ()
	{
		Assert.IsNotNull( projctile );

		magSizeCurrent = magSize;
	}

	void Update ()
	{
		timeForNextShot -= Time.deltaTime;

		if ( timeForNextShot <= 0 && magSizeCurrent > 0 )
		{
			GameObject go = Instantiate( projctile, transform.position, Quaternion.identity );
			go.GetComponent<Rigidbody>( ).AddForce( transform.forward * shootingForce );
			magSizeCurrent--;

			if ( magSizeCurrent <= 0 )
			{
				timeForNextShot = reloadTime;
				magSizeCurrent = magSize;
			}
			else
			{
				timeForNextShot = dalayBetweenShots;
			}
		}
	}
}
