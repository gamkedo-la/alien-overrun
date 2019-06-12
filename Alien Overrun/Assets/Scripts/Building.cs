/**
 * Description: Main class of all buildings. Responsible for core behaviors.
 * Authors: Kornel, SpadXIII
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections;
using System.Collections.Generic;
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

	public int BuildCostMinerals { get { return buildCostMinerals; } private set { buildCostMinerals = value; } }
	[SerializeField] private int buildCostMinerals = 100;

	public int BuildCostCrystals { get { return buildCostCrystals; } private set { buildCostCrystals = value; } }
	[SerializeField] private int buildCostCrystals = 0;

	public int RepairCostPercent { get { return repairCostPercent; } private set { repairCostPercent = value; } }
	[SerializeField] private int repairCostPercent = 50;

	public int MoveCostPercent { get { return moveCostPercent; } private set { moveCostPercent = value; } }
	[SerializeField] private int moveCostPercent = 35;

	public int DeleteCostPercent { get { return deleteCostPercent; } private set { deleteCostPercent = value; } }
	[SerializeField] private int deleteCostPercent = 50;

	public int Threat { get { return threat; } private set { threat = value; } }
	[SerializeField] private int threat = 10;

	public string Color { get { return color; } private set { color = value; } }
	[SerializeField] private string color = "FFFFFF";

	public string Info { get { return info; } }
	[SerializeField] private string info = "";

	public float PlaceDistance { get { return placeDistance; } private set { placeDistance = value; } }
	[SerializeField] private float placeDistance = 6;

	public float BuildTime { get { return buildTime; } private set { buildTime = value; } }
	[SerializeField] private float buildTime = 1.0f;

	[SerializeField] private Collider col = null;
	[SerializeField] private Collider colP = null;
	[SerializeField] private GameObject placeEffect = null;
	[SerializeField] private Behaviour[] toEnableOnBuild = null;
	[SerializeField] private bool enableOnStart = false;

	private int collisions = 0;
	private HP hp = null;

	void Start( )
	{
		Assert.IsNotNull( indicator );
		Assert.IsNotNull( col );
		Assert.IsNotNull( colP );
		Assert.IsNotNull( placeEffect );
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

		AIProgressManager.Instance.AddThreat( Threat, transform.position );

		Instantiate( placeEffect, transform.position, Quaternion.identity );
		StartCoroutine( ShowBuilding( ) );

		if ( BuildingType == BuildingType.Castle )
			BuildingManager.Instance.CastlePlaced = true;
	}

	private IEnumerator ShowBuilding()
	{
		Vector3 scale = transform.localScale;
		scale.y = 0;
		transform.localScale = scale;

		while ( transform.localScale.y < 1 )
		{
			scale.y += Time.deltaTime * BuildTime;
			scale.y = Mathf.Clamp( scale.y, 0f, 1f );
			transform.localScale = scale;

			yield return null;
		}
	}

	public void DisableBuildingToMoveAgain( )
	{
		BuildingManager.Instance.RemoveItem( this );
		col.enabled = true;

		foreach ( var item in toEnableOnBuild )
		{
			item.enabled = false;
		}

		colP.enabled = false;

		//AIProgressManager.Instance.RemoveThreat( Threat );
	}

	public bool CanBePaced( ) => collisions == 0;

	public void ShowPlaceZone( bool show ) => indicator.ShowZone( show );

	public void ShowRange( bool canBuild ) => indicator.ShowRange( canBuild );

	public void HideRange( ) => indicator.HideRange( );

	public bool AreWeCloseEnough( Building anotherBuilding )
		=> Vector3.Distance( transform.position, anotherBuilding.gameObject.transform.position ) <= ( placeDistance + anotherBuilding.placeDistance ) * 0.99f;

	public void BuildingDestroyed( )
	{
		if ( BuildingType == BuildingType.Castle && LevelManager.Instance.CreativeMode )
		{
			GetComponent<HP>( ).ChangeHP( 1000 );
			return;
		}

		if ( BuildingType != BuildingType.Castle )
		{
			AIProgressManager.Instance.RemoveThreat( Threat, transform.position );
			ScreenShake.Instance.DoLow( );
		}
		else
		{
			ScreenShake.Instance.DoBig( );
		}

		Destroy( gameObject );
        FMODUnity.RuntimeManager.PlayOneShot("event:/Buildings/BuildingDestroyed");
	}

	public (float Minerals, float Crystals) GetRepairCost( )
	{
		return ( BuildCostMinerals * ( (float)RepairCostPercent / 100 ) * ( 1 - ( hp.CurrentHP / hp.MaxHP ) ),
				 BuildCostCrystals * ( (float)RepairCostPercent / 100 ) * ( 1 - ( hp.CurrentHP / hp.MaxHP ) ) );
	}

	public (float Minerals, float Crystals) GetDeleteCost( )
	{
		return (BuildCostMinerals * ((float)DeleteCostPercent / 100) * (hp.CurrentHP / hp.MaxHP),
				BuildCostCrystals * ((float)DeleteCostPercent / 100) * (hp.CurrentHP / hp.MaxHP) );
	}

	public (float Minerals, float Crystals) GetMoveCost( )
	{
		return (BuildCostMinerals * (float)MoveCostPercent / 100f,
				BuildCostCrystals * (float)MoveCostPercent / 100f);
	}

	public void Repair( )
	{
		if ( BuildCostMinerals == 0 && BuildCostCrystals == 0)
			return;

		(float Minerals, float Crystals) repairCost = GetRepairCost( );

		if ( ResourceManager.Instance.CheckResources(ResourceType.Minerals, (int)repairCost.Minerals ) &&
			 ResourceManager.Instance.CheckResources( ResourceType.Crystals, (int)repairCost.Crystals ) )
		{
			ResourceManager.Instance.UseResources( ResourceType.Minerals, (int)repairCost.Minerals );
			ResourceManager.Instance.UseResources( ResourceType.Crystals, (int)repairCost.Crystals );
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
