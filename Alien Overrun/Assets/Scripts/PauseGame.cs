using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseGame : MonoBehaviour
{
    private GameObject managers;
    private LevelManager levelManager;
    private CanvasGroup uiCanvasGroup;
    private CanvasGroup pauseCanvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        managers = GameObject.Find("Managers");
        levelManager = managers.GetComponent<LevelManager>();
        uiCanvasGroup = GameObject.Find("UI").GetComponent<CanvasGroup>();
        pauseCanvasGroup = GetComponent<CanvasGroup>();
        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (levelManager.Paused == false)
            {
                Pause( false );
            }
            else
            {
                Unpause();
            }
        }
		else if ( Input.GetKeyDown( KeyCode.P ) )
		{
			if ( levelManager.Paused == false )
				Pause( true );
			else
				Unpause( );
		}
        if (!EventSystem.current.IsPointerOverGameObject())
        {

        }
    }

	public void TogglePause()
	{
		if ( levelManager.Paused )
			Unpause( );
		else
			Pause( true );
	}

    public void Pause( bool activePause )
    {
        levelManager.Paused = true;
        Time.timeScale = 0;

		if ( !activePause )
			HideUI();
    }

    public void Unpause()
    {
        levelManager.Paused = false;
        Time.timeScale = 1;
        ShowUI();
    }
    void HideUI()
    {
        uiCanvasGroup.alpha = 0f;
        uiCanvasGroup.blocksRaycasts = false;
        pauseCanvasGroup.alpha = 1f;
        pauseCanvasGroup.blocksRaycasts = true;
    }
    void ShowUI()
    {
        uiCanvasGroup.alpha = 1f;
        uiCanvasGroup.blocksRaycasts = true;
        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.blocksRaycasts = false;
    }
}

