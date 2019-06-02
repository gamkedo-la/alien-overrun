/**
 * Description: Checks if FMOD loaded all the banks.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.SceneManagement;

public class FMODChecker : MonoBehaviour
{
	void Update ()
	{
		if ( FMODUnity.RuntimeManager.HasBankLoaded( "Master Bank" ) )
			SceneManager.LoadScene( "Main Menu" );
	}
}
