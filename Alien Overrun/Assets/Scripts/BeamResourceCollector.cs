﻿/**
 * Description: Beam resource collector component of Towers - shoots a beam to collect resources.
 * Authors: SpadXIII
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class BeamResourceCollector : MonoBehaviour
{
	[SerializeField] private Transform shootPoint = null;
	[SerializeField] private float reloadTime = 2f;
	[SerializeField] private float shootDuration = 1f;
	[SerializeField] private int damage = 5;
	[SerializeField] private Color shotColor = Color.red;

	private Transform target = null;
	private Resource targetResource = null;
	private float timeToNextShot = 0;

    public int numberOfMineralMiners;
    public FMOD.Studio.EventInstance MineralMiningSound;
    public FMOD.Studio.EventInstance CrystalMiningSound;

    public int numberOfCrystalMiners;

    void Start( )
	{
		Assert.IsNotNull( shootPoint );
        MineralMiningSound = FMODUnity.RuntimeManager.CreateInstance("event:/Buildings/Mineral Miner/MineralMining");
        CrystalMiningSound = FMODUnity.RuntimeManager.CreateInstance("event:/Buildings/Crystal Miner/CrystalMining");
    }

	void Update( )
	{
		timeToNextShot -= Time.deltaTime;

		if ( target && targetResource && timeToNextShot <= 0 )
		{
			timeToNextShot = reloadTime;

			Utilities.DrawLine( shootPoint.position, target.position, shotColor, 0.1f, shootDuration );

			targetResource.CollectResources( damage, transform.position );
			FloatingTextService.Instance.ShowFloatingText( target.position + Vector3.up, damage.ToString( ), 1, shotColor, 1f );

            Debug.Log(targetResource.ResourceType);
            switch (targetResource.ResourceType)
            {
                case ResourceType.Minerals:
                    MineralMiningSound.start();
                    break;
                case ResourceType.Crystals:
                    CrystalMiningSound.start();
                    break;
                default:
                    break;
            }
        }
	}

	public void OnNewOponenet ( Transform oponent )
	{
		target = oponent;
		targetResource = target.GetComponent<Resource>( );
		//Debug.Log( name + " has new oponent: " + oponent.name );
	}

	public void OnOponenetLost ( )
	{
		target = null;
		targetResource = null;
		//Debug.Log( name + " lost oponent" );
	}
}
