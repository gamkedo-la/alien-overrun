/**
 * Description: Holds selected game mode.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;

public class ModeSelection : MonoBehaviour
{
	public bool CreativeMode { get; set; } = false;
	public float LevelDifficultyModifier { get; set; } = 1.0f;

	void Start( )
	{
		DontDestroyOnLoad( gameObject );
	}
}
