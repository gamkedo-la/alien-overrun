/**
 * Description: Shows floating text style texts and numbers.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class FloatingTextService : MonoBehaviour
{
	public static FloatingTextService Instance { get; private set; }

	[SerializeField] private GameObject floatingText = null;
	[SerializeField] private TMP_FontAsset font = null;

	private void Awake( )
	{
		if ( Instance != null && Instance != this )
			Destroy( this );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }

	void Start( )
	{
		Assert.IsNotNull( floatingText, $"Please assign <b>Floating Text</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
	}

	/// <summary>
	/// Draws a debug text on the screen.
	/// </summary>
	/// <param name="position">Position of the text.</param>
	/// <param name="text">Text to display.</param>
	/// <param name="size">Text size.</param>
	/// <param name="color">Text color.</param>
	/// <param name="duration">Duration before it vanishes.</param>
	/// <returns>Returns reference to the text's GameObject.</returns>
	public GameObject ShowFloatingTextOLD( Vector3 position, string text, int size = 14, Color? color = null, float duration = 0.5f )
	{
		GameObject canvasObj = new GameObject( $"Floating Text {Random.Range(0, 10000)}" );
		Canvas canvas = canvasObj.AddComponent<Canvas>( );
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.pixelPerfect = true;

		GameObject textObj = new GameObject( "Text" );
		textObj.transform.SetParent( canvasObj.transform );

		ContentSizeFitter fitter = textObj.AddComponent<ContentSizeFitter>( );
		fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
		fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

		var txt = textObj.AddComponent<TextMeshProUGUI>( );
		txt.font = font;

		txt.text = text;
		txt.fontSize = size;
		txt.color = color != null ? (Color)color : Color.white;
		txt.alignment = TextAlignmentOptions.MidlineJustified;

		textObj.transform.position = Camera.main.WorldToScreenPoint( position );

		Destroy( canvasObj, duration );

		return canvasObj;
	}

	public void ShowFloatingText( Vector3 position, string text, int size, Color color, float multiplyer )
	{
		GameObject go = Instantiate( floatingText, position, Quaternion.identity );
		FloatingText ft = go.GetComponent<FloatingText>( );

		ft.Set( text, color, multiplyer );
	}
}
