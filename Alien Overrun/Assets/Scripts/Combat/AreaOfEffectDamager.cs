/**
 * Description: Simulates an expanding explosion.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent( typeof( SphereCollider ) )]
public class AreaOfEffectDamager : MonoBehaviour
{
	[SerializeField] private int damage = 12;
	[SerializeField] private float startSize = 1f;
	[SerializeField] private float endSize = 5f;
	[SerializeField] private float time = 2f;
	[SerializeField] private DamageType damageType = DamageType.Fire;

	private float currnetSize = 0;
	private float currnetTime = 0;

	void Start ()
	{
		//Assert.IsNotNull(  );
		currnetSize = startSize;
		transform.localScale = Vector3.one * currnetSize;
	}

	void Update ()
	{
		currnetTime += Time.deltaTime;
		currnetSize = startSize + ( ( endSize - startSize ) * ( currnetTime / time ) );
		transform.localScale = Vector3.one * currnetSize;

		if ( currnetTime >= time )
			Destroy( gameObject );
	}

	private void OnCollisionEnter( Collision collision )
	{
		//Debug.Log( "Collision damage with: " + collision.gameObject.name );

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
	}

	private void OnTriggerEnter( Collider other )
	{
		//Debug.Log( "Trigger damage with: " + other.gameObject.name );

		if ( other.transform.CompareTag( Tags.Enemy ) )
		{
			HP hp = other.gameObject.GetComponent<HP>( );
			if ( hp != null )
			{
				float damg = damage * Interactions.GetMultiplier( damageType, hp.Resistance );
				hp.ChangeHP( -damg );
				Utilities.DrawDebugText( other.transform.position + Vector3.up, damg.ToString( ) );
			}
		}
	}
}
