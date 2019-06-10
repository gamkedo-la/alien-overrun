/**
 * Description: Animates the size of the button.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour
{
	[SerializeField] private float bigScale = 1.1f;
	[SerializeField] private float changeSpeed = 0.5f;
	[SerializeField] private bool useOnMouse = false;

	Coroutine coroutine = null;
	Button button = null;
	bool isActive = true;

	private void Start( )
	{
		button = GetComponent<Button>( );
	}

	private void FixedUpdate( )
	{
		if ( button && !button.interactable )
			isActive = false;
		else if ( button && button.interactable )
			isActive = true;
	}

	private void OnMouseDown( )
	{
		if ( useOnMouse )
			OnOverEnter( );
        
	}

	private void OnMouseExit( )
	{
		if ( useOnMouse )
			OnOverExit( );
	}

	public void OnOverEnter( )
	{
		if ( !isActive )
			return;

		if ( coroutine != null )
			StopCoroutine( coroutine );
		coroutine = StartCoroutine( Animate( bigScale ) );
	}

	public void OnOverExit( )
	{
		if ( coroutine != null )
			StopCoroutine( coroutine );
		coroutine = StartCoroutine( Animate( 1f ) );
	}

	private IEnumerator Animate( float to )
	{
		float speed = transform.localScale.x < to ? changeSpeed : -changeSpeed;
		float scale = transform.localScale.x;

		while ( transform.localScale.x != to )
		{
			scale += speed * Time.unscaledDeltaTime;
			scale = Mathf.Clamp( scale, 1f, bigScale );

			transform.localScale = Vector3.one * scale;

			yield return null;
		}
	}
}
