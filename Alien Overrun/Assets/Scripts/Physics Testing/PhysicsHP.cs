/**
 * Description: HP for physics entities.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhysicsHP : MonoBehaviour
{
	public float CurrentHP { get; private set; }
	public float MaxHP { get { return maxHP; } }

	[Header("External objects")]
	[SerializeField, Tooltip("An optional Slider that acts as a HP Bar.")] private Slider hpBar = null;
	[SerializeField] private GameObject explosion = null;

	[Header("Tweakable")]
	[SerializeField] private float maxHP = 10;
	[SerializeField] private float hpLosePerVelSqr = 0.01f;
	[SerializeField] private float minDamageVelSqr = 50f;
	[SerializeField] private bool destroyOnNoHP = false;
	[SerializeField] private ResistanceType resistance = ResistanceType.Normal;
	public ResistanceType Resistance { get { return resistance; } private set { resistance = value; } }

	[Header("Events")]
	[SerializeField] private UnityEvent onHealthChange = null;
	[SerializeField] private UnityEvent onDeath = null;

	void Start( )
	{
		CurrentHP = maxHP;

		if ( hpBar )
		{
			hpBar.maxValue = maxHP;
			hpBar.value = maxHP;
		}
	}

	private void OnCollisionEnter( Collision other )
	{
		if ( other.relativeVelocity.sqrMagnitude <= minDamageVelSqr )
			return;

		float damage = other.relativeVelocity.sqrMagnitude * hpLosePerVelSqr;
		//Debug.Log( other.relativeVelocity.sqrMagnitude );
		//Debug.Log( damage );
		ChangeHP( -damage );
	}

	/// <summary>
	/// Changes current HP. Respects HP restrictions and fires events if necessary.
	/// </summary>
	/// <param name="change">Amount of HP change. Negative values for damage, positive for healing.</param>
	public void ChangeHP( float change )
	{
		CurrentHP += change;
		CurrentHP = CurrentHP > maxHP ? maxHP : CurrentHP;
		CurrentHP = CurrentHP < 0 ? 0 : CurrentHP;

		onHealthChange.Invoke( );

		if ( hpBar )
			hpBar.value = CurrentHP;

		if ( CurrentHP <= 0 )
			DestroyMe( );
	}

	private void DestroyMe( )
	{
		onDeath.Invoke( );

		if ( destroyOnNoHP )
		{
			Instantiate( explosion, transform.position, Quaternion.identity );
			Destroy( gameObject );
		}
	}
}
