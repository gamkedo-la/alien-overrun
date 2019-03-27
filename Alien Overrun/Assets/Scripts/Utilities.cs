/**
 * Description: Utility methods used by other scripts.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.UI;

public class Utilities : MonoBehaviour
{
	/// <summary>
	/// Draws a line.
	/// </summary>
	/// <param name="start">Start point.</param>
	/// <param name="end">End point.</param>
	/// <param name="color">Line color.</param>
	/// <param name="width">Width of the line.</param>
	/// <param name="duration">Duration before it vanishes.</param>
	/// <returns>Returns reference to the line's GameObject.</returns>
	static public GameObject DrawLine( Vector3 start, Vector3 end, Color color, float width = 0.1f, float duration = 0.5f )
	{
		GameObject line = new GameObject( "Line " + Random.Range( 1, 1000 ) );
		line.transform.position = start;

		LineRenderer lr = line.AddComponent<LineRenderer>( );
		lr.material = new Material( Shader.Find( "Sprites/Default" ) );
		lr.startColor = color;
		lr.endColor = color;

		lr.startWidth = width;
		lr.endWidth = width;

		lr.SetPosition( 0, start );
		lr.SetPosition( 1, end );

		Destroy( line, duration );

		return line;
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
	static public GameObject DrawDebugText( Vector3 position, string text, int size = 14, Color? color = null, float duration = 0.5f )
	{
		GameObject canvasObj = new GameObject( "Debug Text" );
		Canvas canvas = canvasObj.AddComponent<Canvas>( );
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.pixelPerfect = true;

		GameObject textObj = new GameObject( "Text" );
		textObj.transform.SetParent( canvasObj.transform );

		ContentSizeFitter fitter = textObj.AddComponent<ContentSizeFitter>( );
		fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
		fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

		Text txt = textObj.AddComponent<Text>( );
		txt.font = Resources.GetBuiltinResource<Font>( "Arial.ttf" );

		txt.text = text;
		txt.fontSize = size;
		txt.color = color != null ? (Color)color : Color.white;
		txt.alignment = TextAnchor.MiddleCenter;

		textObj.transform.position = Camera.main.WorldToScreenPoint( position );

		Destroy( canvasObj, duration );

		return canvasObj;
	}

	/// <summary>
	/// Return a signed angle between 2 vectors.
	/// </summary>
	/// <param name="vector1">First vector.</param>
	/// <param name="vector2">Second vector.</param>
	/// <returns>Signed angle.</returns>
	static public float AngleBetweenVectors( Vector2 vector1, Vector2 vector2 )
	{
		Vector2 diference = vector2 - vector1;
		float sign = ( vector2.y < vector1.y ) ? -1.0f : 1.0f;

		return Vector2.Angle( Vector2.right, diference ) * sign;
	}
}
