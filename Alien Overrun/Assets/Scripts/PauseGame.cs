using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public Animator animator;
    public CanvasGroup[] uiCanvasGroupsToHide;
	[SerializeField] private float maxSpeed = 4f;
	[SerializeField] private float maxForceScale = 0.25f;
	[SerializeField] private float minSpeed = 0.2f;
	[SerializeField] private float minForceScale = 5f;

    private LevelManager levelManager;
    private CanvasGroup pauseCanvasGroup;
	private float currentSpeed = 1f;

	static public float ForceScale = 1;

    void Start()
    {
        levelManager = LevelManager.Instance;

        pauseCanvasGroup = GetComponent<CanvasGroup>();
        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.blocksRaycasts = false;
    }

	void OnEnable( )
	{
		NormalSpeed( );
	}

	void OnDisable( )
	{
		NormalSpeed( );
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
        Time.timeScale = currentSpeed;
        ShowUI();
    }

	public void ToggleSpeed()
	{
		if (Time.timeScale > 0.1f)
		{
			if (Time.timeScale == 8f)
			{
				Time.timeScale = 1f;
				ForceScale = 1f;
				currentSpeed = Time.timeScale;
				Time.fixedDeltaTime = 0.02f * Time.timeScale;
			}
			else
			{
				Time.timeScale *= 2f;
				ForceScale = 0.5f;
				currentSpeed = Time.timeScale;
				Time.fixedDeltaTime = 0.02f * Time.timeScale;
			}
		}
	}

	public void NormalSpeed( )
	{
		Time.timeScale = 1f;
		ForceScale = 1f;
		currentSpeed = Time.timeScale;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}

	public void SlowSpeed( )
	{
		Time.timeScale = minSpeed;
		ForceScale = minForceScale;
		currentSpeed = Time.timeScale;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}

	public void FastSpeed( )
	{
		Time.timeScale = maxSpeed;
		ForceScale = maxForceScale;
		currentSpeed = Time.timeScale;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}

	public void Reset( )
	{
		SceneManager.LoadScene( gameObject.scene.name );
	}

	void HideUI()
    {
		animator.SetTrigger( "Show" );

		foreach ( var uiCanvasGroup in uiCanvasGroupsToHide )
		{
			uiCanvasGroup.alpha = 0f;
			uiCanvasGroup.blocksRaycasts = false;
		}

        //pauseCanvasGroup.alpha = 1f;
        pauseCanvasGroup.blocksRaycasts = true;

		EventSystem.current.SetSelectedGameObject( null );
    }

    void ShowUI()
    {
		animator.SetTrigger( "Hide" );

		foreach ( var uiCanvasGroup in uiCanvasGroupsToHide )
		{
			uiCanvasGroup.alpha = 1f;
			uiCanvasGroup.blocksRaycasts = true;
		}

        //pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.blocksRaycasts = false;

		EventSystem.current.SetSelectedGameObject( null );
    }
}

