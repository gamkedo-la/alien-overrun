using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playMechSounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playMechCreakOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/MechCreak");
    }

    public void playMechStepOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/MechStep");
    }

}
