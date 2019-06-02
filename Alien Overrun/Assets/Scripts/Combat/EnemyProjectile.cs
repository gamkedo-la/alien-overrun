/**
 * Description: Controls arrows that are shot by towers.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class EnemyProjectile : MonoBehaviour
{
	[SerializeField] private GameObject exp = null;
	[SerializeField] private GameObject gfx = null;
	[SerializeField] private int damage = 20;

	private Rigidbody rb;

	void Start( )
	{
		rb = GetComponent<Rigidbody>( );

		Assert.IsNotNull( rb );
		Assert.IsNotNull( gfx );

		Destroy( gameObject, 10f ); // Emergency cleanup
	}

	void Update( )
	{
		/*if ( rb && rb.velocity != Vector3.zero )
			transform.rotation = Quaternion.LookRotation( rb.velocity );*/
	}
	private void OnCollisionEnter( Collision collision )
	{
		if ( collision.transform.CompareTag( Tags.Building ) )
		{
			HP hp = collision.gameObject.GetComponent<HP>( );
			if ( hp != null )
			{
				hp.ChangeHP( -damage );
				FloatingTextService.Instance.ShowFloatingText( collision.contacts[0].point, damage.ToString( ), 1, Color.white, 2f );
			}
		}

		Instantiate( exp, collision.contacts[0].point, Quaternion.identity );
		Destroy( gameObject );
	}
}
