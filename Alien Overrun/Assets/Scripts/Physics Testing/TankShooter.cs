/**
 * Description: Shoots tank projectiles.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class Ammunition
{
	public string Name = "";
	public GameObject Projectile = null;
	public float ShootingForce = 5000f;
}

public class TankShooter : MonoBehaviour
{
	[SerializeField] private Ammunition[] ammo = null;
	[SerializeField] private GameObject shootEffect = null;
	[SerializeField] private float dalayBetweenShots = 0.3f;
	[SerializeField] private float reloadTime = 1.5f;
	[SerializeField] private int magSize = 5;

	private Ammunition currentAmmo = null;
	private int currentAmmoID = 0;
	private float timeForNextShot = 0f;
	private int magSizeCurrent = 0;
	private bool autoShooting = false;

	void Start ()
	{
		Assert.IsNotNull( shootEffect );
		Assert.IsNotNull( ammo );
		Assert.AreNotEqual( ammo.Length, 0 );

		magSizeCurrent = magSize;
		currentAmmo = ammo[0];
		MessageService.Instance.ShowMessage( $"Current ammo: {currentAmmo.Name}" );
	}

	void Update ()
	{
		timeForNextShot -= Time.deltaTime;

		if ( Input.GetKeyDown( KeyCode.V ) )
		{
			if ( autoShooting )
			{
				autoShooting = false;
				MessageService.Instance.ShowMessage( "Manual tank shooting mode" );
			}
			else
			{
				autoShooting = true;
				MessageService.Instance.ShowMessage( "Automatic tank shooting mode" );
			}
		}

		if ( Input.GetKeyDown( KeyCode.B ) )
		{
			currentAmmoID++;
			currentAmmoID = currentAmmoID >= ammo.Length ? 0 : currentAmmoID;
			currentAmmo = ammo[currentAmmoID];

			MessageService.Instance.ShowMessage( $"Current ammo: {currentAmmo.Name}" );
		}

		if ( autoShooting )
			AutoShoot( );
		else
			ManualShoot( );
	}

	private void AutoShoot( )
	{
		if ( timeForNextShot <= 0 && magSizeCurrent > 0 )
		{
			MakeTheShot( );
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

	private void ManualShoot( )
	{
		if ( timeForNextShot <= 0 && Input.GetKey(KeyCode.Space) )
		{
			MakeTheShot( );
			timeForNextShot = dalayBetweenShots;
		}
	}

	private void MakeTheShot( )
	{
		Instantiate( shootEffect, transform.position + transform.forward * 0.3f, transform.rotation );
		GameObject go = Instantiate( ammo[currentAmmoID].Projectile, transform.position, Quaternion.identity );
		go.GetComponent<Rigidbody>( ).AddForce( transform.forward * ammo[currentAmmoID].ShootingForce * PauseGame.ForceScale );
	}
}
