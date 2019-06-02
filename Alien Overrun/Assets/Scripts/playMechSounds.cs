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

	public void playMechCreakOneShot( )
	{
		FMODUnity.RuntimeManager.PlayOneShot( "event:/MechCreak" );
	}

	public void playMechStepOneShot( )
	{
		if ( mechAnimationLoopCount == 1 )
		{
			FMODUnity.RuntimeManager.PlayOneShot( "event:/MechStep" );
		}
	}
}
