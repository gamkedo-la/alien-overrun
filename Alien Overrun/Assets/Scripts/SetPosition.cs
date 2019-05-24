/**
 * Description: Sets position of a transform.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class SetPosition : MonoBehaviour
{
	public Transform Target = null;
	public Vector3 Position = Vector3.zero;
	void Start ()
	{
		Assert.IsNotNull( Target, $"Please assign <b>Target</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
	}

	public void OnPress( )
	{
		Target.position = Position;
	}
}
