/**
 * Description: Controls time of day.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class TimeOfDay : MonoBehaviour
{
	public static TimeOfDay Instance { get; private set; }

	[SerializeField] private Light sun = null;
	[SerializeField] private float startSunAngle = 47f;
	[SerializeField] private float endSunAngle = 5f;
	[SerializeField] private float startSunIntensity = 2.5f;
	[SerializeField] private float endSunIntensity = 1.0f;
	[SerializeField] private Gradient sunColor = null;
	[SerializeField] private float changeSpeed = 1.0f;

	private float currentProgress = 0f;
	private float desiredProgress = 0f;
	private Coroutine coroutine = null;

	private void Awake( )
	{
		if ( Instance != null && Instance != this )
			Destroy( gameObject );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }

	void Start ()
	{
		Assert.IsNotNull( sun, $"Please assign <b>Sun</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
	}

	public void SetDesiredTimeOfDay( float progress )
	{
		desiredProgress = Mathf.Clamp( progress, 0f, 1f );

		if (coroutine != null)
			StopCoroutine( coroutine );
		coroutine = StartCoroutine( ChangeTimeOfDay( ) );
	}

	private IEnumerator ChangeTimeOfDay ( )
	{
		while ( currentProgress != desiredProgress )
		{
			currentProgress += changeSpeed * changeSpeed;
			currentProgress = Mathf.Clamp( currentProgress, 0, desiredProgress );

			float angle = Utilities.ConvertRange( 0f, 1f, startSunAngle, endSunAngle, currentProgress );
			transform.rotation = Quaternion.Euler( angle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z );

			float intencity = Utilities.ConvertRange( 0f, 1f, startSunIntensity, endSunIntensity, currentProgress );
			sun.intensity = intencity;

			sun.color = sunColor.Evaluate( currentProgress );

			//Debug.Log( currentProgress );

			yield return null;
		}
	}
}
