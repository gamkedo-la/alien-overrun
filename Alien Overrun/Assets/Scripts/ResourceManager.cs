/**
 * Description: Manages player's resources.
 * Authors: Kornel, SpadXIII
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public enum ResourceType
{
	Minerals,
	Crystals,
}

public class ResourceManager : AbstractListManager
{
	public static ResourceManager Instance { get; private set; }
	public int Minerals { get; private set; }
	public int Crystals { get; private set; }

	[SerializeField] private TextMeshProUGUI mineralsLabel = null;
	[SerializeField] private TextMeshProUGUI crystalsLabel = null;
	[SerializeField] private int startMinerals = 300;
	[SerializeField] private int startCrystals = 0;

	private const int maxResourcesPossible = 10000;

	private protected override void Awake( )
	{
		base.Awake( );

		if ( Instance != null && Instance != this )
			Destroy( gameObject );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }

	private void Start( )
	{
		Assert.IsNotNull( mineralsLabel );
		Assert.IsNotNull( crystalsLabel );

		Minerals = startMinerals;
		Crystals = startCrystals;
		UpdateLabels( );
	}

	public void CheatAddResources( )
	{
		AddResources( ResourceType.Minerals, 10000 );
		AddResources( ResourceType.Crystals, 10000 );
	}

	public void AddResources( ResourceType type, int amount )
	{
		switch ( type )
		{
			case ResourceType.Minerals:
			Minerals = Mathf.Clamp( Minerals + amount, 0, maxResourcesPossible );
			break;

			case ResourceType.Crystals:
			Crystals = Mathf.Clamp( Crystals + amount, 0, maxResourcesPossible );
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
			Minerals = Mathf.Clamp( Minerals - amount, 0, maxResourcesPossible );
			break;

			case ResourceType.Crystals:
			Crystals = Mathf.Clamp( Crystals - amount, 0, maxResourcesPossible );
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

			case ResourceType.Crystals:
			return Crystals >= amount;

			default:
			break;
		}

		return false;
	}

	private void UpdateLabels( )
	{
		mineralsLabel.text = $"Minerals: {Minerals}";
		crystalsLabel.text = $"Crystals: {Crystals}";
	}
}
