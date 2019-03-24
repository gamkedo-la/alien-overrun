/**
 * Description: Manages buildings, provides functionality for other scripts.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
	public static BuildingManager Instance { get; private set; }

	public List<Building> Buildings { get; private set; }

	private void Awake( )
	{
		Buildings = new List<Building>( );

		if ( Instance != null && Instance != this )
			Destroy( gameObject );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }

	public void AddBuilding( Building building )
	{
		Buildings.Add( building );
	}

	public void RemoveBuilding( Building building )
	{
		Buildings.Remove( building );
	}
}
