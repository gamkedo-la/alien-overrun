/**
 * Description: Manages buildings, provides functionality for other scripts.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager : AbstractListManager
{
	public static BuildingManager Instance { get; private set; }

	[SerializeField] private bool building = false;
	public bool Building { get { return building; } set { building = value; } }

	private protected override void Awake( )
	{
		base.Awake( );

		if ( Instance != null && Instance != this )
			Destroy( gameObject );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }

	public void ShowZones( bool show )
	{
		foreach ( Building building in ItemsList )
			building.ShowPlaceZone( show );
	}

	public Vector3 GetNearestCoreCastleOrZero( Vector3 position )
	{
		Vector3 returnPos = Vector3.zero;

		float distance = 100000000;
		foreach ( Building building in ItemsList )
			if ( ( building.BuildingType == BuildingType.Core || building.BuildingType == BuildingType.Castle ) &&
				 Vector3.Distance( building.transform.position, position) < distance )
			{
				distance = Vector3.Distance( building.transform.position, position );
				returnPos = building.transform.position;
			}

		return returnPos;
	}

	public Building GetNearestCoreCastleOrNull( Vector3 position )
	{
		Building buildingToReturn = null;

		float distance = 100000000;
		foreach ( Building building in ItemsList )
			if ( ( building.BuildingType == BuildingType.Core || building.BuildingType == BuildingType.Castle ) &&
				 Vector3.Distance( building.transform.position, position ) < distance )
			{
				distance = Vector3.Distance( building.transform.position, position );
				buildingToReturn = building;
			}

		return buildingToReturn;
	}

	public bool CanPlaceBuiding( Building buildingToPlace )
	{
		foreach ( Building building in ItemsList )
		{
			if ( building.AreWeCloseEnough( buildingToPlace ) )
				return true;
		}

		return false;
	}

	public int CastlesLeft( )
	{
		return ItemsList.Cast<Building>( ).Select( b => b ).Where( b => b.BuildingType == BuildingType.Castle ).Count( );
	}
}
