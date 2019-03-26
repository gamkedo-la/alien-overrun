/**
 * Description: Main class of all buildings. Responsible for core behaviors.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class Building : MonoBehaviour
{
	[SerializeField] private GameObject indicator = null;
	public string BuildingName { get { return buildingName; } private set { buildingName = value; } }

	[SerializeField] private string buildingName = "Building";
	public int BuildCost { get { return buildCost; } private set { buildCost = value; } }

	[SerializeField] private int buildCost = 100;
	public float BuildTime { get { return buildTime; } private set { buildTime = value; } }

	[SerializeField] private float buildTime = 1.0f;
	[SerializeField] private Collider col = null;
	[SerializeField] private Behaviour[] toEnableOnBuild = null;
	[SerializeField] private bool enableOnStart = false;

	private int collisions = 0;

	void Start ()
	{
		Assert.IsNotNull( indicator );
		Assert.IsNotNull( col );
		Assert.IsNotNull( toEnableOnBuild );
		Assert.AreNotEqual( toEnableOnBuild.Length, 0 );

		indicator.SetActive( false );

		if ( enableOnStart )
			EnableBuilding( );
	}

	void Update ()
	{

	}

	void OnEnable( )
	{

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
		indicator.SetActive( true );
	}

	void OnMouseExit( )
	{
		indicator.SetActive( false );
	}

	void OnTriggerEnter( Collider other )
	{
		if ( other.gameObject.CompareTag( "Building" ) || other.gameObject.CompareTag( "Environment" ) )
			collisions++;
	}

	void OnTriggerExit( Collider other )
	{
		if ( other.gameObject.CompareTag( "Building" ) || other.gameObject.CompareTag( "Environment" ) )
			collisions--;
	}

	public void EnableBuilding()
	{
		BuildingManager.Instance.AddBuilding( this );
		col.isTrigger = false;

		foreach ( var item in toEnableOnBuild )
		{
			item.enabled = true;
		}
	}

	public bool CanBePaced()
	{
		return collisions == 0;
	}
}
