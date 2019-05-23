/**
 * Description: Shows and/or on enabled/disabled.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;

public class ShowHide : MonoBehaviour
{
	public GameObject[] ToShowOnEnabled = null;
	public GameObject[] ToShowOnDisabled = null;
	public GameObject[] ToHideOnEnabled = null;
	public GameObject[] ToHideOnDisabled = null;

	private void OnEnable( )
	{
		foreach ( var item in ToShowOnEnabled )
			item.SetActive( true );

		foreach ( var item in ToHideOnEnabled )
			item.SetActive( false );
	}

	private void OnDisable( )
	{
		foreach ( var item in ToShowOnDisabled )
			item.SetActive( true );

		foreach ( var item in ToHideOnDisabled )
			item.SetActive( false );
	}
}
