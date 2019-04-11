using TMPro;
using UnityEngine;

public class FramesPerSecond : MonoBehaviour
{
	[SerializeField] private  CanvasGroup fpsCounter = null;
	[SerializeField] private  TextMeshProUGUI fpsText = null;
	[SerializeField] private float fpsUpdatePeriod = 1f;

	private int framesCount = 0;
    private float sinceLast = 0f;

	void Update()
    {
		framesCount++;
		sinceLast += Time.unscaledDeltaTime;

		if ( sinceLast >= fpsUpdatePeriod )
		{
			float fps = (framesCount / fpsUpdatePeriod);
			fpsText.text = $"{(int)fps} FPS";

			sinceLast = 0;
			framesCount = 0;
		}

        if (Input.GetKeyDown(KeyCode.F3))
            ToggleFPSCounter();
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
