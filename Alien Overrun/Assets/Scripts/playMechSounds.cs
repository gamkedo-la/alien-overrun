using UnityEngine;

public class playMechSounds : MonoBehaviour
{
	[SerializeField] private int mechAnimationLoopCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (mechAnimationLoopCount == 10)
        {
            mechAnimationLoopCount = 0;
        }
    }

	public void PlayStepSound( ) => playMechStepOneShot( );

	public void PlaySpecialSound( ) => playMechCreakOneShot( );

	public void IncreaseCounter( ) => increaseMechAnimationLoopCount( );

	private void increaseMechAnimationLoopCount( )
	{
		mechAnimationLoopCount++;
		if ( mechAnimationLoopCount == 100 )
		{
			mechAnimationLoopCount = 0;
		}
	}

    private void playMechCreakOneShot()
    {
        if (mechAnimationLoopCount == 1 || mechAnimationLoopCount == 5)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Mech/MechCreak");
        }
    }

    private void playMechStepOneShot()
    {
        if (mechAnimationLoopCount == 1 || mechAnimationLoopCount == 5)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Mech/MechStep");
        }
    }
}
