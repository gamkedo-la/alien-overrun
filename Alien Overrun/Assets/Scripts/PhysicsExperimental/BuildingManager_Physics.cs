/**
 * Description: Manages buildings, provides functionality for other scripts.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager_Physics : MonoBehaviour
{
	public static BuildingManager_Physics Instance { get; private set; }

	public List<Building_Physics> Buildings { get; private set; }

	[SerializeField] private bool building = false;
	public bool Building { get { return building; } set { building = value; } }

	private void Awake()
	{
		Buildings = new List<Building_Physics>();

		if (Instance != null && Instance != this)
			Destroy(gameObject);
		else
			Instance = this;
	}

	private void OnDestroy() { if (this == Instance) { Instance = null; } }

	public void AddBuilding(Building_Physics building)
	{
		Buildings.Add(building);
	}

	public void RemoveBuilding(Building_Physics building)
	{
		Buildings.Remove(building);
	}

	public void ShowZones(bool show)
	{
		foreach (var building in Buildings)
			building.ShowPlaceZone(show);
	}

	public Vector3 GetNearestCoreOrZero(Vector3 position)
	{
		Vector3 returnPos = Vector3.zero;

		float distance = 100000000;
		foreach (var building in Buildings)
			if (building.BuildingType == BuildingPhysicsType.Core &&
				 Vector3.Distance(building.transform.position, position) < distance)
			{
				distance = Vector3.Distance(building.transform.position, position);
				returnPos = building.transform.position;
			}

		return returnPos;
	}

	public bool CanPlaceBuiding(Building_Physics buildingToPlace)
	{
		foreach (var building in Buildings)
		{
			if (building.AreWeCloseEnough(buildingToPlace))
				return true;
		}

		return false;
	}

	public int CoresLeft()
	{
		return Buildings.Select(b => b).Where(b => b.BuildingType == BuildingPhysicsType.Core).Count();
	}
}
