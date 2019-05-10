/**
 * Description: Progress/aggression marker class.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ProgressMarker : MonoBehaviour
{
	public int Theashold { get; private set; }
	public Color ActiveColor { get; set; }
	public bool Reached { get; private set; }

	[SerializeField] private Image upperPart = null;
	[SerializeField] private Image bottomPart = null;
	[SerializeField] private TextMeshProUGUI amountLabel = null;

	private string message;

	void Start( )
	{
		Assert.IsNotNull( upperPart );
		Assert.IsNotNull( bottomPart );
		Assert.IsNotNull( amountLabel );

		Reached = false;
	}

	public void Activate( )
	{
		upperPart.color = ActiveColor;
		bottomPart.color = ActiveColor;
		amountLabel.color = ActiveColor;

		if ( Reached )
			return;

		Reached = true;
		MessageService.Instance.ShowMessage( message, 2f, ActiveColor );
	}

	public void Set( Vector2 position, int value, Color activeColor, string message )
	{
		transform.localPosition = position;
		amountLabel.text = value.ToString( );
		ActiveColor = activeColor;
		Theashold = value;
		this.message = message;
	}
}
