/**
 * Description: Core enemy functionality.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class Enemy : MonoBehaviour
{
	[SerializeField] private NavMeshAgent agent = null;
	[SerializeField] private int mineralsForKill = 20;

	private Vector3 destination = Vector3.zero;
	private Rigidbody rb = null;

	void Start ()
	{
		rb = GetComponent<Rigidbody>( );
		Assert.IsNotNull( rb );
		Assert.IsNotNull( agent );

		destination = BuildingManager.Instance.GetNearestCoreOrZero( transform.position );
		agent.SetDestination( destination );
	}

	void OnEnable( )
	{
		EnemyManager.Instance.AddEnemy( this );
	}

	void OnDisable( )
	{
		if ( EnemyManager.Instance )
			EnemyManager.Instance.RemoveEnemy( this );
	}

	public void SetDestination( )
	{
		SetDestination( BuildingManager.Instance.GetNearestCoreOrZero( transform.position ) );
	}

	public void SetDestination( Transform target )
	{
		SetDestination( target.position );
	}

	public void SetDestination( Vector3 destination )
	{
		this.destination = destination;

		agent.isStopped = false;
		agent.SetDestination( destination );
	}

	public void HoldPosition( )
	{
		agent.isStopped = true;
	}

	public void OnDeath( )
	{
		if ( ResourceManager.Instance )
			ResourceManager.Instance.AddResources( ResourceType.Minerals, mineralsForKill );

		Utilities.DrawDebugText( transform.position + Vector3.up * 2, "+" + mineralsForKill.ToString( ), 12, Color.green );
	}
}
