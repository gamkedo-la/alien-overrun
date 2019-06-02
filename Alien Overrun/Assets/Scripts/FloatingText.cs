/**
 * Description: Floating Text control class.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class FloatingText : MonoBehaviour
{
	[SerializeField] private TextMeshPro text = null;
	[SerializeField] private float speedMove = 2f;
	[SerializeField] private float speedFade = 0.5f;
	[SerializeField] private float speedSideways = 2f;
	[SerializeField] private float speedG = 4f;

	private float multiplayer;
	private float gravity = 0;
	private float sideways = 0;
	private float rnd = 1;

	void Start ()
	{
		Assert.IsNotNull( text, $"Please assign <b>Text</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
	}

	private void Update( )
	{
		gravity -= speedG * multiplayer * Time.deltaTime;
		transform.position +=  (Vector3.up * 4 + transform.right * sideways * rnd + Vector3.down * -gravity * rnd) * speedMove * multiplayer * Time.deltaTime;

		Color c = text.color;
		c.a -= speedFade * multiplayer * Time.deltaTime;
		c.a = Mathf.Clamp( c.a, 0f, 1f );
		text.color = c;

		if ( c.a <= 0 )
			Destroy( gameObject );
	}

	public void Set ( string message, Color color, float multiplayer )
	{
		this.multiplayer = multiplayer;
		text.text = message;
		text.color = color;

		sideways = speedSideways * Random.Range( 0, 2 ) > 0 ? 1 : -1;
		rnd = Random.Range( 0.5f, 1.5f );
	}
}
