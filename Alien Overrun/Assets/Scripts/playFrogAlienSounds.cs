using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playFrogAlienSounds : MonoBehaviour
{
	public int mechAnimationLoopCount;

	// Start is called before the first frame update
	void Start( )
	{
		mechAnimationLoopCount = 0;
	}

	// Update is called once per frame
	void Update( )
	{
		if ( mechAnimationLoopCount == 100 )
		{
			mechAnimationLoopCount = 0;
		}
	}

	public void increaseMechAnimationLoopCount( )
	{
		mechAnimationLoopCount++;
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
