using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playFrogAlienSounds : MonoBehaviour
{
	public void PlayStepSound( ) => playFrogAlienStep( );

	public void PlaySpecialSound( ) { }

    public void PlayAttackSound() => PlayFrogAlienStompOneShot();
    
        
    

    public void PlayFrogAlienStompOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Frog Alien/FrogAlienStomp");
    }

	public void IncreaseCounter( ) { }

	private void playFrogAlienStep()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Frog Alien/frogAlienSteps");
    }

}
