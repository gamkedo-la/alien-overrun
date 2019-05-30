/**
 * Description: Simulates turning on lights at night.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class TurnOnTheLights : MonoBehaviour
{
	[SerializeField] private Material lightMaterial = null;
	[SerializeField] private GameObject[] toTurnOnObject = null;
	[SerializeField] private Renderer[] toTurnOnRenderer = null;
	[SerializeField] private float renrererChanceToTurnOn = 50f;
	[SerializeField] private float threshold = 0.5f;
	[SerializeField] private float maxDelay = 2f;

	void Start ()
	{
		Assert.IsNotNull( lightMaterial, $"Please assign <b>OnMaterial</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
		//Assert.AreNotEqual( toTurnOnObject.Length, 0, $"Please assign <b>OnMaterial</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
		//Assert.AreNotEqual( toTurnOnRenderer.Length, 0, $"Please assign <b>OnMaterial</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
		Invoke( "CheckForNight", 1f );
	}

	private void CheckForNight ()
	{
		if ( TimeOfDay.Instance.CurrentProgress >= threshold )
			Invoke( "TurnOn", Random.Range( 0f, maxDelay ) );
		else
			Invoke( "CheckForNight", 1f );
	}

	private void TurnOn()
	{
		foreach ( var r in toTurnOnRenderer )
		{
			float rnd = Random.Range( 0, 100 );
			bool should = rnd <= renrererChanceToTurnOn;
			if ( should )
			{
				//Debug.Log( $"{rnd}, {should}" );
				r.material = lightMaterial;
			}
		}

		foreach ( var o in toTurnOnObject )
			o.SetActive( true );
	}
}
