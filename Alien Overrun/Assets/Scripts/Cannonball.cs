/**
 * Description: Controls a cannonball that is shot by towers.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class Cannonball : MonoBehaviour
{
	[SerializeField] private GameObject explosion = null;
	[SerializeField] private GameObject aoeDamage = null;
	[SerializeField] private GameObject trail = null;
	[SerializeField] private int damage = 12;

	void Start ()
	{
		Assert.IsNotNull( explosion );
		Assert.IsNotNull( aoeDamage );
		Assert.IsNotNull( trail );
	}

	private void OnCollisionEnter( Collision collision )
	{
		//if ( collision.transform.CompareTag( "Environment" ) )
		//IamStuck( collision.transform );

		/*if ( collision.transform.CompareTag( "Enemy" ) )
		{
			HP hp = collision.gameObject.GetComponent<HP>( );
			if ( hp != null )
				hp.ChangeHP( -damage );

			Utilities.DrawDebugText( collision.transform.position + Vector3.up, damage.ToString( ) );
		}*/

		Instantiate( explosion, collision.contacts[0].point, Quaternion.identity );
		Instantiate( aoeDamage, collision.contacts[0].point, Quaternion.identity );

		trail.transform.SetParent( null );
		Destroy( trail, 2f );
		Destroy( gameObject );
	}
}
