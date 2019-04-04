/**
 * Description: Manages player's resources.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public enum ResourceType
{
	Minerals,
}

public class ResourceManager : MonoBehaviour
{
	public static ResourceManager Instance { get; private set; }

	public int Minerals { get; private set; }

	[SerializeField] private TextMeshProUGUI mineralsLabel = null;
	[SerializeField] private int startMinerals = 300;

	private void Awake( )
	{
		if ( Instance != null && Instance != this )
			Destroy( gameObject );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }

	private void Start( )
	{
		Assert.IsNotNull( mineralsLabel );

		Minerals = startMinerals;
	}

	private void FixedUpdate( )
	{
		CreativeModeResourceCheck( );
	}

	public void AddResources( ResourceType type, int amount )
	{
		switch ( type )
		{
			case ResourceType.Minerals:
			Minerals = Mathf.Clamp( Minerals + amount, 0, 10000 );
			break;

			default:
			break;
		}

		UpdateLabels( );
	}

	public void UseResources( ResourceType type, int amount )
	{
		switch ( type )
		{
			case ResourceType.Minerals:
			Minerals = Mathf.Clamp( Minerals - amount, 0, 10000 );
			break;

			default:
			break;
		}

		UpdateLabels( );
	}

	public bool CheckResources( ResourceType type, int amount )
	{
		switch ( type )
		{
			case ResourceType.Minerals:
			return Minerals >= amount;

			default:
			break;
		}

		return false;
	}

	private void CreativeModeResourceCheck( )
	{
		if ( LevelManager.Instance.CreativeMode && Minerals < 10000 )
			Minerals = 10000;
	}

	private void UpdateLabels( )
	{
		mineralsLabel.text = $"Minerals: {Minerals}";
	}
}
