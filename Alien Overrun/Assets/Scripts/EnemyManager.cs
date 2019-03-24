/**
 * Description: Manages enemies, spawns new ones, provides functionality for other scripts.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager Instance { get; private set; }

	public List<Enemy> Enemies { get; private set; }

	[SerializeField] private GameObject enemy = null;
	[SerializeField] private Transform spawnCenter = null;
	[SerializeField] private float radius = 3f;

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
		Assert.IsNotNull( enemy );
	}

	public void AddEnemy( Enemy enemy )
	{
		Enemies.Add( enemy );
	}

	public void RemoveEnemy( Enemy enemy )
	{
		Enemies.Remove( enemy );
	}

	public void SpawnNewEnemy( )
	{
		Vector3 enemyPos = Vector3.zero;
		Vector2 circle = Random.insideUnitCircle * radius;
		enemyPos.x = spawnCenter.position.x + circle.x;
		enemyPos.y = spawnCenter.position.y;
		enemyPos.z = spawnCenter.position.z + circle.y;

		Instantiate( enemy, enemyPos, Quaternion.identity );
	}
}
