using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerExistsSoPlaySound : MonoBehaviour
{
    
    public int NumberOfMineralMiners = 0;
    private FMOD.Studio.EventInstance MineralMiningSound = FMODUnity.RuntimeManager.CreateInstance("event:/MineralMining");
    FMOD.Studio.PLAYBACK_STATE MineralMiningSoundPlaybackState;
    // Start is called before the first frame update
    void Start()
    {
        NumberOfMineralMiners++;
    }

    // Update is called once per frame
    void Update()
    {
        MineralMiningSound.getPlaybackState(out MineralMiningSoundPlaybackState);

        if (NumberOfMineralMiners > 0 /*&& MineralMiningSoundPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING*/)
        {
            MineralMiningSound.start();
        }  
        if (NumberOfMineralMiners == 0)
        {
            MineralMiningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    private void OnDestroy()
    {
        NumberOfMineralMiners--;
    }
}
