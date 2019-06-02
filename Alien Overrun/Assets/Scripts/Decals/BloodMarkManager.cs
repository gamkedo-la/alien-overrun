/**
 * Description: Makes scorch marks on the ground.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class BloodMarkManager : MonoBehaviour
{
	public static BloodMarkManager Instance { get; private set; }

	[SerializeField] private Vector3 rotation = new Vector3(0,-180,0);
	[SerializeField] private ParticleSystem markParticles = null;

	private void Awake( )
	{
		if ( Instance != null && Instance != this )
			Destroy( gameObject );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }

	void Start( )
	{
		Assert.IsNotNull( markParticles );

		//InvokeRepeating( "TestSystem", 0.5f, 0.5f );
		markParticles.transform.rotation = Quaternion.LookRotation( rotation );
	}

	public void PutAt( Vector3 position, int count, float spread )
	{
		position.y = 0.0001f; // To avoid Z-fighting

		for ( int i = 0; i < count; i++ )
		{
			markParticles.transform.position = new Vector3
			(
				position.x + Random.Range( -spread, spread ),
				position.y,
				position.z + Random.Range( -spread, spread )
			);
			markParticles.Emit( 1 );
		}
	}

	[ContextMenu( "Test System" )]
	private void TestSystem( )
	{
		PutAt( Vector3.one * Random.Range( -5f, 5f ), 5, 1f );
	}
}
