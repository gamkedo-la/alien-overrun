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
	[SerializeField] private string buildingName = "Budynek";
	public int BuildCost { get { return buildCost; } private set { buildCost = value; } }
	[SerializeField] private int buildCost = 100;
	public float BuildTime { get { return buildTime; } private set { buildTime = value; } }
	[SerializeField] private float buildTime = 1.0f;
	[SerializeField] private Collider col = null;

	private int collisions = 0;

	void Start ()
	{
		Assert.IsNotNull( indicator );
		Assert.IsNotNull( col );

		indicator.SetActive( false );
	}

	void Update ()
	{

	}

	void OnEnable( )
	{
		BuildingManager.Instance.AddBuilding( this );
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
		col.isTrigger = false;
	}

	public bool CanBePaced()
	{
		return collisions == 0;
	}
}
