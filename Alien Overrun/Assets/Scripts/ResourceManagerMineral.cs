/**
 * Description: Manages player's crystal resource nodes.
 * Authors: Kornel, SpadXIII
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

public class ResourceManagerMineral : AbstractListManager
{
	public static ResourceManagerMineral Instance { get; private set; }

	private int onMap = 0;

	private protected override void Awake( )
	{
		base.Awake( );

		if ( Instance != null && Instance != this )
			Destroy( gameObject );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }

	public void Add( int amount )
	{
		onMap += amount;
		CancelInvoke( "Display" );
		Invoke( "Display", 1f );
	}

	private void Display()
	{
		UnityEngine.Debug.Log( "Minerals on map: " + onMap );
	}
}