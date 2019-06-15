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
	[SerializeField] private GameObject[] spikes = null;
	[SerializeField] private NavMeshAgent agent = null;
	[SerializeField] private GroundDetect detector = null;
	[SerializeField] private float minPhysicsReactVelSqr = 3f;
	[SerializeField] private float getUpSpeed = 0.1f;
	[SerializeField] private float getUpTimeMax = 2f;
	[SerializeField] private int mineralsForKill = 20;
	[SerializeField] private float maxVelocityMag = 150f;
	[SerializeField] private float thresholdForNavMeshReEnable = 10f;
	[SerializeField] private float timeToDestroyOnNotMoving2 = 60f;
	[SerializeField] private float knockBackDampening = 0.95f;
	[SerializeField] private int bloodCount = 1;
	[SerializeField] private float bloodSpread = 0.5f;

	[Tooltip("The chance of this unit becoming a stronger one, between 0 and 100 percent.")]
	[Range(0, 100)] [SerializeField] private float eliteChance = 10f;

	public string DebugInfo = "";

	private float timeToDestroyOnNotMovingCurrent = 60f;
	private Vector3 destination = Vector3.zero;
	private Vector3 oldPos = Vector3.zero;
	private Rigidbody rb = null;
	private bool isDynamic = false;
	private bool hold = false;

	private Vector3 knockBackVel = Vector3.zero;

	void Start ()
	{
		timeToDestroyOnNotMovingCurrent = timeToDestroyOnNotMoving2;

		rb = GetComponent<Rigidbody>( );
		Assert.IsNotNull( rb );
		Assert.IsNotNull( agent );
		Assert.IsNotNull( detector );

		SetDestination( );

		OponentFinder oponentFinder = gameObject.GetComponent<OponentFinder>( );
		oponentFinder.SetOponentListManager( BuildingManager.Instance );

		Invoke( "OnDeath", 7 * 60 );

		if ( Random.value <= ( eliteChance / 100 ) )
		{
			HP hp = gameObject.GetComponent<HP>( );
			hp.MakeEliteUnit( );
			transform.localScale *= 1.3f;

			foreach ( var s in spikes )
				s.SetActive( true );
		}
		else
		{
			foreach ( var s in spikes )
				s.SetActive( false );
		}
	}

	void FixedUpdate( )
	{
		if ( isDynamic && rb.velocity.magnitude >  maxVelocityMag)
		{
			//Debug.Log( "E " + rb.velocity.magnitude );
			rb.velocity = rb.velocity.normalized * maxVelocityMag;
		}

		// Just in case enemy drops outside of the map
		if ( transform.position.y < -1000f )
			Destroy( gameObject );
		// ...or is immobile
		if ( oldPos == transform.position && !hold )
			timeToDestroyOnNotMovingCurrent -= Time.fixedDeltaTime;
		else
		{
			oldPos = transform.position;
			timeToDestroyOnNotMovingCurrent = timeToDestroyOnNotMoving2;
		}

			// knockback from being hit
			if (knockBackVel.magnitude > 0.001f) {
			//Debug.Log("KNOCKBACK USING TRANSFORM!");
			transform.position = transform.position + knockBackVel;
			knockBackVel = knockBackVel * knockBackDampening;
		}

		if (timeToDestroyOnNotMovingCurrent <= 0)
		{
			OnDeath( );
			Destroy( gameObject );
		}
	}

	public void knockBack(Vector3 kvel) {
		//Debug.Log("STARTING KNOCKBACK USING TRANSFORM!");
		knockBackVel = kvel;
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
		string targetName = "";
		(destination, targetName) = BuildingManager.Instance.GetNearestCoreCastleOrZero( transform.position );
		DebugInfo = $"Destination: {targetName}";
		SetDestination( destination );
	}

	public void SetDestination( Transform target )
	{
		DebugInfo = $"Destination: {target.name}";
		SetDestination( target.position );
	}

	private void SetDestination( Vector3 destination )
	{
		this.destination = destination;
		hold = false;

		if ( isDynamic )
			return;

		agent.isStopped = false;
		agent.SetDestination( destination );
	}

	public void HoldPosition( )
	{
		if ( isDynamic )
			return;

		hold = true;
		agent.isStopped = true;
	}

	public void OnDeath( )
	{
		if ( ResourceManager.Instance && mineralsForKill != 0 )
		{
			ResourceManager.Instance.AddResources( ResourceType.Minerals, mineralsForKill );
			FloatingTextService.Instance.ShowFloatingText( transform.position + Vector3.up * 2, "+" + mineralsForKill.ToString( ), 1, Color.green, 1f );
		}

		if ( transform.position.y < 3f )
			BloodMarkManager.Instance.PutAt( transform.position, bloodCount, bloodSpread );
	}

	private void OnCollisionEnter( Collision collision )
	{
		if ( collision.relativeVelocity.sqrMagnitude <= minPhysicsReactVelSqr )
			return;

		DisableNavMesh( );
		rb.AddForce( collision.contacts[0].normal * -collision.relativeVelocity.sqrMagnitude * 20 * PauseGame.ForceScale );
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
