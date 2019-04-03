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
	[SerializeField] private DamageType damageType = DamageType.Physical;

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
		//if ( collision.transform.CompareTag( Tags.Environment ) )
		//IamStuck( collision.transform );

		if ( collision.transform.CompareTag( Tags.Enemy ) )
		{
			HP hp = collision.gameObject.GetComponent<HP>( );
			if ( hp != null )
			{
				float damg = damage * Interactions.GetMultiplier( damageType, hp.Resistance );
				hp.ChangeHP( -damg );
				Utilities.DrawDebugText( collision.transform.position + Vector3.up, damg.ToString( ) );
			}
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
