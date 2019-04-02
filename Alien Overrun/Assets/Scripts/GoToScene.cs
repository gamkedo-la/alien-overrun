/**
 * Description: Allows to transition to a scene.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
	[SerializeField] private string sceneName = "Main";

	public void GoTo()
	{
		SceneManager.LoadScene( sceneName, LoadSceneMode.Single );
	}
}
