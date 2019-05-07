/**
 * Description: Core enemy functionality.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

[RequireComponent(typeof(OponentFinder))]
public class Enemy : AbstractListableItem
{
	[SerializeField] private NavMeshAgent agent = null;
	[SerializeField] private GroundDetect detector = null;
	[SerializeField] private float minPhysicsReactVelSqr = 3f;
	[SerializeField] private float getUpSpeed = 0.1f;
	[SerializeField] private float getUpTimeMax = 2f;
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
		Assert.IsNotNull( detector );

		destination = BuildingManager.Instance.GetNearestCoreOrZero( transform.position );
		agent.SetDestination( destination );

		OponentFinder oponentFinder = gameObject.GetComponent<OponentFinder>( );
		oponentFinder.SetOponentListManager( BuildingManager.Instance );
	}

	void OnEnable( )
	{
		EnemyManager.Instance.AddItem( this );
	}

	void OnDisable( )
	{
		if ( EnemyManager.Instance )
			EnemyManager.Instance.RemoveItem( this );
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

		if ( isDynamic )
			return;

		agent.isStopped = false;
		agent.SetDestination( destination );
	}

	public void HoldPosition( )
	{
		if ( isDynamic )
			return;

		agent.isStopped = true;
	}

	public void OnDeath( )
	{
		if ( ResourceManager.Instance && mineralsForKill != 0 )
		{
			ResourceManager.Instance.AddResources( ResourceType.Minerals, mineralsForKill );
			Utilities.DrawDebugText( transform.position + Vector3.up * 2, "+" + mineralsForKill.ToString( ), 12, Color.green );
		}
	}

	private void OnCollisionEnter( Collision collision )
	{
		if ( collision.relativeVelocity.sqrMagnitude <= minPhysicsReactVelSqr )
			return;

		DisableNavMesh( );
		rb.AddForce( collision.contacts[0].normal * -collision.relativeVelocity.sqrMagnitude * 20 );
	}

	public void DisableNavMesh( )
	{
		if ( isDynamic )
			return;

		StopCoroutine( FlipBack( ) );

		if ( agent.isOnNavMesh && !agent.isStopped )
			agent.isStopped = true;

		agent.velocity = Vector3.zero;
		agent.enabled = false;
		rb.isKinematic = false;
		isDynamic = true;

		InvokeRepeating( "CheckEnableNavMesh", 1, 1 );
	}

	private void CheckEnableNavMesh()
	{
		if ( isDynamic && rb.velocity.sqrMagnitude <= thresholdForNavMeshReEnable && detector.IsOnGround )
		{
			CancelInvoke( "CheckEnableNavMesh" );
			StartCoroutine( FlipBack( ) );
		}
	}

	private void EnableNavMesh( )
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		agent.enabled = enabled;
		rb.isKinematic = true;
		agent.isStopped = false;
		agent.SetDestination( destination );
		isDynamic = false;
	}

	private IEnumerator FlipBack()
	{
		RaycastHit[] hits = Physics.RaycastAll( new Ray( transform.position, Vector3.down ), 5 );
		Vector3? point = null;
		foreach ( var hit in hits )
		{
			if ( hit.collider.gameObject.CompareTag( Tags.Environment ) )
			{
				point = hit.point;
				break;
			}
		}

		if ( point != null )
		{
			Vector3 goodPosition = (Vector3)point;
			Quaternion goodRotation = Quaternion.Euler( 0, transform.localEulerAngles.y, 0 );

			float breakTime = getUpTimeMax;

			while ( Vector3.Distance( transform.position, goodPosition) > 0.2f &&
					Quaternion.Angle( transform.localRotation, goodRotation ) > 10f &&
					breakTime > 0)
			{
				breakTime -= Time.fixedDeltaTime;
				Quaternion newRotation = Quaternion.Lerp( transform.localRotation, goodRotation, getUpSpeed );
				Vector3 newPosition = Vector3.Lerp( transform.position, goodPosition, getUpSpeed );

				rb.MoveRotation( newRotation );
				rb.MovePosition( newPosition );

				yield return new WaitForFixedUpdate( );
			}
		}

		EnableNavMesh( );
	}
}
