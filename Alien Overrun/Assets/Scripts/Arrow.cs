/**
 * Description: Controls arrows that are shot by towers.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class Arrow : MonoBehaviour
{
	[SerializeField] private GameObject gfx = null;
	[SerializeField] private int damage = 20;

	private Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>( );

		Assert.IsNotNull( rb );
		Assert.IsNotNull( gfx );
	}

	void Update ()
	{
		if ( rb && rb.velocity != Vector3.zero )
			transform.rotation = Quaternion.LookRotation( rb.velocity );
	}

	private void OnCollisionEnter( Collision collision )
	{
		//if ( collision.transform.CompareTag( "Environment" ) )
			//IamStuck( collision.transform );

		if ( collision.transform.CompareTag( "Enemy" ) )
		{
			HP hp = collision.gameObject.GetComponent<HP>( );
			if ( hp != null )
				hp.ChangeHP( -damage );

			IamStuck( collision.transform );
			Utilities.DrawDebugText( collision.transform.position + Vector3.up, damage.ToString( ) );

			return;
		}

		IamStuck( collision.transform );
	}

	private void IamStuck ( Transform collider )
	{
		//GetComponent<Rigidbody>( ).velocity = Vector3.zero;
		//GetComponent<Rigidbody>( ).isKinematic = true;
		Destroy( GetComponent<Rigidbody>( ) );
		Destroy( GetComponent<BoxCollider>( ) );

		transform.SetParent( collider );
		Destroy( gameObject, 5f ); // TODO: Make it disappear over few seconds?
	}
}
