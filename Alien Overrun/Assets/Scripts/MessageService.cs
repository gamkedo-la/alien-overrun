/**
 * Description: Shows messages on screen.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class MessageService : MonoBehaviour
{
	public static MessageService Instance { get; private set; }

	[SerializeField] private TextMeshProUGUI label = null;

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
		Assert.IsNotNull( label );
	}

	public void ShowMessage( string message )
	{
		ShowMessage( message, 2.0f, Color.white );
	}

	public void ShowMessage( string message, float time, Color color )
	{
		label.color = color;
		label.text = message;

		CancelInvoke( "HideMessage" );
		Invoke( "HideMessage", time );
	}

	private void HideMessage( )
	{
		label.text = "";
	}
}
