/**
 * Description: Holds settings and basic functionality for game level.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private GameObject[] skirmishModeOnly = null;
	[SerializeField] private GameObject[] creativeModeOnly = null;

	[SerializeField] private bool paused = false;
	public bool Paused { get { return paused; } set { paused = value; } }

	[SerializeField] private bool creativeMode = false;
	public bool CreativeMode
	{
		get { return creativeMode; }
		set
		{
			creativeMode = value;
			SwitchMode( );
		}
	}

	[SerializeField] private UnityEvent onWin = null;
	[SerializeField] private UnityEvent onLose = null;

	public static LevelManager Instance { get; private set; }

	private void Awake( )
	{
		if ( Instance != null && Instance != this )
			Destroy( gameObject );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }

	void Start ()
	{
		Assert.IsNotNull( skirmishModeOnly );
		Assert.AreNotEqual( skirmishModeOnly.Length, 0 );
		Assert.IsNotNull( creativeModeOnly );
		Assert.AreNotEqual( creativeModeOnly.Length, 0 );

		// "Load" info about selected mode
		ModeSelection modeSelection = FindObjectOfType<ModeSelection>( );
		if ( modeSelection )
			CreativeMode = modeSelection.CreativeMode;

		SwitchMode( );
	}

	void Update ()
	{

	}

	private void CheckGameEnd()
	{
		// Lose
		if ( BuildingManager.Instance.CoresLeft( ) <= 0 )
		{
			CancelInvoke( "CheckGameEnd" );
			onLose.Invoke( );
		}

		// Win
		if ( EnemyManager.Instance.EndOfWaves && EnemyManager.Instance.Enemies.Count == 0 )
		{
			CancelInvoke( "CheckGameEnd" );
			onWin.Invoke( );
		}
	}

	private void SwitchMode( )
	{
		if ( CreativeMode )
		{
			foreach ( var go in skirmishModeOnly )
				go.SetActive( false );

			foreach ( var go in creativeModeOnly )
				go.SetActive( true );
		}
		else
		{
			foreach ( var go in skirmishModeOnly )
				go.SetActive( true );

			foreach ( var go in creativeModeOnly )
				go.SetActive( false );

			InvokeRepeating( "CheckGameEnd", 1, 0.5f );
		}
	}
}
