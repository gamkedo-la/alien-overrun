/**
 * Description: Fires an event in response to animation event.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
	[SerializeField] private UnityEvent onAnimationEvent1 = null;
	[SerializeField] private UnityEvent onAnimationEvent2 = null;
	[SerializeField] private UnityEvent onAnimationEvent3 = null;

	public void OnAnimationEvent1 ()
	{
		onAnimationEvent1.Invoke( );
	}

	public void OnAnimationEvent2( )
	{
		onAnimationEvent2.Invoke( );
	}

	public void OnAnimationEvent3( )
	{
		onAnimationEvent3.Invoke( );
	}
}
