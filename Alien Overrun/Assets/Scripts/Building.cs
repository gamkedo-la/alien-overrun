﻿/**
 * Description: Main class of all buildings. Responsible for core behaviors.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class Building : MonoBehaviour
{
	[SerializeField] private Indicator indicator = null;

	public string BuildingName { get { return buildingName; } private set { buildingName = value; } }
	[SerializeField] private string buildingName = "Building";

	public int BuildCost { get { return buildCost; } private set { buildCost = value; } }
	[SerializeField] private int buildCost = 100;

	public float PlaceDistance { get { return placeDistance; } private set { placeDistance = value; } }
	[SerializeField] private float placeDistance = 6;

	public float BuildTime { get { return buildTime; } private set { buildTime = value; } }
	[SerializeField] private float buildTime = 1.0f;

	[SerializeField] private Collider col = null;
	[SerializeField] private Behaviour[] toEnableOnBuild = null;
	[SerializeField] private bool enableOnStart = false;

	private int collisions = 0;

	void Start( )
	{
		Assert.IsNotNull( indicator );
		Assert.IsNotNull( col );
		Assert.IsNotNull( toEnableOnBuild );
		Assert.AreNotEqual( toEnableOnBuild.Length, 0 );

		indicator.HideAll( );

		if ( enableOnStart )
			EnableBuilding( );
	}

	void OnDisable( )
	{
		if ( BuildingManager.Instance )
			BuildingManager.Instance.RemoveBuilding( this );
	}

	void OnMouseDown( )
	{
		//GetComponentInChildren<MeshRenderer>( ).material.color = Random.ColorHSV( );
		Debug.Log( $"Clicked: {name}" );
	}

	void OnMouseEnter( )
	{
		if ( !LevelManager.Instance.Paused )
			indicator.ShowRange( true );
	}

	void OnMouseExit( )
	{
		indicator.HideRange( );
	}

	void OnTriggerEnter( Collider other )
	{
		if ( CollidesWithTags( other.gameObject ) )
			collisions++;
	}

	void OnTriggerExit( Collider other )
	{
		if ( CollidesWithTags( other.gameObject ) )
			collisions--;
	}

	public void EnableBuilding( )
	{
		BuildingManager.Instance.AddBuilding( this );
		col.isTrigger = false;

		foreach ( var item in toEnableOnBuild )
			item.enabled = true;
	}

	public bool CanBePaced( ) => collisions == 0;

	public void ShowPlaceZone( bool show ) => indicator.ShowZone( show );

	public void ShowRange( bool canBuild ) => indicator.ShowRange( canBuild );
	public void HideRange( ) => indicator.HideRange( );

	public bool AreWeCloseEnough( Building anotherBuilding )
		=> Vector3.Distance(transform.position, anotherBuilding.gameObject.transform.position) <= placeDistance;

	private bool CollidesWithTags( GameObject gameObject )
	{
		return gameObject.CompareTag( Tags.Building ) ||
			   gameObject.CompareTag( Tags.Enemy ) ||
			   gameObject.CompareTag( Tags.Environment );
	}
}
