/**
 * Description: Core functionality of placing buildings on the ground.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BuildingPlacer : MonoBehaviour
{
	[SerializeField] private Button button = null;
	[SerializeField] private GameObject tooltip = null;
	[SerializeField] private TextMeshProUGUI tooltipInfo = null;
	[SerializeField] private TextMeshProUGUI buttonText = null;
	[SerializeField] private Building building = null;
	[SerializeField] private GameObject buildingPlacer = null;
	[SerializeField] private Transform pointOfPlane = null;
	[SerializeField] private bool requiresCastle = true;
    [SerializeField] public AudioClip placeSound;

    private int costM = 0;
	private int costC = 0;
	private Building buildingToPlace;
	private Vector3 mouseOffset;
	private Plane plane;
	private string buildingName;
	private bool canPlace = false;
	private Camera cam = null;
    private AudioSource aud = null;

    void Start ()
	{
		Assert.IsNotNull( button );
		Assert.IsNotNull( tooltipInfo );
		Assert.IsNotNull( button );
		Assert.IsNotNull( buttonText );
		Assert.IsNotNull( building );
		Assert.IsNotNull( buildingPlacer );
		Assert.IsNotNull( pointOfPlane );

		buildingName = building.BuildingName;
		costM = building.BuildCostMinerals;
		costC = building.BuildCostCrystals;
		buttonText.text = $"{buildingName}";//\n[{costM}M {costC}C";// {building.Threat}F]";
		tooltipInfo.text = $"{building.Info}\n<b><color=#FAA70D><size=12>Minerals: {costM}</color>\n<color=#0D91FA>Crystals: {costC}</size></color></b>\n<color=#FF5353>Threat: {building.Threat}</color>";

		cam = Camera.main;

        aud = gameObject.AddComponent<AudioSource>();
    }

	void Update ()
	{
        CheckCancelBuild( );
		MoveBuilding( );
		CheckIfCanPlaceBuilding( );
		UpdateIndicator( );
		TryPlaceBuilding( );
	}

	void FixedUpdate( )
	{
		CheckRequirements( );
	}

	public void OnOverEnter( )
	{
		Invoke( "ShowTooltip", 0.25f );
	}

	public void OnOverExit( )
	{
		HideTooltip( );
	}

	private void ShowTooltip( )
	{
		tooltip.SetActive( true );
	}

	private void HideTooltip( )
	{
		CancelInvoke( "ShowTooltip" );
		tooltip.SetActive( false );
	}

	public void StartPlaceing( )
	{
		BuildingManager.Instance.Building = true;
		AIProgressManager.Instance.NextThreatShow( building.Threat );

		BuildingManager.Instance.ShowZones( true );
		buildingToPlace = Instantiate( buildingPlacer, transform.position, Quaternion.identity ).GetComponent<Building>( );
		if ( buildingToPlace.BuildingType == BuildingType.Castle || buildingToPlace.BuildingType == BuildingType.Core )
			buildingToPlace.ShowPlaceZone( true );

		ResourceManager.Instance.UseResources( ResourceType.Minerals, costM );
		ResourceManager.Instance.UseResources( ResourceType.Crystals, costC );

		Vector3 upVector = Vector3.up;
		plane = new Plane( upVector, pointOfPlane.position );
		mouseOffset = Vector3.zero;
	}

	private void CheckCancelBuild( )
	{
		if ( !BuildingManager.Instance.Building || buildingToPlace == null )
			return;

		// Cancel only on RMB or Esc
		if ( ! ( Input.GetMouseButtonDown( 1 ) || Input.GetKeyDown( KeyCode.Escape ) ) )
			return;

		ResourceManager.Instance.AddResources( ResourceType.Minerals, costM );
		ResourceManager.Instance.AddResources( ResourceType.Crystals, costC );
		AIProgressManager.Instance.NextThreatHide( );
		Destroy( buildingToPlace.gameObject );
		buildingToPlace = null;
		BuildingManager.Instance.ShowZones( false );
		Invoke( "CanBuild", 0.01f );
	}

	private void CanBuild( )
	{
		BuildingManager.Instance.Building = false;
	}

	private void MoveBuilding( )
	{
		if ( buildingToPlace == null )
			return;

		if ( buildingToPlace.BuildingType == BuildingType.Castle || buildingToPlace.BuildingType == BuildingType.Core )
			buildingToPlace.ShowPlaceZone( true );

		Ray mRay = cam.ScreenPointToRay( Input.mousePosition );
		if ( plane.Raycast( mRay, out float mouseDistance ) )
			buildingToPlace.transform.position = mRay.GetPoint( mouseDistance ) + mouseOffset;
	}

	private void CheckIfCanPlaceBuilding( )
	{
		if ( buildingToPlace == null )
			return;

		if ( building.BuildingType != BuildingType.Castle )
			canPlace = BuildingManager.Instance.CanPlaceBuiding( buildingToPlace );
		else
			canPlace = true;

		if ( !canPlace )
			return;

		canPlace = buildingToPlace.CanBePaced( );

		if ( canPlace && building.BuildingType == BuildingType.Castle )
		{
			float minRange = building.PlaceDistance;
			IEnumerable<GameObject> oponents = ResourceManagerMineral.Instance.GetGameObjects( );
			//Debug.Log( $"{minRange}, {oponents.Count()}" );
			canPlace = false;

			foreach ( var o in oponents )
			{
				if ( Vector3.Distance( buildingToPlace.transform.position, o.transform.position ) <= minRange )
					canPlace = true;
			}
		}
		else if ( canPlace && building.GetComponent<ResourceMinerTowerMineral>() )
		{
			float minRange = building.GetComponent<OponentFinder>().GetAttackDistance();
			IEnumerable<GameObject> oponents = ResourceManagerMineral.Instance.GetGameObjects( );
			//Debug.Log( $"{minRange}, {oponents.Count()}" );
			canPlace = false;

			foreach ( var o in oponents )
			{
				if ( Vector3.Distance( buildingToPlace.transform.position, o.transform.position ) <= minRange )
					canPlace = true;
			}
		}
		else if ( canPlace && building.GetComponent<ResourceMinerTowerCrystal>( ) )
		{
			float minRange = building.GetComponent<OponentFinder>( ).GetAttackDistance( );
			IEnumerable<GameObject> oponents = ResourceManagerCrystal.Instance.GetGameObjects( );
			//Debug.Log( $"{minRange}, {oponents.Count()}" );
			canPlace = false;

			foreach ( var o in oponents )
			{
				if ( Vector3.Distance( buildingToPlace.transform.position, o.transform.position ) <= minRange )
					canPlace = true;
			}
		}
	}

	private void UpdateIndicator( )
	{
		if ( buildingToPlace == null )
			return;

		buildingToPlace.ShowRange( canPlace );
	}

	private void TryPlaceBuilding( )
	{
		if ( buildingToPlace == null )
			return;

		if ( !Input.GetMouseButtonDown( 0 ) ) // Only if we press LMB
			return;

		if ( !canPlace )
			return;

        buildingToPlace.HideRange( );
		buildingToPlace.EnableBuilding( );
		buildingToPlace = null;
		BuildingManager.Instance.ShowZones( false );
		BuildingManager.Instance.Building = false;

		AIProgressManager.Instance.NextThreatHide( );

		FMODUnity.RuntimeManager.PlayOneShot("event:/Building_Placer");
		//aud.pitch = Random.Range( 0.9f, 1.1f );
        //aud.PlayOneShot(placeSound);
    }

	private void CheckRequirements( )
	{
		bool canBuild = false;

		if ( ResourceManager.Instance.CheckResources( ResourceType.Minerals, costM ) &&
			 ResourceManager.Instance.CheckResources( ResourceType.Crystals, costC ) &&
			 !BuildingManager.Instance.Building )
			canBuild = true;

		if ( requiresCastle && !BuildingManager.Instance.CastlePlaced )
			canBuild = false;

		if ( !requiresCastle && BuildingManager.Instance.CastlePlaced )
			canBuild = false;

		button.interactable = canBuild;
	}
}
