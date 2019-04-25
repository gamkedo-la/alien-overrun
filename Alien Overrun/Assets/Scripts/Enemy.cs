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
	[SerializeField] private float thresholdForNavMeshReEnable = 10f;

	private Vector3 destination = Vector3.zero;
	private Rigidbody rb = null;
	private bool isDynamic = false;

	void Start ()
	{
		rb = GetComponent<Rigidbody>( );
		Assert.IsNotNull( rb );
		Assert.IsNotNull( agent );

		destination = BuildingManager.Instance.GetNearestCoreOrZero( transform.position );
		agent.SetDestination( destination );
	}

	/*void FixedUpdate( )
	{
		if ( isDynamic && rb.velocity.sqrMagnitude <= thresholdForNavMeshReEnable )
			EnableNavMesh( );
	}*/

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

	public void DisableNavMesh( )
	{
		Debug.Log( "DisableNavMesh: " + name );
		agent.isStopped = true;
		agent.velocity = Vector3.zero;
		rb.isKinematic = false;
		isDynamic = true;

		InvokeRepeating( "CheckEnableNavMesh", 1, 1 );
	}

	private void CheckEnableNavMesh()
	{
		if ( isDynamic && rb.velocity.sqrMagnitude <= thresholdForNavMeshReEnable )
		{
			CancelInvoke( "CheckEnableNavMesh" );
			EnableNavMesh( );
		}
	}

	private void EnableNavMesh( )
	{
		Debug.Log( "EnableNavMesh: " + name );
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		rb.isKinematic = true;
		agent.isStopped = false;
		isDynamic = false;
	}
}
