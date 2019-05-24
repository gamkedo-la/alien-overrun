/**
 * Description: Core functionality of placing buildings on the ground.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BuildingPlacer : MonoBehaviour
{
	[SerializeField] private Button button = null;
	[SerializeField] private TextMeshProUGUI buttonText = null;
	[SerializeField] private Building building = null;
	[SerializeField] private GameObject buildingPlacer = null;
	[SerializeField] private Transform pointOfPlane = null;
	[SerializeField] private bool requiresFirstThreshold = true;
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
		Assert.IsNotNull( buttonText );
		Assert.IsNotNull( building );
		Assert.IsNotNull( buildingPlacer );
		Assert.IsNotNull( pointOfPlane );

		buildingName = building.BuildingName;
		costM = building.BuildCostMinerals;
		costC = building.BuildCostCrystals;
		buttonText.text = $"{buildingName}\n[{costM}M {costC}C {building.Threat}F]";

		cam = Camera.main;

        aud = gameObject.AddComponent<AudioSource>();
        aud.clip = placeSound;
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

	public void StartPlaceing( )
	{
		BuildingManager.Instance.Building = true;

		BuildingManager.Instance.ShowZones( true );
		buildingToPlace = Instantiate( buildingPlacer, transform.position, Quaternion.identity ).GetComponent<Building>( );
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
		aud.pitch = Random.Range( 0.9f, 1.1f );
        aud.PlayOneShot(placeSound);
    }

	private void CheckRequirements( )
	{
		bool canBuild = false;

		if ( ResourceManager.Instance.CheckResources( ResourceType.Minerals, costM ) &&
			 ResourceManager.Instance.CheckResources( ResourceType.Crystals, costC ) &&
			 !BuildingManager.Instance.Building )
			canBuild = true;

		if ( requiresFirstThreshold && !AIProgressManager.Instance.FistTheasholdReached )
			canBuild = false;

		if ( !requiresFirstThreshold && AIProgressManager.Instance.FistTheasholdReached )
			canBuild = false;

		button.interactable = canBuild;
	}
}
