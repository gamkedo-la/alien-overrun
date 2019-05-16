/**
 * Description: Manages enemies, spawns new ones, provides functionality for other scripts.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyManager : AbstractListManager
{
	public static EnemyManager Instance { get; private set; }

	public bool EndOfWaves { get; private set; }

	[Header("Objects")]
	[SerializeField] private GameObject[] enemyPrefabs = null;
	[SerializeField] private Transform crativeModeSpawnPoint = null;
	[SerializeField] private Transform[] spawnPoints = null;
	[SerializeField] private TextMeshProUGUI enemyCount = null;
	[Header("Current Wave Parameters")]
	[SerializeField] private int waveNum = 0;
	[SerializeField] private float waveDelay = 2f;
	[SerializeField] private float spawnDelayMaxOffsetPercent = 20f;
	[SerializeField] private float delayBetweenEnemies = 1f;
	[SerializeField] private float delayBetweenEnemiesMaxOffsetPercent = 20f;
	[SerializeField] private float enemiesInWave = 5;
	[SerializeField] private float enemiesInWaveMaxOffsetPercent = 20f;
	[SerializeField] private float[] enemyTypePercentChance = {0.34f, 0.33f, 0.33f };
	[SerializeField] private float[] spawnPointIDPercentChance = {0.34f, 0.33f, 0.33f };
	[Header("Spawn Options")]
	[SerializeField] private float radius = 3f;
	[SerializeField] private float autoSpawningDelay = 0.1f;

	private Coroutine coroutine;
	private bool autoSpawning = false;
	private bool spawningLastWave = false;

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
		Assert.IsNotNull( enemyPrefabs );
		Assert.AreNotEqual( enemyPrefabs.Length, 0 );
		Assert.IsNotNull( crativeModeSpawnPoint );
		Assert.IsNotNull( spawnPoints );
		Assert.AreNotEqual( spawnPoints.Length, 0 );
		Assert.IsNotNull( enemyCount );

		StartWaves( );
	}

	public void AddItem( Enemy enemy )
	{
		base.AddItem( enemy );
		enemyCount.text = "Enemy count: " + ItemsList.Count;
	}

	public void RemoveItem( Enemy enemy )
	{
		base.RemoveItem( enemy );
		enemyCount.text = "Enemy count: " + ItemsList.Count;
	}

	public void SpawnRandomEnemy( )
	{
		Vector3 enemyPos = Vector3.zero;
		Vector2 circle = Random.insideUnitCircle * radius;
		enemyPos.x = crativeModeSpawnPoint.position.x + circle.x;
		enemyPos.y = crativeModeSpawnPoint.position.y;
		enemyPos.z = crativeModeSpawnPoint.position.z + circle.y;

		Instantiate( enemyPrefabs[Random.Range( 0, enemyPrefabs.Length )], enemyPos, Quaternion.identity );

		if ( autoSpawning )
			Invoke( "SpawnRandomEnemy", autoSpawningDelay );
	}

	public void SpawnRandomEnemyStart( )
	{
		autoSpawning = true;
		Invoke( "SpawnRandomEnemy", autoSpawningDelay );
	}

	public void SpawnRandomEnemyEnd( )
	{
		autoSpawning = false;
	}

	public void SpawnEnemy( int id )
	{
		SpawnNewEnemy( id, crativeModeSpawnPoint );
	}

	public void NextWaveNow( )
	{
		/*StopCoroutine( coroutine );
		currentWave.WaveDelay = 0;
		coroutine = StartCoroutine( SpawnWaves( ) );*/
		Debug.LogError( "NextWaveNow should not be called anymore." );
	}

	private void SpawnNewEnemy( int id, Transform spawnPoint )
	{
		Vector3 enemyPos = Vector3.zero;
		Vector2 circle = Random.insideUnitCircle * radius;
		enemyPos.x = spawnPoint.position.x + circle.x;
		enemyPos.y = spawnPoint.position.y;
		enemyPos.z = spawnPoint.position.z + circle.y;

		Instantiate( enemyPrefabs[id], enemyPos, Quaternion.identity );
	}

	private void StartWaves( )
	{
		coroutine = StartCoroutine( SpawnWaves( ) );
	}

	private IEnumerator SpawnWaves( )
	{
		// Waiting till we build a Castle
		while ( !AIProgressManager.Instance.FistTheasholdReached )
			yield return new WaitForSeconds( 1 );

		// Waiting between waves
		float waveOffset = waveDelay * ( spawnDelayMaxOffsetPercent / 100 );
		yield return new WaitForSeconds( waveDelay + Random.Range( -waveOffset, waveOffset ) );

		// Pause if we enter Creative Mode
		while ( LevelManager.Instance.CreativeMode )
			yield return new WaitForSeconds( 1 );

		// Next wave
		MessageService.Instance.ShowMessage( "New enemies approaching...", 1f, Color.red );
		waveNum++;

		// Spawn enemies in current wave
		int enemyNumOffset = Mathf.CeilToInt( enemiesInWave * ( enemiesInWaveMaxOffsetPercent / 100 ) );
		int enemisToSpawn = (int)( enemiesInWave + Random.Range( -enemyNumOffset, enemyNumOffset ) );
		for ( int i = 0; i < enemisToSpawn; i++ )
		{
			// Wait between spawning each enemy
			float enemyDelayOffet = delayBetweenEnemies * ( delayBetweenEnemiesMaxOffsetPercent / 100 );
			yield return new WaitForSeconds( delayBetweenEnemies + Random.Range( -enemyDelayOffet, enemyDelayOffet ) );

			// Pause if we enter Creative Mode
			while ( LevelManager.Instance.CreativeMode )
				yield return new WaitForSeconds( 1 );

			// Pick enemy using set probabilities
			int enemyID = 0;
			float rnd = Random.Range( 0f, 1f );
			float sum = 0;
			for ( int j = 0; j < enemyTypePercentChance.Length; j++ )
			{
				sum += enemyTypePercentChance[j];
				if ( sum >= rnd )
				{
					enemyID = j;
					break;
				}
			}

			// Pick spawn point using set probabilities
			int spawnPointID = 0;
			rnd = Random.Range( 0f, 1f );
			sum = 0;
			for ( int j = 0; j < spawnPointIDPercentChance.Length; j++ )
			{
				sum += spawnPointIDPercentChance[j];
				if ( sum >= rnd )
				{
					spawnPointID = j;
					break;
				}
			}

			SpawnNewEnemy( enemyID, spawnPoints[spawnPointID] );
		}

		if ( !spawningLastWave )
			coroutine = StartCoroutine( SpawnWaves( ) );
		else
			EndOfWaves = true;
	}

	public void LastThresholdReached( )
	{
		spawningLastWave = true;
	}

	public void ChangeParametersOnThresholdChange( WaveParameters newParameters )
	{
		float difficulty = LevelManager.Instance.LevelDifficultyModifier;

		waveDelay = newParameters.WaveDelay * difficulty;
		spawnDelayMaxOffsetPercent = newParameters.SpawnDelayMaxOffsetPercent * difficulty;
		delayBetweenEnemies = newParameters.DelayBetweenEnemies * difficulty;
		delayBetweenEnemiesMaxOffsetPercent = newParameters.DelayBetweenEnemiesMaxOffsetPercent * difficulty;
		enemiesInWave = newParameters.EnemiesInWave * difficulty;
		enemiesInWaveMaxOffsetPercent = newParameters.EnemiesInWaveMaxOffsetPercent * difficulty;
		enemyTypePercentChance[0] = newParameters.EnemyTypePercentChance[0] * difficulty;
		enemyTypePercentChance[1] = newParameters.EnemyTypePercentChance[1] * difficulty;
		enemyTypePercentChance[2] = newParameters.EnemyTypePercentChance[2] * difficulty;
		spawnPointIDPercentChance[0] = newParameters.SpawnPointIDPercentChance[0] * difficulty;
		spawnPointIDPercentChance[1] = newParameters.SpawnPointIDPercentChance[1] * difficulty;
		spawnPointIDPercentChance[2] = newParameters.SpawnPointIDPercentChance[2] * difficulty;
	}

	public void ChangeParametersOnThreatChange( WaveParameters parametersIncrease, int increase )
	{
		float difficulty = LevelManager.Instance.LevelDifficultyModifier;

		waveDelay += parametersIncrease.WaveDelay * increase;
		spawnDelayMaxOffsetPercent += parametersIncrease.SpawnDelayMaxOffsetPercent * increase * difficulty;
		delayBetweenEnemies += parametersIncrease.DelayBetweenEnemies * increase * difficulty;
		delayBetweenEnemiesMaxOffsetPercent += parametersIncrease.DelayBetweenEnemiesMaxOffsetPercent * increase * difficulty;
		enemiesInWave += parametersIncrease.EnemiesInWave * increase * difficulty;
		enemiesInWaveMaxOffsetPercent += parametersIncrease.EnemiesInWaveMaxOffsetPercent * increase * difficulty;
		enemyTypePercentChance[0] += parametersIncrease.EnemyTypePercentChance[0] * increase * difficulty;
		enemyTypePercentChance[1] += parametersIncrease.EnemyTypePercentChance[1] * increase * difficulty;
		enemyTypePercentChance[2] += parametersIncrease.EnemyTypePercentChance[2] * increase * difficulty;
		spawnPointIDPercentChance[0] += parametersIncrease.SpawnPointIDPercentChance[0] * increase * difficulty;
		spawnPointIDPercentChance[1] += parametersIncrease.SpawnPointIDPercentChance[1] * increase * difficulty;
		spawnPointIDPercentChance[2] += parametersIncrease.SpawnPointIDPercentChance[2] * increase * difficulty;
	}
}
