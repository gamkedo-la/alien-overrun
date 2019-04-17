/**
 * Description: Shows and moves where enemies will be coming from.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class SpawnIndicator : MonoBehaviour
{
	[SerializeField] private Transform spawnPoint = null;
	[SerializeField] private Transform pointOfPlane = null;
	[Space]
	[SerializeField] private Renderer[] renderers = null;
	[SerializeField] private Material goodMat = null;
	[SerializeField] private Material badMat = null;

	private Vector3 mouseOffset;
	private Plane plane;
	private bool placing = false;
	private bool canPlace = false;
	private int collisions = 0;
	private Camera cam = null;

	void Start( )
	{
		Assert.IsNotNull( spawnPoint );
		Assert.IsNotNull( pointOfPlane );
		Assert.IsNotNull( renderers );
		Assert.AreNotEqual( renderers.Length, 0 );
		Assert.IsNotNull( goodMat );
		Assert.IsNotNull( badMat );

		cam = Camera.main;
	}

	void Update( )
	{
		Move( );
		CheckIfCanPlace( );
		TryPlace( );
	}

	void OnTriggerEnter( Collider other )
	{
		if ( other.gameObject.CompareTag( Tags.Environment ) && other.gameObject.name != "Ground Plane" )
			collisions++;
	}

	void OnTriggerExit( Collider other )
	{
		if ( other.gameObject.CompareTag( Tags.Environment ) && other.gameObject.name != "Ground Plane" )
			collisions--;
	}

	public void StartPlacing( )
	{
		placing = true;
		Vector3 upVector = Vector3.up;
		plane = new Plane( upVector, pointOfPlane.position );
		mouseOffset = Vector3.zero;
	}

	private void Move( )
	{
		if ( !placing )
			return;

		Ray mRay = cam.ScreenPointToRay( Input.mousePosition );
		if ( plane.Raycast( mRay, out float mouseDistance ) )
			transform.position = mRay.GetPoint( mouseDistance ) + mouseOffset;
	}

	private void CheckIfCanPlace( )
	{
		if ( !placing )
			return;

		canPlace = collisions == 0;

		if ( canPlace )
			foreach ( var item in renderers )
				item.material = goodMat;
		else
			foreach ( var item in renderers )
				item.material = badMat;
	}

	private void TryPlace( )
	{
		if ( !placing || !canPlace )
			return;

		if ( !Input.GetMouseButtonDown( 0 ) ) // Only if we press LMB
			return;

		placing = false;
		spawnPoint.position = transform.position;
	}
}
