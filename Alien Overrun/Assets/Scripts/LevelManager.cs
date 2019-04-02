/**
 * Description: Holds settings and basic functionality for game level.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private bool paused = false;
	public bool Paused { get { return paused; } set { paused = value; } }

	[SerializeField] private bool creativeMode = false;
	public bool CreativeMode { get { return creativeMode; } set { creativeMode = value; } }

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
		//Assert.IsNotNull(  );

		if ( !CreativeMode )
			InvokeRepeating( "CheckGameEnd", 1, 0.5f );
	}

	void Update ()
	{

	}

	private void CheckGameEnd()
	{
		// Lose
		if ( BuildingManager.Instance.CoresLeft( ) <= 0 )
		{
			Debug.Log( "Game Over" );
			CancelInvoke( "CheckGameEnd" );
		}

		// Win
		if ( EnemyManager.Instance.EndOfWaves && EnemyManager.Instance.Enemies.Count == 0 )
		{
			Debug.Log( "Game WON!" );
			CancelInvoke( "CheckGameEnd" );
		}
	}
}
