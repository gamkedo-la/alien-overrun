/**
 * Description: Shows/hides the end (win/lose) screen.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;

public class EndScreen : MonoBehaviour
{
	public Animator animator;
	public PauseGame pauseGame;
	[SerializeField] private CanvasGroup[] uiCanvasGroupsToHide = null;

	private CanvasGroup pauseCanvasGroup;

	void Start( )
	{
		pauseCanvasGroup = GetComponent<CanvasGroup>( );
		pauseCanvasGroup.alpha = 0f;
		pauseCanvasGroup.blocksRaycasts = false;
	}

	public void Show( )
	{
		HideUI( );
	}

	public void Hide( )
	{
		ShowUI( );
	}

	public void Pause( )
	{
		//pauseGame.Pause( true );
	}

	private void HideUI( )
	{
		animator.SetTrigger( "Show" );

		foreach ( var uiCanvasGroup in uiCanvasGroupsToHide )
		{
			uiCanvasGroup.alpha = 0f;
			uiCanvasGroup.blocksRaycasts = false;
		}

		pauseCanvasGroup.alpha = 1f;
		pauseCanvasGroup.blocksRaycasts = true;
	}

	private void ShowUI( )
	{
		animator.SetTrigger( "Hide" );

		foreach ( var uiCanvasGroup in uiCanvasGroupsToHide )
		{
			uiCanvasGroup.alpha = 1f;
			uiCanvasGroup.blocksRaycasts = true;
		}

		pauseCanvasGroup.alpha = 0f;
		pauseCanvasGroup.blocksRaycasts = false;
	}
}
