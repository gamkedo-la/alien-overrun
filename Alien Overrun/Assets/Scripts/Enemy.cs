/**
 * Description: Core enemy functionality.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float speed = 10;

	private Vector3 destination = Vector3.zero;
	private Rigidbody rb = null;
	private bool onTheMove = true;

	void Start ()
	{
		rb = GetComponent<Rigidbody>( );
		Assert.IsNotNull( rb );
	}

	void Update ()
	{
		Rotate( );
		Move( );
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

	public void SetDestination( Vector3 destination ) => this.destination = destination;

	public void IsMoving( bool isMoving ) => onTheMove = isMoving;

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
