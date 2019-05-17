/**
 * Description: Sets difficulty based on slider value.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class LevelDifficultySetter : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI label = null;
	[SerializeField] private ModeSelection modeSelection = null;
	[SerializeField] private Slider slider = null;
	[SerializeField] private Image[] images = null;
	[SerializeField] private float multiplyerUsed = 10f;
	[SerializeField] private Gradient gradient = new Gradient( );

	void Start ()
	{
		Assert.IsNotNull( label );
		Assert.IsNotNull( modeSelection );
		Assert.AreNotEqual( images.Length, 0 );

		SetValue( 10.0f );
	}

	public void SetValue( float value )
	{
		modeSelection.LevelDifficultyModifier = value / multiplyerUsed;
		label.text = string.Format( "x{0:0.0}", value / multiplyerUsed );

		foreach ( var image in images )
			image.color = gradient.Evaluate( Utilities.ConvertRange( slider.minValue, slider.maxValue, 0, 1, value ) );
	}
}
