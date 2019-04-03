/**
 * Description: Beam shooter component of Towers - shoots a beam.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class BeamShooter : MonoBehaviour
{
	[SerializeField] private Transform shootPoint = null;
	[SerializeField] private float reloadTime = 2f;
	[SerializeField] private float shootDuration = 1f;
	[SerializeField] private float damage = 5f;
	[SerializeField] private Color shotColor = Color.red;
	[SerializeField] private DamageType damageType = DamageType.Magical;

	private Transform target = null;
	private float timeToNextShot = 0;

	void Start( )
	{
		Assert.IsNotNull( shootPoint );
	}

	void Update( )
	{
		timeToNextShot -= Time.deltaTime;

		if ( target && timeToNextShot <= 0 )
		{
			timeToNextShot = reloadTime;

			Utilities.DrawLine( shootPoint.position, target.position, shotColor, 0.1f, shootDuration );

			HP hp = target.GetComponent<HP>( );
			float damg = damage * Interactions.GetMultiplier( damageType, hp.Resistance );

			hp.ChangeHP( -damg );
			Utilities.DrawDebugText( target.position + Vector3.up, damg.ToString( ) );
		}
	}

	public void OnNewOponenet ( Transform oponent )
	{
		target = oponent;
		//Debug.Log( name + " has new oponent: " + oponent.name );
	}

	public void OnOponenetLost ( )
	{
		target = null;
		//Debug.Log( name + " lost oponent" );
	}
}
