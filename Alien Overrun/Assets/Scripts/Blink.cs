/**
 * Description: Does a 'Blink on hit' effect.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/


using UnityEngine;
using UnityEngine.Assertions;

public class Blink : MonoBehaviour
{
	[SerializeField] private Renderer[] toBlink = null;
	[SerializeField] private Material blinkMaterial = null;
	[SerializeField] private float blinkTime = 0.1f;

	private Material normalMaterial = null;
	private bool blinking = false;

	void Start( )
	{
		Assert.IsNotNull( toBlink, "You need to add sprites." );
		Assert.AreNotEqual( toBlink.Length, 0, "You need to add sprites." );
		Assert.IsNotNull( blinkMaterial, "Blink material can not be empty." );

		normalMaterial = toBlink[0].material;
	}

	public void DoBlink( )
	{
		DoBlink( blinkTime );
	}

	public void DoBlink( float time )
	{
		if ( blinking )
			return;

		blinking = true;
		SwapMaterial( blinkMaterial );
		Invoke( "Unblink", time );
	}

	private void Unblink( )
	{
		SwapMaterial( normalMaterial );
		blinking = false;
	}

	private void SwapMaterial( Material material )
	{
		foreach ( var sprite in toBlink )
			sprite.material = material;
	}
}
