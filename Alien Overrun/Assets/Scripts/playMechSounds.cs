using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playMechSounds : MonoBehaviour
{
    public int mechAnimationLoopCount;

    // Start is called before the first frame update
    void Start()
    {
        mechAnimationLoopCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (mechAnimationLoopCount == 10)
        {
            mechAnimationLoopCount = 0;
        }
    }

    public void increaseMechAnimationLoopCount()
    {
        mechAnimationLoopCount++;
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
