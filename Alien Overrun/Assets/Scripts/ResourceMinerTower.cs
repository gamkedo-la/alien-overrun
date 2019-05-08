/**
 * Description: Main class of a resource miner building. Responsible for core behaviors.
 * Authors: SpadXIII
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMinerTower : Building
{
	protected private override void SetOponentListManager( )
	{
		OponentFinder oponentFinder = gameObject.GetComponent<OponentFinder>( );
		oponentFinder.SetOponentListManager( ResourceManager.Instance );
	}
}
