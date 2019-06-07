using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playFrogAlienSounds : MonoBehaviour
{
	public void playFrogAlienStep()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Frog Alien/frogAlienSteps");
    }

}
