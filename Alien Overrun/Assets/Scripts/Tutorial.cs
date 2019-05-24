/**
 * Description: Controls the tutorial
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class Tutorial : MonoBehaviour
{
	[SerializeField] private GameObject tutorialObject = null;

	void Start ()
	{
		Assert.IsNotNull( tutorialObject, $"Please assign <b>TutorialObject</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );

		if ( PlayerPrefs.GetInt( "Tutorial", 1 ) == 1 )
			tutorialObject.SetActive( true );
	}

	public void Checkbox (bool value)
	{
		if ( value )
			NeverShow( );
		else
			AlwaysShow( );
	}

	public void NeverShow ()
	{
		PlayerPrefs.SetInt( "Tutorial", 0 );
	}

	[ContextMenu( "Always Show" )]
	public void AlwaysShow( )
	{
		PlayerPrefs.SetInt( "Tutorial", 1 );
	}
}
