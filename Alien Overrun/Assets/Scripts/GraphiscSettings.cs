/**
 * Description: Controls V Synchronization.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.UI;

public class GraphiscSettings : MonoBehaviour
{
	[SerializeField] private GameObject grass = null;
	[SerializeField] private Image grassButton = null;
	[SerializeField] private Image vsyncButton = null;
	[SerializeField] private Image shakeButton = null;
	[SerializeField] private Image postButton = null;
	[SerializeField] private Color onColor = Color.green;
	[SerializeField] private Color offColor = Color.gray;
	[SerializeField] private GameObject postProcessObject = null;
	[SerializeField] private bool vSyncOffonStart = true;

	void Start ()
	{
		if ( !postProcessObject )
			postProcessObject = GameObject.Find( "Post-process Volume" );

		if ( vSyncOffonStart )
			Off( );
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

		if ( QualitySettings.vSyncCount == 0 )
			vsyncButton.color = onColor;
		else
			vsyncButton.color = offColor;
	}

	public void TogglePostProcess( )
	{
		postProcessObject.SetActive( !postProcessObject.activeSelf );

		if ( postProcessObject.activeSelf )
			postButton.color = onColor;
		else
			postButton.color = offColor;
	}

	public void ToggleGrass( )
	{
		grass.SetActive( !grass.activeSelf );

		if ( grass.activeSelf )
			grassButton.color = onColor;
		else
			grassButton.color = offColor;
	}

	public void ToggleScreenshake( )
	{
		ScreenShake.Instance.On = !ScreenShake.Instance.On;

		if ( ScreenShake.Instance.On )
			shakeButton.color = onColor;
		else
			shakeButton.color = offColor;
	}
}
