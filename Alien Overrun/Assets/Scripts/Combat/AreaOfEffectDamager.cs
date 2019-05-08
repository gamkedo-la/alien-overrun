/**
 * Description: Simulates an expanding explosion.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;

[RequireComponent( typeof( SphereCollider ) )]
public class AreaOfEffectDamager : MonoBehaviour
{
	[Header("Damage")]
	[SerializeField] private int damage = 12;
	[SerializeField] private DamageType damageType = DamageType.Fire;
	[Header("Propagation")]
	[SerializeField] private float startSize = 1f;
	[SerializeField] private float endSize = 5f;
	[SerializeField] private float time = 2f;
	[Header("Force")]
	[SerializeField] private float explosionForce = 1000f;
	[Header("Other")]
	[SerializeField] private LayerMask ignoreLayer;

	private float currnetSize = 0;
	private float currnetTime = 0;

	void Start ()
	{
		currnetSize = startSize;
		transform.localScale = Vector3.one * currnetSize;
	}

	void Update ()
	{
		Grow( );
	}

	private void OnCollisionEnter( Collision collision )
	{
		//Debug.Log( "Collision damage with: " + collision.gameObject.name );
		DoDamage( collision.gameObject );
		ApplyForce( collision.gameObject );
	}

	private void OnTriggerEnter( Collider other )
	{
		//Debug.Log( "Trigger damage with: " + other.gameObject.name );
		DoDamage( other.gameObject );
		ApplyForce( other.gameObject );
	}

	private void DoDamage( GameObject other )
	{
		if ( other.transform.CompareTag( Tags.Enemy ) )
		{
			HP hp = other.gameObject.GetComponent<HP>( );
			if ( hp )
			{
				float damg = damage * Interactions.GetMultiplier( damageType, hp.Resistance );
				hp.ChangeHP( -damg );
				Utilities.DrawDebugText( other.transform.position + Vector3.up, damg.ToString( ) );
			}
		}
	}

	private void ApplyForce( GameObject other )
	{
		if ( other.transform.CompareTag( Tags.BigProjectile ) )
			return;

		Enemy enemy = other.gameObject.GetComponent<Enemy>( );
		if ( enemy )
			enemy.DisableNavMesh( );

		Rigidbody rb = other.gameObject.GetComponent<Rigidbody>( );
		if ( rb )
		{
			// Force decreases with distance (current size of the explosion)
			//float force = maxForce * ( 1.1f - transform.localScale.x / endSize );
			rb.AddExplosionForce( explosionForce, transform.position - transform.up, endSize, 1, ForceMode.Impulse );
		}
	}

	private void Grow( )
	{
		currnetTime += Time.deltaTime;
		currnetSize = startSize + ( ( endSize - startSize ) * ( currnetTime / time ) );
		transform.localScale = Vector3.one * currnetSize;

		if ( currnetTime >= time )
			Destroy( gameObject );
	}
}
