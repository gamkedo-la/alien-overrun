/**
 * Description: Makes sure object size is constant.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;

public class ConstantSize : MonoBehaviour
{
	private Vector3 scale = Vector3.zero;

	void Start ()
	{
		scale = transform.localScale;
	}

	void Update ()
	{
		transform.localScale = scale;
	}
}
