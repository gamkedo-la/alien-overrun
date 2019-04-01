using TMPro;
using UnityEngine;

public class FramesPerSecond : MonoBehaviour
{
    public CanvasGroup fpsCounter;
    public TextMeshProUGUI fpsText;
    public float fps;

    private float sinceLastFrame;

    void Start()
    {
        fpsCounter.alpha = 0f;
        sinceLastFrame = 0f;
    }

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
