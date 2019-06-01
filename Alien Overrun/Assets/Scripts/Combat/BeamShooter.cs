/**
 * Description: Beam shooter component of Towers - shoots a beam.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class BeamShooter : MonoBehaviour
{
	[SerializeField] private Transform shootPoint = null;
	[SerializeField] private GameObject muzzleFlashPrefab = null;
	[SerializeField] private float knockBackForce = 0.1f;
	[SerializeField] private float reloadTime = 2f;
	[SerializeField] private float shootDuration = 1f;
	[SerializeField] private float damage = 5f;
	[SerializeField] private Color shotColor = Color.red;
	[SerializeField] private DamageType damageType = DamageType.Magical;

	private Transform target = null;
	private HP targetHP = null;
	private float timeToNextShot = 0;

	void Start( )
	{
		Assert.IsNotNull( shootPoint );
	}

	void Update( )
	{
		timeToNextShot -= Time.deltaTime;

		if ( target && targetHP && timeToNextShot <= 0 )
		{
			timeToNextShot = reloadTime;

			Utilities.DrawLine( shootPoint.position, target.position, shotColor, 0.1f, shootDuration );

			float damg = damage * Interactions.GetMultiplier( damageType, targetHP.Resistance );

			targetHP.ChangeHP( -damg );
			FloatingTextService.Instance.ShowFloatingText( target.position + Vector3.up, damg.ToString( ) );

			// muzzle flash particle effect
			if (muzzleFlashPrefab) {
				Vector3 dir = ( target.position - shootPoint.position ).normalized;
				var muzzleFlash = Instantiate( muzzleFlashPrefab, shootPoint.position, Quaternion.LookRotation(dir));
                FMODUnity.RuntimeManager.PlayOneShot("event:/Mage_Tower_Attacking");
            }

			Enemy baddie = target.gameObject.GetComponent<Enemy>( );
			if (baddie)
			{
				/*float knockBackForce = 0.25f; // in decreasing game units per frame
				Vector3 knockBackVec = new Vector3(
					Random.Range(-1 * knockBackForce, knockBackForce),
					Random.Range(knockBackForce / 2, knockBackForce),
					Random.Range(-1 * knockBackForce, knockBackForce));*/
				Vector3 dir = ( target.position - transform.position ).normalized;
				Vector3 knockBackVec = dir * knockBackForce;
				baddie.knockBack(knockBackVec);
			}

			/*
			// knockback - UNUSED: physics can't affect enemies or navmesh breaks
			Rigidbody rb = target.gameObject.GetComponent<Rigidbody>( );
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
	}

	public void OnNewOponenet ( Transform oponent )
	{
		target = oponent;
		targetHP = target.GetComponent<HP>( );
		//Debug.Log( name + " has new oponent: " + oponent.name );
	}

	public void OnOponenetLost ( )
	{
		target = null;
		targetHP = null;
		//Debug.Log( name + " lost oponent" );
	}
}
