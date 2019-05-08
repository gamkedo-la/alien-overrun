/**
 * Description: Detects if entity is on ground.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;

public class GroundDetect : MonoBehaviour
{
	public bool IsOnGround { get { return touchPoints != 0; } }

	private int touchPoints = 0;

	private void OnCollisionEnter( Collision collision )
	{
		if ( collision.gameObject.CompareTag( Tags.Environment ) )
			touchPoints++;
	}

	private void OnCollisionExit( Collision collision )
	{
		if ( collision.gameObject.CompareTag( Tags.Environment ) )
			touchPoints--;
	}
}
