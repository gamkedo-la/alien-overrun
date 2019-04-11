using UnityEngine;
using UnityEngine.EventSystems;

public class PauseGame : MonoBehaviour
{
    public CanvasGroup[] uiCanvasGroupsToHide;

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
		if ( BuildingManager.Instance.Building )
			return;

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

	public void ToggleSpeed()
	{
		if (Time.timeScale > 0.1f)
		{
			if (Time.timeScale == 8f)
			{
				Time.timeScale = 1f;
				Time.fixedDeltaTime = 0.02f * Time.timeScale;
			}
			else
			{
				Time.timeScale *= 2f;
				Time.fixedDeltaTime = 0.02f * Time.timeScale;
			}
		}
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

		EventSystem.current.SetSelectedGameObject( null );
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

		EventSystem.current.SetSelectedGameObject( null );
    }
}

