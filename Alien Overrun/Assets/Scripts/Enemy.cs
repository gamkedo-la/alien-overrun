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
	[SerializeField] private float speed = 10;

	private Vector3 destination = Vector3.zero;
	private Rigidbody rb = null;
	private bool onTheMove = true;

	void Start ()
	{
		rb = GetComponent<Rigidbody>( );
		Assert.IsNotNull( rb );
		Assert.IsNotNull( agent );

		destination = BuildingManager.Instance.GetNearestCoreOrZero( transform.position );
		agent.SetDestination( destination );
	}

	void Update ()
	{
		//Rotate( );
		//Move( );
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
		agent.SetDestination( destination );
	}

	public void IsMoving( bool isMoving )
	{
		onTheMove = isMoving;
		agent.isStopped = isMoving;
	}

	private void Rotate( )
	{
		if ( !onTheMove )
			return;

		transform.LookAt( destination );
	}

	private void Move()
	{
		if ( !onTheMove )
			return;

		Vector3 dir = ( destination - rb.position ).normalized;
		rb.MovePosition( rb.position + dir * speed * Time.deltaTime );
	}
}
