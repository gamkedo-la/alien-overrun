using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTarget : MonoBehaviour
{
	private GameObject target = null;
	
	private void OnTriggerStay(Collider other)
	{
		if (target == null && other.gameObject.tag == "Building")
		{
			target = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (target != null && other.gameObject.tag == "Building")
		{
			target = null;
		}
	}

	public GameObject GetTarget() { return target; }
}
