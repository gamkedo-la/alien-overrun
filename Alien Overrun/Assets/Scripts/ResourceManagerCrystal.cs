/**
 * Description: Manages player's mineral resource nodes.
 * Authors: Kornel, SpadXIII
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

public class ResourceManagerCrystal : AbstractListManager
{
	public static ResourceManagerCrystal Instance { get; private set; }

	private protected override void Awake( )
	{
		base.Awake( );

		if ( Instance != null && Instance != this )
			Destroy( gameObject );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }
}
