/**
 * Description: Shows threat floating text.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ThreatText : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI label = null;
	[SerializeField] private Vector2 destination = new Vector2(-34, 240);
	[SerializeField] private float speed = 3;

	private int value = 0;

	void Start ()
	{
		Assert.IsNotNull( label, $"Please assign <b>Label</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
	}

	void Update ()
	{
		Vector2 dir = (Vector3)destination - transform.localPosition;
		dir.Normalize( );

		transform.localPosition += (Vector3)dir * speed * Time.deltaTime;

		if (Vector2.Distance( transform.localPosition, destination) < 3f)
		{
			AIProgressManager.Instance.AddThreatNow( value );
			Destroy( gameObject );
		}
	}

	public void Set( int value, Vector2 destination )
	{
		this.value = value;
		label.text = value.ToString( );
		this.destination = destination;
	}
}
