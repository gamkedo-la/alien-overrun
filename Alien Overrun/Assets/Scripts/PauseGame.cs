using UnityEngine;
using UnityEngine.EventSystems;

public class PauseGame : MonoBehaviour
{
    public CanvasGroup[] uiCanvasGroupsToHide;
    private GameObject managers;
    private LevelManager levelManager;
    private CanvasGroup pauseCanvasGroup;

    void Start()
    {
        levelManager = LevelManager.Instance;

        pauseCanvasGroup = GetComponent<CanvasGroup>();
        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.blocksRaycasts = false;
    }

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
		foreach ( var uiCanvasGroup in uiCanvasGroupsToHide )
		{
			uiCanvasGroup.alpha = 0f;
			uiCanvasGroup.blocksRaycasts = false;
		}

        pauseCanvasGroup.alpha = 1f;
        pauseCanvasGroup.blocksRaycasts = true;
    }

    void ShowUI()
    {
		foreach ( var uiCanvasGroup in uiCanvasGroupsToHide )
		{
			uiCanvasGroup.alpha = 1f;
			uiCanvasGroup.blocksRaycasts = true;
		}

        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.blocksRaycasts = false;
    }
}

