/**
 * Description: Controls smoke intensity.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class Smoke : MonoBehaviour
{
	[SerializeField] private ParticleSystem ps = null;
	[SerializeField] private HP hp = null;
	[SerializeField] private Gradient gradient;

	private ParticleSystem.MainModule main;
	private ParticleSystem.EmissionModule emision;

	void Start ()
	{
		Assert.IsNotNull( ps, $"Please assign <b>Ps</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
		Assert.IsNotNull( hp, $"Please assign <b>HP</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );

		main = ps.main;
		emision = ps.emission;
		OnChange( );
	}

	public void OnChange ()
	{
		main.startColor = gradient.Evaluate( 1 - ( hp.CurrentHP / hp.MaxHP ) );
		emision.rateOverTime = main.startColor.color.a > 0.01f ? 15 : 0;
	}
}
