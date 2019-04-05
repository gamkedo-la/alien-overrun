/**
 * Description: Manages buildings, provides functionality for other scripts.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
	public static BuildingManager Instance { get; private set; }

	public List<Building> Buildings { get; private set; }

	[SerializeField] private bool building = false;
	public bool Building { get { return building; } set { building = value; } }

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

	public void ShowZones( bool show )
	{
		foreach ( var building in Buildings )
			building.ShowPlaceZone( show );
	}

	public Vector3 GetNearestCoreOrZero( Vector3 position )
	{
		Vector3 returnPos = Vector3.zero;

		float distance = 100000000;
		foreach ( var building in Buildings )
			if ( building.BuildingType == BuildingType.Core &&
				 Vector3.Distance( building.transform.position, position) < distance )
			{
				distance = Vector3.Distance( building.transform.position, position );
				returnPos = building.transform.position;
			}

		return returnPos;
	}

	public bool CanPlaceBuiding( Building buildingToPlace )
	{
		foreach ( var building in Buildings )
		{
			if ( building.AreWeCloseEnough( buildingToPlace ) )
				return true;
		}

		return false;
	}

	public int CoresLeft( )
	{
		return Buildings.Select( b => b ).Where( b => b.BuildingType == BuildingType.Core ).Count( );
	}
}
