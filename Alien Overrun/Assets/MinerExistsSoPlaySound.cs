using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerExistsSoPlaySound : MonoBehaviour
{

    
    public int NumberOfMineralMiners = 0;
    private bool mineralMiningSoundPlaying;
    
    

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
        mineralMiningSoundPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (NumberOfMineralMiners == 1 && !mineralMiningSoundPlaying)
        {
            //FMODUnity.RuntimeManager.PlayOneShot("event:/Buildings/Mineral Miner/MineralMining");
            mineralMiningSoundPlaying = true;
        }  
        if (NumberOfMineralMiners == 0)
        {
            //MineralMiningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            mineralMiningSoundPlaying = false;
        }
    }

}
