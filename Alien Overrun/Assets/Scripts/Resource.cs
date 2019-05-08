/**
 * Description: Core resource functionality.
 * Authors: SpadXIII, Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

public class Resource : AbstractListableItem
{
	[SerializeField] private GameObject visuals = null;
	[SerializeField] private GameObject destroyEffect = null;
	public ResourceType ResourceType { get { return resourceType; } private set { resourceType = value; } }
	[SerializeField] private ResourceType resourceType = ResourceType.Minerals;
	[SerializeField] private int minResources = 100;
	[SerializeField] private int maxResources = 1000;
	[SerializeField] private float scaleFactor = 0.003f;
	[SerializeField] private float minNodeSize = 0.4f;
	[SerializeField] private UnityEvent onChange = null;
	[SerializeField] private UnityEvent onDeath = null;

	private int currentResources;

	void Start( )
	{
		Assert.IsNotNull( visuals );

		currentResources = Random.Range( minResources, maxResources + 1 );

		transform.localRotation = Quaternion.Euler
		(
			Random.Range( 0f, 359f ),
			Random.Range( 0f, 359f ),
			Random.Range( 0f, 359f )
		);

		ScaleVisuals( );
	}

	void OnEnable( )
	{
		if ( ResourceManager.Instance )
			ResourceManager.Instance.AddItem( this );
	}

	void OnDisable( )
	{
		if ( ResourceManager.Instance )
			ResourceManager.Instance.RemoveItem( this );
	}

	public void CollectResources( int amount )
	{
		currentResources -= Mathf.Abs(amount);

		ResourceManager.Instance.AddResources( resourceType, amount );
		ScaleVisuals( );

		onChange.Invoke( );

		if ( currentResources <= 0 )
			DestroyMe( );
	}

	private void DestroyMe( )
	{
		Instantiate( destroyEffect, transform.position, Quaternion.identity );
		onDeath.Invoke( );
		Destroy( gameObject );
	}

	private void ScaleVisuals( )
	{
		float newScale = ( currentResources * scaleFactor ) + minNodeSize;
		transform.localScale = new Vector3( newScale, newScale, newScale );
	}
}
