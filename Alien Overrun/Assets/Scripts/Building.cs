/**
 * Description: Main class of all buildings. Responsible for core behaviors.
 * Authors: Kornel, SpadXIII
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public enum BuildingType
{
	Tower,
	Core,
	Castle
}

public class Building : AbstractListableItem
{
	public Indicator Indicator { get { return indicator; } private set { indicator = value; } }
	[SerializeField] private Indicator indicator = null;

	public BuildingType BuildingType { get { return buildingType; } private set { buildingType = value; } }
	[SerializeField] private BuildingType buildingType = BuildingType.Tower;

	public string BuildingName { get { return buildingName; } private set { buildingName = value; } }
	[SerializeField] private string buildingName = "Building";

	public int BuildCost { get { return buildCost; } private set { buildCost = value; } }
	[SerializeField] private int buildCost = 100;

	public int RepairCostPercent { get { return repairCostPercent; } private set { repairCostPercent = value; } }
	[SerializeField] private int repairCostPercent = 50;

	public int Threat { get { return threat; } private set { threat = value; } }
	[SerializeField] private int threat = 10;

	public float PlaceDistance { get { return placeDistance; } private set { placeDistance = value; } }
	[SerializeField] private float placeDistance = 6;

	public float BuildTime { get { return buildTime; } private set { buildTime = value; } }
	[SerializeField] private float buildTime = 1.0f;

	[SerializeField] private Collider col = null;
	[SerializeField] private Collider colP = null;
	[SerializeField] private Behaviour[] toEnableOnBuild = null;
	[SerializeField] private bool enableOnStart = false;

	private int collisions = 0;
	private HP hp = null;

	void Start( )
	{
		Assert.IsNotNull( indicator );
		Assert.IsNotNull( col );
		Assert.IsNotNull( colP );
		Assert.IsNotNull( toEnableOnBuild );
		Assert.AreNotEqual( toEnableOnBuild.Length, 0 );

		indicator.HideAll( );

		SetOponentListManager( );

		if ( enableOnStart )
			EnableBuilding( );

		hp = GetComponent<HP>( );
	}

	protected private virtual void SetOponentListManager( )
	{
		OponentFinder[] oponentFinders = gameObject.GetComponentsInChildren<OponentFinder>( );

		foreach ( var oF in oponentFinders )
			oF.SetOponentListManager( EnemyManager.Instance );
	}

	void OnDisable( )
	{
		if ( BuildingManager.Instance )
			BuildingManager.Instance.RemoveItem( this );
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
		BuildingManager.Instance.AddItem( this );
		col.enabled = false;

		foreach ( var item in toEnableOnBuild )
		{
			item.enabled = true;
		}

		colP.enabled = true;

		AIProgressManager.Instance.AddThreat( Threat );
	}

	public void DisableBuildingToMoveAgain()
	{
		BuildingManager.Instance.RemoveItem( this );
		col.enabled = true;

		foreach (var item in toEnableOnBuild)
		{
			item.enabled = false;
		}

		colP.enabled = false;

		AIProgressManager.Instance.RemoveThreat( Threat );
	}

	public bool CanBePaced( ) => collisions == 0;

	public void ShowPlaceZone( bool show ) => indicator.ShowZone( show );

	public void ShowRange( bool canBuild ) => indicator.ShowRange( canBuild );

	public void HideRange( ) => indicator.HideRange( );

	public bool AreWeCloseEnough( Building anotherBuilding )
		=> Vector3.Distance(transform.position, anotherBuilding.gameObject.transform.position) <= placeDistance;

	public void BuildingDestroyed()
	{
		if (BuildingType != BuildingType.Castle)
			AIProgressManager.Instance.RemoveThreat( Threat );
	}

	public void Repair( )
	{
		if ( BuildCost == 0 )
			return;

		float repairCost = BuildCost * ( (float)RepairCostPercent / 100 ) * ( 1 - ( hp.CurrentHP / hp.MaxHP ) );
		Debug.Log( repairCost );

		if ( ResourceManager.Instance.CheckResources(ResourceType.Minerals, (int)repairCost ) )
		{
			ResourceManager.Instance.UseResources( ResourceType.Minerals, (int)repairCost );
			hp.ChangeHP( hp.MaxHP );
		}
	}

	private bool CollidesWithTags( GameObject gameObject )
	{
		return gameObject.CompareTag( Tags.Building ) ||
			   gameObject.CompareTag( Tags.Enemy ) ||
			   gameObject.CompareTag( Tags.Resource ) ||
			   gameObject.CompareTag( Tags.Environment );
	}
}
