using UnityEngine;

public class playMechSounds : MonoBehaviour
{
	[SerializeField] private int mechAnimationLoopCount = 0;

	public void increaseMechAnimationLoopCount( )
	{
		mechAnimationLoopCount++;
		if ( mechAnimationLoopCount == 100 )
		{
			mechAnimationLoopCount = 0;
		}
	}

    // Update is called once per frame
    void Update()
    {
        if (mechAnimationLoopCount == 10)
        {
            mechAnimationLoopCount = 0;
        }
    }

    public void playMechCreakOneShot()
    {
        if (mechAnimationLoopCount == 1 || mechAnimationLoopCount == 5)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/MechCreak");
        }
    }

    public void playMechStepOneShot()
    {
        if (mechAnimationLoopCount == 1 || mechAnimationLoopCount == 5)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/MechStep");
        }
    }

}
