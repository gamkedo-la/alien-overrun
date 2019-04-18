using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Physics : MonoBehaviour
{
	public float speed = 5f;

	public GameObject[] coresToAttack;

	public TriggerTarget triggerTarget;

	private GameObject target = null;
	private bool trigger = false;

	private float triggerDelay = 0.1f;
	private float triggerTimer = 0f;

	private Animator animator;

    void Start()
    {
		triggerTimer = triggerDelay;
		animator = GetComponent<Animator>();
    }
	
    void Update()
    {
		GetTarget();

		if (target != null)
		{
			if (!trigger)
			{
				Vector3 dir = (target.transform.position - transform.position).normalized;
				transform.position += dir * Time.deltaTime * speed;
				transform.rotation = Quaternion.Euler(0f, 90 - Utilities.AngleBetweenVectors(
					new Vector2(transform.position.x, transform.position.z),
					new Vector2(target.transform.position.x, target.transform.position.z)), 0f);
			}
			else
			{
				transform.rotation = Quaternion.Euler(0f, 90 - Utilities.AngleBetweenVectors(
					new Vector2(transform.position.x, transform.position.z),
					new Vector2(target.transform.position.x, target.transform.position.z)), 0f);
				animator.SetTrigger("Attack");
			}
		}

		if (triggerTimer <= 0f)
		{
			trigger = false;
			triggerTimer = triggerDelay;
		}
		else
		{
			triggerTimer -= Time.deltaTime;
		}
	}

	void GetTarget()
	{
		if (triggerTarget.GetTarget() == null)
		{
			int index = GetNearestCore();
			if(index > -1)
				target = coresToAttack[index];
		}
		else
		{
			GameObject tempTarget = triggerTarget.GetTarget();
			if (tempTarget != null)
				target = tempTarget;
		}
	}

	int GetNearestCore()
	{
		float leastDistance = 999999f;
		int index = -1;

		for (int i = 0; i < coresToAttack.Length; i++)
		{
			if (coresToAttack[i] != null)
			{
				float dist = Vector3.Distance(coresToAttack[i].transform.position, transform.position);
				if (leastDistance > dist)
				{
					leastDistance = dist;
					index = i;
				}
			}
		}
			
		return index;
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Building")
		{
			trigger = true;
			triggerTimer = triggerDelay;
		}
	}

}
