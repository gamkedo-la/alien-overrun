/**
 * Description: Controls the volume of the game.
 * Authors: Kornel, Stebs
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
	[SerializeField] private AudioMixer audioMixer = null;
	[SerializeField] private Slider volumeSlider = null;
	[SerializeField] private string exposedPropertyName = "Volume";

	//private FMOD.Studio.Bus masterSoundBus;

	void Start( )
	{
		Assert.IsNotNull( audioMixer, $"Please assign <b>AudioMixer</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
		Assert.IsNotNull( volumeSlider, $"Please assign <b>VolumeSlider</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );

		volumeSlider.value = PlayerPrefs.GetFloat( exposedPropertyName, 1f );
		//masterSoundBus = FMODUnity.RuntimeManager.GetBus( "bus:/master_volume" );
	}

	/// <summary>
	/// Set desired volume.
	/// </summary>
	/// <param name="value">New volume (0, 1).</param>
	public void SetVolume( float value )
	{
		value = Mathf.Clamp( value, 0f, 1f );

		PlayerPrefs.SetFloat( exposedPropertyName, value );

		if ( value != 0 )
			audioMixer.SetFloat( exposedPropertyName, Mathf.Log10( value ) * 20 );
		else
			audioMixer.SetFloat( exposedPropertyName, -80 ); // No sound

		//masterSoundBus.setVolume( value );
	}
}