/**
 * Description: Controls arrows that are shot by towers.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class Arrow : MonoBehaviour
{
	[SerializeField] private GameObject line = null;
	[SerializeField] private int damage = 20;
	[SerializeField] private float knockBackForce = 0.1f;
	[SerializeField] private DamageType damageType = DamageType.Physical;

	private Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>( );

		Assert.IsNotNull( rb );
		Assert.IsNotNull( line );

		Destroy( gameObject, 10f ); // Emergency cleanup
	}

	void Update ()
	{
		if ( rb && rb.velocity != Vector3.zero )
			transform.rotation = Quaternion.LookRotation( rb.velocity );
	}

	private void OnTriggerEnter( Collider other )
	{
		//if ( collision.transform.CompareTag( Tags.Environment ) )
		//IamStuck( collision.transform );

		if ( other.transform.CompareTag( Tags.Enemy ) )
		{
			HP hp = other.gameObject.GetComponent<HP>( );
			if ( hp != null )
			{
				float damg = damage * Interactions.GetMultiplier( damageType, hp.Resistance );
				hp.ChangeHP( -damg );
				FloatingTextService.Instance.ShowFloatingText( other.transform.position + Vector3.up, damg.ToString( ) );
			}

			Enemy baddie = other.gameObject.GetComponent<Enemy>( );
			if (baddie)
			{
				/*Vector3 knockBackVec = new Vector3(
					Random.Range(-1 * knockBackForce, knockBackForce),
					Random.Range(knockBackForce / 2, knockBackForce),
					Random.Range(-1 * knockBackForce, knockBackForce));*/
				if ( Random.Range( 0f, 1f ) <= 0.1f )
				{
					Vector3 knockBackVec = other.gameObject.transform.forward * -knockBackForce * Random.Range( 0f, 1f );
					baddie.knockBack( knockBackVec );
				}
			}

			// knockback - UNUSED: physics can't affect enemies or navmesh breaks
			/*
			Rigidbody rb = other.gameObject.GetComponent<Rigidbody>( );
			if (rb != null && knockBackForce > 0f) {
				// for proper directionality: maybe something like
				// other.contacts[0].point - other.gameObject.transform.position
				Vector3 knockBackVec = new Vector3(
					Random.Range(-1 * knockBackForce, knockBackForce),
					Random.Range(knockBackForce / 2, knockBackForce),
					Random.Range(-1 * knockBackForce, knockBackForce));
				rb.isKinematic = false;
				//Debug.Log("KNOCKBACK vec: " + knockBackVec);
				rb.AddForce(knockBackVec);//, ForceMode.VelocityChange);
				// FIXME: need a one time coroutine with the rb's scope maintained
				// to reset back to normal kinematic control after a short time
				//Invoke(regainKinematicControl,3f); // rb.isKinematic = true;
			}
			*/
		}

		IamStuck( other.transform );
	}

	private void IamStuck ( Transform collider )
	{
		//GetComponent<Rigidbody>( ).velocity = Vector3.zero;
		//GetComponent<Rigidbody>( ).isKinematic = true;
		Destroy( GetComponent<Rigidbody>( ) );
		Destroy( GetComponent<BoxCollider>( ) );

		transform.SetParent( collider );
		Destroy( line );
		Destroy( gameObject, 5f ); // TODO: Make it disappear over few seconds?
	}
}
