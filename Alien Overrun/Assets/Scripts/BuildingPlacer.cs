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

	private int cost = 50;
	private Building buildingToPlace;
	private Vector3 mouseOffset;
	private Plane plane;
	private string buildingName;
	private bool canPlace = false;

	void Start ()
	{
		Assert.IsNotNull( button );
		Assert.IsNotNull( buttonText );
		Assert.IsNotNull( building );
		Assert.IsNotNull( buildingPlacer );
		Assert.IsNotNull( pointOfPlane );

		buildingName = building.BuildingName;
		cost = building.BuildCost;
		buttonText.text = string.Format( "{0} [{1}]", buildingName, cost );
	}

	void Update ()
	{
		MoveBuilding( );
		CheckIfCanPlaceBuilding( );
		TryPlaceBuilding( );
	}

	private void MoveBuilding( )
	{
		if ( buildingToPlace == null )
			return;

		Ray mRay = Camera.main.ScreenPointToRay( Input.mousePosition );
		if ( plane.Raycast( mRay, out float mouseDistance ) )
			buildingToPlace.transform.position = mRay.GetPoint( mouseDistance ) + mouseOffset;
	}

	private void CheckIfCanPlaceBuilding( )
	{
		if ( buildingToPlace == null )
			return;

		// Here we can check things like min. distance to other buildings, etc. And visualize it
		canPlace = true;
		if ( !canPlace )
			return;

		canPlace = buildingToPlace.CanBePaced( );
	}

	private void TryPlaceBuilding( )
	{
		if ( buildingToPlace == null )
			return;

		if ( !Input.GetMouseButtonDown( 0 ) ) // Only if we press LMB
			return;

		if ( !canPlace )
			return;

		buildingToPlace.EnableBuilding( );
		buildingToPlace = null;
	}

	void FixedUpdate( )
	{
		CheckResourceRequirements( );
	}

	public void StartPlaceing()
	{
		buildingToPlace = Instantiate( buildingPlacer, transform.position, Quaternion.identity ).GetComponent<Building>();
		ResourceManager.Instance.UseResources( ResourceType.Minerals, cost );

		Vector3 upVector = Vector3.up;
		plane = new Plane( upVector, pointOfPlane.position );
		mouseOffset = Vector3.zero;
	}

	private void CheckResourceRequirements( )
	{
		if ( ResourceManager.Instance.CheckResources( ResourceType.Minerals, cost ) )
			button.interactable = true;
		else
			button.interactable = false;
	}
}
