/**
 * Description: Main class of all buildings. Responsible for core behaviors.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public enum BuildingPhysicsType
{
	Core,
	Tower,
	Wall
}

public class Building_Physics : MonoBehaviour
{
	public GameObject Indicator { get { return indicator; } private set { indicator = value; } }
	[SerializeField] private GameObject indicator = null;

	public BuildingPhysicsType BuildingType { get { return buildingType; } private set { buildingType = value; } }
	[SerializeField] private BuildingPhysicsType buildingType = BuildingPhysicsType.Tower;

	public string BuildingName { get { return buildingName; } private set { buildingName = value; } }
	[SerializeField] private string buildingName = "Building";

	public int BuildCost { get { return buildCost; } private set { buildCost = value; } }
	[SerializeField] private int buildCost = 100;

	public float PlaceDistance { get { return placeDistance; } private set { placeDistance = value; } }
	[SerializeField] private float placeDistance = 6;

	public float BuildTime { get { return buildTime; } private set { buildTime = value; } }
	[SerializeField] private float buildTime = 1.0f;

	[SerializeField] private Collider buildingTrigger = null;
	[SerializeField] private Collider[] buildingMesh = null;
	[SerializeField] private Behaviour[] toEnableOnBuild = null;
	[SerializeField] private bool enableOnStart = false;

	private int collisions = 0;

	void Start()
	{
		Assert.IsNotNull(indicator);
		Assert.IsNotNull(buildingTrigger);
		Assert.AreNotEqual(buildingMesh.Length, 0);
		//Assert.IsNotNull(toEnableOnBuild);
		//Assert.AreNotEqual(toEnableOnBuild.Length, 0);

		indicator.SetActive(false);

		foreach (var m in buildingMesh)
			m.enabled = false;

		if (enableOnStart)
			EnableBuilding();
	}

	void OnDisable()
	{
		if (BuildingManager_Physics.Instance)
			BuildingManager_Physics.Instance.RemoveBuilding(this);
	}

	void OnMouseDown()
	{
		//GetComponentInChildren<MeshRenderer>( ).material.color = Random.ColorHSV( );
		Debug.Log($"Clicked: {name}");
	}


	void OnMouseEnter()
	{
		//The Range is shown when selected (locked, not hover)
		/*
		if ( !LevelManager.Instance.Paused )
			indicator.ShowRange( true );
		*/
		indicator.SetActive(true);
	}

	void OnMouseExit()
	{
		//The Range gets hidden when deselected (locked, not hover)
		//indicator.HideRange( );
		indicator.SetActive(false);
	}

	void OnTriggerEnter(Collider other)
	{
		if (CollidesWithTags(other.gameObject))
			collisions++;
	}

	void OnTriggerExit(Collider other)
	{
		if (CollidesWithTags(other.gameObject))
			collisions--;
	}

	public void EnableBuilding()
	{
		BuildingManager_Physics.Instance.AddBuilding(this);
		//buildingTrigger.isTrigger = false;

		foreach (var item in toEnableOnBuild)
			item.enabled = true;

		foreach (var m in buildingMesh)
			m.enabled = true;
	}

	public bool CanBePaced() => collisions == 0;
	
	public bool AreWeCloseEnough(Building_Physics anotherBuilding)
		=> Vector3.Distance(transform.position, anotherBuilding.gameObject.transform.position) <= placeDistance;

	private bool CollidesWithTags(GameObject gameObject)
	{
		return gameObject.CompareTag(Tags.Building) ||
			   gameObject.CompareTag(Tags.Enemy) ||
			   gameObject.CompareTag(Tags.Environment);
	}
}
