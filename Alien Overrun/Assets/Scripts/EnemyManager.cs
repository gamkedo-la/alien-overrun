/**
 * Description: Manages enemies, spawns new ones, provides functionality for other scripts.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class Wave
{
	public int ID;
	public float WaveDelay = 2f;
	public float EnemiesDelay = 1f;
	public int[] Enemies;
	public int SpawnPointID;
}

[System.Serializable]
public class Waves
{
	public List<Wave> EnemyWaves;
}

public class EnemyManager : MonoBehaviour
{
	private const string myWaves = @"
{
    ""EnemyWaves"":
	[
        {
			""ID"": 1,
            ""WaveDelay"": 5.0,
            ""EnemiesDelay"": 0.5,
            ""Enemies"": [ 0, 0, 0 ],
			""SpawnPointID"": 0
		},
		{
			""ID"": 2,
            ""WaveDelay"": 6.0,
            ""EnemiesDelay"": 0.5,
            ""Enemies"": [ 0, 0, 0 ],
			""SpawnPointID"": 1
		},
		{
			""ID"": 3,
            ""WaveDelay"": 6.0,
            ""EnemiesDelay"": 0.5,
            ""Enemies"": [ 0, 0, 0 ],
			""SpawnPointID"": 2
		},
		{
			""ID"": 4,
            ""WaveDelay"": 15.0,
            ""EnemiesDelay"": 0.5,
            ""Enemies"": [ 1, 1 ],
			""SpawnPointID"": 0
		},
		{
			""ID"": 5,
            ""WaveDelay"": 15.0,
            ""EnemiesDelay"": 0.5,
            ""Enemies"": [ 1, 1, 0, 0 ],
			""SpawnPointID"": 1
		},
		{
			""ID"": 6,
            ""WaveDelay"": 5.0,
            ""EnemiesDelay"": 1.0,
            ""Enemies"": [ 2, 0, 0 ],
			""SpawnPointID"": 2
		},
		{
			""ID"": 7,
            ""WaveDelay"": 20.0,
            ""EnemiesDelay"": 1.0,
            ""Enemies"": [ 2, 2, 1, 1, 0, 0 ],
			""SpawnPointID"": 1
		},
		{
			""ID"": 8,
            ""WaveDelay"": 15.0,
            ""EnemiesDelay"": 0.5,
            ""Enemies"": [ 2, 2, 1, 1, 1, 1 ],
			""SpawnPointID"": 0
		},
		{
			""ID"": 9,
            ""WaveDelay"": 20.0,
            ""EnemiesDelay"": 1.5,
            ""Enemies"": [ 1, 1, 0, 0, 2, 2, 1, 1, 0, 0, 1, 1 ],
			""SpawnPointID"": 1
		},
		{
			""ID"": 10,
            ""WaveDelay"": 1.0,
            ""EnemiesDelay"": 1.5,
            ""Enemies"": [ 1, 1, 0, 0, 2, 2, 1, 1, 0, 0, 1, 1 ],
			""SpawnPointID"": 1
		}
    ]
}";

	public static EnemyManager Instance { get; private set; }

	public List<Enemy> Enemies { get; private set; }

	public bool EndOfWaves { get { return endOfWaves; } set { endOfWaves = value; } }
	private bool endOfWaves = false;

	[SerializeField] private GameObject[] enemyPrefabs = null;
	[SerializeField] private Transform crativeModeSpawnPoint = null;
	[SerializeField] private Transform[] spawnPoints = null;
	[SerializeField] private TextMeshProUGUI enemyCount = null;
	[SerializeField] private float radius = 3f;
	[SerializeField] private float autoSpawningDelay = 0.1f;

	private Waves waves;
	private Wave currentWave;
	private int currentWaveIndex = 0;
	private Coroutine coroutine;
	private bool autoSpawning = false;

	private void Awake( )
	{
		Enemies = new List<Enemy>( );

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

	public void AddEnemy( Enemy enemy )
	{
		Enemies.Add( enemy );
		enemyCount.text = "Enemy count: " + Enemies.Count;
	}

	public void RemoveEnemy( Enemy enemy )
	{
		Enemies.Remove( enemy );
		enemyCount.text = "Enemy count: " + Enemies.Count;
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
		StopCoroutine( coroutine );
		currentWave.WaveDelay = 0;
		coroutine = StartCoroutine( SpawnWaves( ) );
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
		waves = JsonUtility.FromJson<Waves>( myWaves );

		currentWave = waves.EnemyWaves[currentWaveIndex];
		coroutine = StartCoroutine( SpawnWaves( ) );
	}

	private IEnumerator SpawnWaves( )
	{
		yield return new WaitForSeconds( currentWave.WaveDelay );

		MessageService.Instance.ShowMessage( "New enemies approaching...", 1f, Color.red );

		for ( int i = 0; i < currentWave.Enemies.Length; i++ )
		{
			yield return new WaitForSeconds( currentWave.EnemiesDelay );

			while ( LevelManager.Instance.CreativeMode )
				yield return new WaitForSeconds( 1 );

			SpawnNewEnemy( currentWave.Enemies[i], spawnPoints[currentWave.SpawnPointID] );
		}

		currentWaveIndex++;
		if ( currentWaveIndex < waves.EnemyWaves.Count )
		{
			currentWave = waves.EnemyWaves[currentWaveIndex];
			// TODO: Make button active.
			coroutine = StartCoroutine( SpawnWaves( ) );
		}
		else
		{
			endOfWaves = true;
			//Debug.Log( "End of waves!" );
		}
	}
}
