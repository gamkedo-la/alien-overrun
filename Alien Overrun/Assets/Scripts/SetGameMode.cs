/**
 * Description: Sets the game mode.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;

public class SetGameMode : MonoBehaviour
{
	public void SetMode( bool creativeMode )
	{
		ModeSelection modeSelection = FindObjectOfType<ModeSelection>( );
		if ( modeSelection )
			modeSelection.CreativeMode = creativeMode;
	}
}
