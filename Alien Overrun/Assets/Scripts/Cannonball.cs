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

	void Start ()
	{
		Assert.IsNotNull( explosion );
		Assert.IsNotNull( aoeDamage );
		Assert.IsNotNull( trail );
	}

	private void OnCollisionEnter( Collision collision )
	{
		Instantiate( explosion, collision.contacts[0].point, Quaternion.identity );
		Instantiate( aoeDamage, collision.contacts[0].point, Quaternion.identity );

		trail.transform.SetParent( null );
		Destroy( trail, 2f );
		Destroy( gameObject );
	}
}
