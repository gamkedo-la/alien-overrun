/**
 * Description: Manages visual indicator (can be used with different kinds).
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class Indicator : MonoBehaviour
{
	[SerializeField] private OponentFinder oponentFinder = null;
	[SerializeField] private float addedDistance = 0f;
	[SerializeField] private Building building = null;
	[Space]
	[SerializeField] private GameObject zone = null;
	[SerializeField] private GameObject range = null;
	[SerializeField] private Renderer[] rangeRenderers = null;
	[Space]
	[SerializeField] private Material onMat = null;
	[SerializeField] private Material offMat = null;
	[SerializeField] private Material zoneMat = null;
	[Space]
	[SerializeField] private Material buildingNormalMat = null;
	[SerializeField] private Material buildingBadMat = null;
	[SerializeField] private Renderer[] buildingRenderers = null;
	[SerializeField] private GameObject[] toHideOnBad = null;

	void Start( )
	{
		Assert.IsNotNull( oponentFinder);
		Assert.IsNotNull( building);
		Assert.IsNotNull( range);
		Assert.IsNotNull( rangeRenderers );
		Assert.AreNotEqual( rangeRenderers.Length, 0 );
		Assert.IsNotNull( zone );
		Assert.IsNotNull( onMat );
		Assert.IsNotNull( offMat );
		Assert.IsNotNull( zoneMat );
		Assert.IsNotNull( buildingNormalMat );
		Assert.IsNotNull( buildingBadMat );
		Assert.IsNotNull( buildingRenderers );
		Assert.AreNotEqual( buildingRenderers.Length, 0 );
		Assert.AreNotEqual( toHideOnBad.Length, 0 );

		range.transform.localScale = Vector3.one * ( ( oponentFinder.GetAttackDistance( ) + addedDistance ) * 2f );
		zone.transform.localScale = Vector3.one * ( building.PlaceDistance * 2f );
	}

	public void HideAll( )
	{
		zone.SetActive( false );
		range.SetActive( false );
	}

	public void ShowRange( bool canBuild )
	{
		if ( canBuild )
			ShowRangeGood( );
		else
			ShowRangeBad( );
	}

	public void HideRange( )
	{
		range.SetActive( false );
	}

	public void ShowZone( bool show )
	{
		zone.SetActive( show );
	}

	private void ShowRangeGood( )
	{
		range.SetActive( true );
		foreach ( var item in rangeRenderers )
			item.material = onMat;

		foreach (var item in buildingRenderers)
			item.material = buildingNormalMat;

		foreach ( var item in toHideOnBad )
			item.SetActive( true );
	}

	private void ShowRangeBad( )
	{
		range.SetActive( true );
		foreach ( var item in rangeRenderers )
			item.material = offMat;

		foreach (var item in buildingRenderers)
			item.material = buildingBadMat;

		foreach ( var item in toHideOnBad )
			item.SetActive( false );
	}
}
