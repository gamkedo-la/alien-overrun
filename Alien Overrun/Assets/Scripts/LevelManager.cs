/**
 * Description: Holds settings and basic functionality for game level.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private GameObject[] skirmishModeOnly = null;
	[SerializeField] private GameObject[] creativeModeOnly = null;
	[SerializeField] private Transform mineralSets = null;
	[SerializeField] private Transform crystalSets = null;
	[SerializeField] private Transform terrainSets = null;

	public bool Paused { get { return paused; } set { paused = value; } }
	[SerializeField] private bool paused = false;

	public float LevelDifficultyModifier { get { return dificultyLevelModifier; } set { dificultyLevelModifier = value; } }
	[SerializeField, Range(0.2f, 5f)] private float dificultyLevelModifier = 1.0f;

	public bool CreativeMode
	{
		get { return creativeMode; }
		set
		{
			creativeMode = value;
			SwitchMode( );
		}
	}
	[SerializeField] private bool creativeMode = false;

	[SerializeField] private UnityEvent onWin = null;
	[SerializeField] private UnityEvent onLose = null;

	public static LevelManager Instance { get; private set; }

    [SerializeField] private GameObject backgroundMusicObject;

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
		Assert.IsNotNull( mineralSets );
		Assert.IsNotNull( crystalSets );
		Assert.IsNotNull( terrainSets );

		Transform[] minerals = mineralSets.Cast<Transform>( ).ToArray( );
		foreach ( var m in minerals )
			m.gameObject.SetActive( false );
		minerals[Random.Range( 0, minerals.Length )].gameObject.SetActive( true );

		Transform[] crystals = crystalSets.Cast<Transform>( ).ToArray( );
		foreach ( var c in crystals )
			c.gameObject.SetActive( false );
		crystals[Random.Range( 0, crystals.Length )].gameObject.SetActive( true );

		Transform[] terrains = terrainSets.Cast<Transform>( ).ToArray( );
		foreach ( var t in terrains )
			t.gameObject.SetActive( false );
		terrains[Random.Range( 0, terrains.Length )].gameObject.SetActive( true );

		// "Load" info about selected mode
		ModeSelection modeSelection = FindObjectOfType<ModeSelection>( );
		if ( modeSelection )
		{
			CreativeMode = modeSelection.CreativeMode;
			LevelDifficultyModifier = modeSelection.LevelDifficultyModifier;
		}

        backgroundMusicObject = GameObject.FindGameObjectWithTag("BackgroundMusic");
        Assert.IsNotNull(backgroundMusicObject);
        //Debug.Log(backgroundMusic);
		SwitchMode( );
	}

	void Update ()
	{

	}

	private void CheckGameEnd()
	{
		// Lose
		if ( BuildingManager.Instance.CastlesLeft( ) <= 0 && AIProgressManager.Instance.FistTheasholdReached )
		{
			CancelInvoke( "CheckGameEnd" );
			onLose.Invoke( );
            FMODUnity.RuntimeManager.PlayOneShot("event:/General SFX/gameOver");
		}

		// Win
		if ( EnemyManager.Instance.EndOfWaves && EnemyManager.Instance.ItemsList.Count == 0 )
		{
			CancelInvoke( "CheckGameEnd" );
			onWin.Invoke( );
            backgroundMusicObject.GetComponent<AudioSource>().Stop();
            FMODUnity.RuntimeManager.PlayOneShot("event:/General SFX/winGame");
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

			CancelInvoke( "CheckGameEnd" );
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
