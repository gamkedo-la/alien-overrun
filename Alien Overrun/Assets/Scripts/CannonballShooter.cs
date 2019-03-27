/**
 * Description: Shooter component of Towers - shoots Cannonball.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class CannonballShooter : MonoBehaviour
{
	[SerializeField] private Transform spawnPoint = null;
	[SerializeField] private GameObject projectile = null;
	[SerializeField] private Transform cannon = null;
	[Space]
	[SerializeField] private float reloadTime = 2f;
	[SerializeField] private int ammoCost = 0;
	[Space]
	[SerializeField, Range(0,10)] private float shootingForcePerDistance = 0.5f;
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
		Assert.IsNotNull( cannon );
	}

	void Update( )
	{
		timeToNextShot -= Time.deltaTime;

		TryToShoot( );
	}

	public void OnNewOponenet( Transform oponent )
	{
		target = oponent;
	}

	public void OnOponenetLost( )
	{
		target = null;
	}

	private void TryToShoot( )
	{
		if ( target && timeToNextShot <= 0 && HasAmmo( ) )
		{
			timeToNextShot = reloadTime;
			Shoot( );
		}
	}

	private bool HasAmmo( )
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

		//Debug.DrawRay( spawnPoint.position, dir * 20, new Color( 1f, 0f, 0f, 0.9f ), 1f );

		float angle = Utilities.AngleBetweenVectors( new Vector2( transform.position.x, transform.position.z ), new Vector2( target.transform.position.x, target.transform.position.z ) );
		cannon.rotation = Quaternion.Euler( 0, angle + 180, 0 );

		// and blast off!
		Vector3 force = dir * ( shootingForcePerDistance * currentDistanceToTarget );
		var go = Instantiate( projectile, spawnPoint.position, Quaternion.identity );
		go.GetComponent<Rigidbody>( ).AddForce( force );
	}
}
