using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playScuttlerSounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playScuttlerStepOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Scuttler/ScuttlerStep");
    }

    public void playScuttlerSwooshOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Scuttler/ScuttlerSwoosh");
    }

    public void playScuttlerAttackOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Scuttler/ScuttlerAttack");
    }

}
