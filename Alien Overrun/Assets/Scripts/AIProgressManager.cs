/**
 * Description: Manages the aggression level of the AI and overall game difficulty.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[System.Serializable]
public class WaveParameters
{
	public float DelayBeforeWave = 2f;
	public float SpawnDelayMaxOffsetPercent = 20f;
	public float DelayBetweenEnemies = 1f;
	public float DelayBetweenEnemiesMaxOffsetPercent = 20f;
	public float EnemiesInWave = 5;
	public float EnemiesInWaveMaxOffsetPercent = 20f;
	public bool[] ChanceForMegaWave = {true, false, false, false, false, false };
	public float MegaWaveMultiplayer = 2.0f;
	public float[] EnemyTypePercentChance = {0.34f, 0.33f, 0.33f };
	public float[] SpawnPointIDPercentChance = {0.34f, 0.33f, 0.33f };
}

[System.Serializable]
public class Threshold
{
	public int Value;
	public Color Color;
	public string Message;
	public WaveParameters ParametersChangeOnThreshold;
	public WaveParameters ParametersChangePerPoint;
}

public class AIProgressManager : MonoBehaviour
{
	[Header("Bar")]
	[SerializeField] private Slider bar = null;
	[SerializeField] private Image progressImage = null;
	[SerializeField] private TextMeshProUGUI threatLabel = null;
	[Header("Markers")]
	[SerializeField] private GameObject progressMarker = null;
	[SerializeField] private Transform markersParent = null;
	[SerializeField] private float minPos = -191f;
	[SerializeField] private float maxPos = 191f;
	[Header( "Threat" )]
	[SerializeField] private GameObject threatText = null;
	[SerializeField] private RectTransform threatDestination = null;
	[SerializeField] private Threshold[] thresholds = null;
	[SerializeField] private int threatCurrent = 0;

	private List<ProgressMarker> progressMarkers = new List<ProgressMarker>();
	private int threatMax;
	private ProgressMarker currentProgressMarker;

	public static AIProgressManager Instance { get; private set; }

	public int Threat { get; private set; }
	public bool FistTheasholdReached { get; private set; } = false;

	private void Awake( )
	{
		if ( Instance != null && Instance != this )
			Destroy( gameObject );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }

	void Start( )
	{
		Assert.IsNotNull( bar );
		Assert.IsNotNull( progressImage );
		Assert.IsNotNull( threatLabel );
		Assert.IsNotNull( markersParent );
		Assert.IsNotNull( progressMarker );
		Assert.IsNotNull( threatText );
		Assert.IsNotNull( threatDestination );

		threatMax = thresholds[thresholds.Length - 1].Value;
		bar.maxValue = threatMax;
		bar.value = 0;

		for ( int i = 0; i < thresholds.Length; i++ )
		{
			ProgressMarker pm = Instantiate( progressMarker, markersParent ).GetComponent<ProgressMarker>();
			progressMarkers.Add( pm );
			float xPos = Utilities.ConvertRange( 0, threatMax, minPos, maxPos, thresholds[i].Value );
			pm.Set( new Vector2( xPos, 0 ), thresholds[i].Value, thresholds[i].Color, thresholds[i].Message,
					thresholds[i].ParametersChangeOnThreshold, thresholds[i].ParametersChangePerPoint, i == thresholds.Length - 1 );
		}

		currentProgressMarker = progressMarkers[0];

		UpdateBar( );
	}

	public void AddThreat( int amount, Vector3 position )
	{
		var go = Instantiate( threatText, Camera.main.WorldToScreenPoint( position ), Quaternion.identity, transform );
		go.GetComponent<ThreatText>( ).Set( amount, threatDestination.localPosition );
	}

	public void AddThreatNow( int amount )
	{
		int oldThreat = threatCurrent;
		threatCurrent += amount;
		threatCurrent = Mathf.Clamp( threatCurrent, 0, threatMax );

		EnemyManager.Instance.ChangeParametersOnThreatChange( currentProgressMarker.ParametersChangePerPoint, threatCurrent - oldThreat );

		UpdateBar( );
	}

	public void RemoveThreat( int amount, Vector3 position )
	{
		var go = Instantiate( threatText, Camera.main.WorldToScreenPoint( position ), Quaternion.identity, transform );
		go.GetComponent<ThreatText>( ).Set( -amount, threatDestination.localPosition );
	}

	public void RemoveThreatNow( int amount )
	{
		int oldThreat = threatCurrent;
		threatCurrent -= amount;
		threatCurrent = Mathf.Clamp( threatCurrent, 0, threatMax );

		EnemyManager.Instance.ChangeParametersOnThreatChange( currentProgressMarker.ParametersChangePerPoint, threatCurrent - oldThreat );

		UpdateBar( );
	}

	private void UpdateBar( )
	{
		bar.value = threatCurrent;
		threatLabel.text = threatCurrent.ToString( );

		foreach ( var pm in progressMarkers )
		{
			if ( !pm.Reached && pm.Theashold <= threatCurrent )
			{
				pm.Activate( );
				progressImage.color = pm.ActiveColor;
				Color largestColor = pm.ActiveColor;

				EnemyManager.Instance.ChangeParametersOnThresholdChange( pm.ParametersChangeOnThreshold );

				FistTheasholdReached = true;

				if ( pm.LastThreshold )
					EnemyManager.Instance.LastThresholdReached( );

				foreach ( var p in progressMarkers ) // A bit of duplicate code but just changes the color off all markers
					if ( p.Reached )
					{
						p.ActiveColor = largestColor;
						p.Activate( );
					}
			}
		}
	}
}
