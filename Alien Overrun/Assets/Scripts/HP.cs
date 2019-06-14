/**
 * Description: Universal Health script with basic utility functions.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
	public float CurrentHP { get; private set; }
	public float MaxHP { get { return maxHP; } }

	[Header("External objects")]
	[SerializeField, Tooltip("An optional Slider that acts as a HP Bar.")] private Slider hpBar = null;
	[SerializeField, Tooltip("An optional Prefab that acts as a corpse when this unit dies.")] private GameObject corpse = null;

	[Header("Tweakable")]
	[SerializeField] private float maxHP = 10;
	[SerializeField] private bool destroyOnNoHP = false;
	[SerializeField] private ResistanceType resistance = ResistanceType.Normal;
	public ResistanceType Resistance { get { return resistance; } private set { resistance = value; } }

	[Header("Events")]
	[SerializeField] private UnityEvent onHealthChange = null;
	[SerializeField] private UnityEvent onDeath = null;

	void Awake( )
	{
		CurrentHP = maxHP;
	}

	void Start( )
	{
		SetHPBar( );
	}

	void SetHPBar( )
	{
		if ( hpBar )
		{
			hpBar.maxValue = maxHP;
			hpBar.value = maxHP;

			SetHPBarVisability( );
		}
	}

	public void MakeEliteUnit( )
	{
		maxHP *= 2f;
		CurrentHP = maxHP;

		SetHPBar( );
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
        if (gameObject.tag == "Building")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Buildings/BuildingDamage");
        }

        if ( hpBar )
		{
			hpBar.value = CurrentHP;
			SetHPBarVisability( );
		}

		if ( CurrentHP <= 0 )
			DestroyMe( );
	}

	private void DestroyMe( )
	{
		onDeath.Invoke( );

		if ( destroyOnNoHP )
			Destroy( gameObject );

		if ( corpse != null)
			Instantiate(corpse, gameObject.transform.position, gameObject.transform.rotation);
	}

	private void SetHPBarVisability( )
	{
		if ( CurrentHP == maxHP )
			hpBar.transform.parent.gameObject.SetActive( false );
		else
			hpBar.transform.parent.gameObject.SetActive( true );
	}
}
