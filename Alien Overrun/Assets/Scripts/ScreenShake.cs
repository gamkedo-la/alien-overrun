/**
 * Description: Controls screen shake.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class ScreenShake : MonoBehaviour
{
	public static ScreenShake Instance { get; private set; }

	public bool On { get; set; } = true;

	[SerializeField] private Animator animator = null;

	private void Awake( )
	{
		if ( Instance != null && Instance != this )
			Destroy( gameObject );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }

	void Start ()
	{
		Assert.IsNotNull( animator, $"Please assign <b>Animator</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
	}

	[ContextMenu("Do Low")]
	public void DoLow ()
	{
		if ( On )
			animator.SetTrigger( "Low" );
	}

	[ContextMenu("Do Big")]
	public void DoBig( )
	{
		if ( On )
			animator.SetTrigger( "Big" );
	}

	[ContextMenu( "Do Place" )]
	public void DoPlace( )
	{
		if ( On )
			animator.SetTrigger( "Place" );
	}
}
