/**
 * Description: Core resource functionality.
 * Authors: SpadXIII
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

public class Resource : AbstractListableItem
{
	public int currentResources { get; private set; }

	[SerializeField] private int totalResources = 2000;
	[SerializeField] private UnityEvent onChange = null;
	[SerializeField] private UnityEvent onDeath = null;

	public void CollectResources( int amount )
	{
		currentResources -= Mathf.Abs(amount);

		ResourceManager.Instance.AddResources( ResourceType.Minerals, amount );

		onChange.Invoke( );

		if ( currentResources <= 0 )
			DestroyMe( );
	}

	private void DestroyMe( )
	{
		onDeath.Invoke( );
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
}
