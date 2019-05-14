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
public struct Thresholds
{
	public int Value;
	public Color Color;
	public string Message;
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
	[SerializeField] private Thresholds[] thresholds = null;
	[SerializeField] private int threatCurrent = 0;

	private List<ProgressMarker> progressMarkers = new List<ProgressMarker>();
	private int threatMax;

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

		threatMax = thresholds[thresholds.Length - 1].Value;
		bar.maxValue = threatMax;
		bar.value = 0;

		for ( int i = 0; i < thresholds.Length; i++ )
		{
			ProgressMarker pm = Instantiate( progressMarker, markersParent ).GetComponent<ProgressMarker>();
			progressMarkers.Add( pm );
			float xPos = Utilities.ConvertRange( 0, threatMax, minPos, maxPos, thresholds[i].Value );
			pm.Set( new Vector2( xPos, 0 ), thresholds[i].Value, thresholds[i].Color, thresholds[i].Message );
		}

		UpdateBar( );
	}

	public void AddThreat( int amount )
	{
		threatCurrent += amount;
		threatCurrent = Mathf.Clamp( threatCurrent, 0, threatMax );

		UpdateBar( );
	}

	public void RemoveThreat( int amount )
	{
		threatCurrent -= amount;
		threatCurrent = Mathf.Clamp( threatCurrent, 0, threatMax );

		UpdateBar( );
	}

	private void UpdateBar( )
	{
		bar.value = threatCurrent;
		threatLabel.text = threatCurrent.ToString( );

		Color largestColor = Color.white;
		foreach ( var pm in progressMarkers )
		{
			if ( !pm.Reached && pm.Theashold <= threatCurrent )
			{
				pm.Activate( );
				progressImage.color = pm.ActiveColor;
				largestColor = pm.ActiveColor;

				FistTheasholdReached = true;
			}
		}

		// A bit of duplicate code. Just change the color off all markers
		foreach ( var pm in progressMarkers )
			if ( pm.Reached )
			{
				pm.ActiveColor = largestColor;
				pm.Activate( );
			}
	}
}
