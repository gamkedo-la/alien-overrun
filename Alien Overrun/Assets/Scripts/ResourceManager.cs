﻿/**
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
	[SerializeField] private float resourceTick = 1f;
	[SerializeField] private int resourcesPerTick = 10;

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

		Invoke( "ResourceTick", resourceTick );
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

	private void ResourceTick( )
	{
		AddResources( ResourceType.Minerals, resourcesPerTick );
		Invoke( "ResourceTick", resourceTick );
	}

	private void UpdateLabels( )
	{
		mineralsLabel.text = $"Minerals: {Minerals}";
	}
}
