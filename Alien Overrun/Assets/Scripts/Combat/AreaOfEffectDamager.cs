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

		if ( collision.transform.CompareTag( "Enemy" ) )
		{
			HP hp = collision.gameObject.GetComponent<HP>( );
			if ( hp != null )
				hp.ChangeHP( -damage );

			Utilities.DrawDebugText( collision.transform.position + Vector3.up, damage.ToString( ) );
		}
	}

	private void OnTriggerEnter( Collider other )
	{
		//Debug.Log( "Trigger damage with: " + other.gameObject.name );

		if ( other.transform.CompareTag( "Enemy" ) )
		{
			HP hp = other.gameObject.GetComponent<HP>( );
			if ( hp != null )
				hp.ChangeHP( -damage );

			Utilities.DrawDebugText( other.transform.position + Vector3.up, damage.ToString( ) );
		}
	}
}
