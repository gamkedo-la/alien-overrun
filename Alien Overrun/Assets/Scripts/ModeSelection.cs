/**
 * Description: Holds selected game mode.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;

public class ModeSelection : MonoBehaviour
{
	public bool CreativeMode { get; set; }

	void Start( )
	{
		DontDestroyOnLoad( gameObject );
	}
}
