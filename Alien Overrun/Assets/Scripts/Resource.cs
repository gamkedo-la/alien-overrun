/**
 * Description: Core resource functionality.
 * Authors: SpadXIII
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class Resource : AbstractListableItem
{
	[SerializeField] private int amountOfMinerals = 2000;

	void OnEnable( )
	{
		ResourceManager.Instance.AddItem( this );
	}

	void OnDisable( )
	{
		if ( ResourceManager.Instance )
			ResourceManager.Instance.RemoveItem( this );
	}
}
