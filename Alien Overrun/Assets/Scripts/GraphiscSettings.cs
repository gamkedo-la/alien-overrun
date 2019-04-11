/**
 * Description: Controls V Synchronization.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;

public class GraphiscSettings : MonoBehaviour
{
	[SerializeField] private GameObject postProcessObject = null;

	void Start ()
	{
		if ( !postProcessObject )
			postProcessObject = GameObject.Find( "Post-process Volume" );
	}

	public void On ()
	{
		QualitySettings.vSyncCount = 1;
	}

	public void Off( )
	{
		QualitySettings.vSyncCount = 0;
	}

	public void ToggleVSync( )
	{
		if ( QualitySettings.vSyncCount == 0 )
			QualitySettings.vSyncCount = 1;
		else
			QualitySettings.vSyncCount = 0;
	}

	public void TogglePostProcess( )
	{
		postProcessObject.SetActive( !postProcessObject.activeSelf );
	}
}
