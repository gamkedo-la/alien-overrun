using UnityEngine;

public class playMechSounds : MonoBehaviour
{
	[SerializeField] private int mechAnimationLoopCount = 0;
    FMOD.Studio.EventInstance MineralMiningSound;

    private void Awake()
    {
        //MineralMiningSound = FMODUnity.RuntimeManager.CreateInstance("event:/Buildings/Mineral Miner/MineralMining");
    }
    // Update is called once per frame

    private void Start()
    {
        MineralMiningSound.start();
    }
    void Update()
    {
        if (mechAnimationLoopCount == 10)
        {
            mechAnimationLoopCount = 0;
        }
    }

	public void PlayStepSound( ) => playMechStepOneShot( );

	public void PlaySpecialSound( ) => playMechCreakOneShot( );

	public void IncreaseCounter( ) => increaseMechAnimationLoopCount( );

	private void increaseMechAnimationLoopCount( )
	{
		mechAnimationLoopCount++;
		if ( mechAnimationLoopCount == 100 )
		{
			mechAnimationLoopCount = 0;
		}
	}

    private void playMechCreakOneShot()
    {
        
            FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Mech/MechCreak");
       
    }

    private void playMechStepOneShot()
    {
        
            FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Mech/MechStep");
       
    }
}
