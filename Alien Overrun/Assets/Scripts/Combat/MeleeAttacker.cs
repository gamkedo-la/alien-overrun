/**
 * Description: Script for Melee Attacks.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class MeleeAttacker : MonoBehaviour
{
	[SerializeField] private GameObject effect = null;
	[SerializeField] private float effectOffsetZ = 1f;
	[SerializeField] private float effectOffsetY = 1f;
	[SerializeField] private Animator animator = null;
	[SerializeField] private float attackDelay = 2f;
	[SerializeField] private int damage = 10;
	[SerializeField] private int ammoCost = 0;
	[SerializeField] private bool autoApplyDamage = false;
	[SerializeField] private float applyDamageDelay = 1.04f;
	[SerializeField] private DamageType damageType = DamageType.Physical;

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

		HP hp = target.GetComponent<HP>( );
		float damg = damage * Interactions.GetMultiplier( damageType, hp.Resistance );
		Instantiate( effect, transform.position + transform.forward * effectOffsetZ + transform.up * effectOffsetY, Quaternion.identity );

		hp.ChangeHP( -damg );
		FloatingTextService.Instance.ShowFloatingText( target.position + Vector3.up, damg.ToString( ) );
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
		if (autoApplyDamage)
			Invoke( "ApplyDamage", applyDamageDelay );
	}
}
