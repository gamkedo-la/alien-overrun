using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerExistsSoPlaySound : MonoBehaviour
{


    public int NumberOfMineralMiners = 0;
    //private bool mineralMiningSoundPlaying;
    private FMOD.Studio.EventInstance MineralMiningSound;


    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        MineralMiningSound = FMODUnity.RuntimeManager.CreateInstance("event:/Buildings/Mineral Miner/MineralMining");
        //mineralMiningSoundPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (NumberOfMineralMiners == 1 /*&& !mineralMiningSoundPlaying*/)
        {
            MineralMiningSound.start();
            //mineralMiningSoundPlaying = true;
        }
        if (NumberOfMineralMiners == 0)
        {
            MineralMiningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //mineralMiningSoundPlaying = false;
        }
    }

}
