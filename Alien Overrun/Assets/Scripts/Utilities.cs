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

	static public float ConvertRange( float originalStart, float originalEnd, float newStart, float newEnd, float value )
	{
		float scale = ( newEnd - newStart ) / ( originalEnd - originalStart );
		return ( newStart + ( ( value - originalStart ) * scale ) );
	}
}
