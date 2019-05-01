/**
 * Description: Abstract list manager class to be used with listable items.
 * Authors: SpadXIII
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

abstract public class AbstractListManager : MonoBehaviour
{
	public List<AbstractListableItem> ItemsList { get; private set; }

	private protected virtual void Awake( )
	{
		ItemsList = new List<AbstractListableItem>( );
    }

    public IEnumerable<GameObject> GetGameObjects( )
    {
        return ItemsList.Select( b => b.gameObject );
    }

	public void AddItem( AbstractListableItem item )
	{
		ItemsList.Add( item );
	}

	public void RemoveItem( AbstractListableItem item )
	{
		ItemsList.Remove( item );
	}
}
