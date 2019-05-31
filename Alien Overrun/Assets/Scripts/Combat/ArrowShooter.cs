/**
 * Description: Shooter component of Towers - shoots arrows.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class ArrowShooter : MonoBehaviour
{
	[SerializeField] private Transform spawnPoint = null;
	[SerializeField] private GameObject projectile = null;
	[SerializeField] private GameObject muzzleFlashPrefab = null;
	[Space]
	[SerializeField] private float reloadTime = 2f;
	[SerializeField] private int ammoCost = 0;
	[Space]
	[SerializeField, Range(0,300)] private float shootingForceToDistance = 3.7f;
	[SerializeField] private float upAngleCorrection = -90f;
	[SerializeField] private float arrowTimePerUnit = 0.09f;
	[SerializeField] private float verticalSpread = 5f;
	[SerializeField] private float horizontalSpread = 5f;

	private float timeToNextShot = 0f;
	private float fireDistance = 15f;
	private Transform target = null;

	void Start( )
	{
		Assert.IsNotNull( projectile );
		Assert.IsNotNull( spawnPoint );
	}

	void Update( )
	{
		timeToNextShot -= Time.deltaTime;

		TryToShoot( );
	}

	public void OnNewOponenet( Transform oponent )
	{
		target = oponent;
		//Debug.Log( name + " has new oponent: " + oponent.name );
	}

	public void OnOponenetLost( )
	{
		target = null;
		//Debug.Log( name + " lost oponent" );
	}

	private void TryToShoot( )
	{
		if ( target && timeToNextShot <= 0 && HasAmmo( ) )
		{
			timeToNextShot = reloadTime;
			Shoot( );
		}
	}

	private bool HasAmmo()
	{
		return ResourceManager.Instance.CheckResources( ResourceType.Minerals, ammoCost );
	}

	private void Shoot( )
	{
		if ( !target )
		{
			Debug.LogError( "Tried shooting but enemy = null" );
			return;
		}

		ResourceManager.Instance.UseResources( ResourceType.Minerals, ammoCost );

		// Position
		Vector3 standardTower = transform.position;
		standardTower.y = 0;
		Vector3 standardEnemy = target.transform.position;
		standardEnemy.y = 0;

		// Angle, based on enemy distance
		float upAngle = 45f;
		float currentDistanceToTarget = Vector3.Distance( transform.position, target.transform.position );

		float arrowTravelTime = currentDistanceToTarget * arrowTimePerUnit;
		//float enemyWillMoveUnitsInThatTime = currentEnemy.GetComponent<Enemy>( ).GetCurrentSpeed( ) * arrowTravelTime;
		float enemyWillMoveUnitsInThatTime = 0;

		Vector3 dirFromEnemy = ( standardTower - standardEnemy ).normalized;
		Vector3 projectedEnemyPos = target.transform.position + ( dirFromEnemy * enemyWillMoveUnitsInThatTime );

		float percentOfDistance = Vector3.Distance( transform.position, projectedEnemyPos ) / fireDistance;
		float distanceCorrection = ( 1 - percentOfDistance ) * upAngleCorrection;
		upAngle += distanceCorrection;

		// Direction and Force
		standardEnemy = projectedEnemyPos;
		standardEnemy.y = 0;

		Vector3 dir = ( standardEnemy - standardTower ).normalized;
		dir = Quaternion.AngleAxis( upAngle, Vector3.Cross( Vector3.down, dir ) ) * dir;

		// Some randomness
		dir = Quaternion.AngleAxis( Random.Range( -horizontalSpread, horizontalSpread ), Vector3.Cross( Vector3.down, dir ) ) * dir;
		dir = Quaternion.AngleAxis( Random.Range( -verticalSpread, verticalSpread ), Vector3.Cross( Vector3.right, dir ) ) * dir;

		Debug.DrawRay( spawnPoint.position, dir * 20, new Color( 1f, 0f, 0f, 0.9f ), 1f );

		// and blast off!
		Vector3 force = dir * ( shootingForceToDistance * fireDistance );
		var go = Instantiate( projectile, spawnPoint.position, Quaternion.identity );
		go.GetComponent<Rigidbody>( ).AddForce( force * PauseGame.ForceScale );

		// muzzle flash particle effect
		if (muzzleFlashPrefab) {
			var muzzleFlash = Instantiate( muzzleFlashPrefab, spawnPoint.position, Quaternion.LookRotation(dir));
		}
	}
}
