using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramesPerSecond : MonoBehaviour
{

    public int fps;
    public CanvasGroup fpsCounter;
    public Text fpsText;
    // Start is called before the first frame update
    void Start()
    {
        fpsCounter.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        fpsText.text = fps.ToString() + " FPS";
        fps = (int)(1f / Time.unscaledDeltaTime);
        if (Input.GetKeyDown(KeyCode.F3) && fpsCounter.alpha == 0f)
        {
            fpsCounter.alpha = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.F3) && fpsCounter.alpha == 1f)
        {
            fpsCounter.alpha = 0f;
        }
    }

	public void ToggleFPSCounter( )
	{
		if ( fpsCounter.alpha == 0f)
        {
            fpsCounter.alpha = 1f;
        }
        else if ( fpsCounter.alpha == 1f)
        {
            fpsCounter.alpha = 0f;
        }
	}
}
