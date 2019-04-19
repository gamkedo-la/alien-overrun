using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGap : MonoBehaviour
{
	private void OnCollisionStay(Collision collision)
	{
		Vector3 dir = collision.contacts[0].point - transform.position;
		dir = -dir.normalized;
		dir = new Vector3(dir.x, 0f, dir.z);
		transform.parent.parent.position += dir * 0.1f;
	}
}
