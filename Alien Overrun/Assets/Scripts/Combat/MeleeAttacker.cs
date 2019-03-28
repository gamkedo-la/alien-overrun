/**
 * Description: Script for Melee Attacks.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class MeleeAttacker : MonoBehaviour
{
	[SerializeField] private Animator animator = null;
	[SerializeField] private float attackDelay = 2f;
	[SerializeField] private int damage = 10;
	[SerializeField] private int ammoCost = 0;

	private float timeToNextAttack = 0f;
	private Transform target = null;

	void Start( )
	{
		Assert.IsNotNull( animator );
	}

	void Update( )
	{
		timeToNextAttack -= Time.deltaTime;

		TryToAttack( );
	}

	public void OnNewOponenet( Transform oponent )
	{
		target = oponent;
		transform.LookAt( oponent );
	}

	public void OnOponenetLost( )
	{
		target = null;
	}

	public void ApplyDamage()
	{
		if ( !target )
			return;

		target.GetComponent<HP>( ).ChangeHP( -damage );
		Utilities.DrawDebugText( target.position + Vector3.up, damage.ToString( ) );
	}

	private void TryToAttack( )
	{
		if ( target && timeToNextAttack <= 0 && HasAmmo( ) )
		{
			timeToNextAttack = attackDelay;
			StartAttack( );
		}
	}

	private bool HasAmmo( )
	{
		return ResourceManager.Instance.CheckResources( ResourceType.Minerals, ammoCost );
	}

	private void StartAttack( )
	{
		if ( !target )
		{
			Debug.LogError( "Tried shooting but enemy = null" );
			return;
		}

		ResourceManager.Instance.UseResources( ResourceType.Minerals, ammoCost );
		animator.SetTrigger( "Attack" );
	}
}
