using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramesPerSecond : MonoBehaviour
{

    public float fps;
    private float sinceLastFrame;
    public CanvasGroup fpsCounter;
    public Text fpsText;
    // Start is called before the first frame update
    void Start()
    {
        fpsCounter.alpha = 0f;
        sinceLastFrame = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        sinceLastFrame += (Time.unscaledDeltaTime - sinceLastFrame) * 0.1f;
        fps = 1f / sinceLastFrame;
        fpsText.text = (int) fps + " FPS";
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ToggleFPSCounter();
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
