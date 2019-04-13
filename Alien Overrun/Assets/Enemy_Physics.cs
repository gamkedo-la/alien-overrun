using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Physics : MonoBehaviour
{
	public float speed = 5f;

	public GameObject[] cores;

	public TriggerTarget triggerTarget;

	private GameObject target = null;

    void Start()
    {
        
    }
	
    void Update()
    {
		GetTarget();

		Vector3 dir = (target.transform.position - transform.position).normalized;
		transform.position += dir * Time.deltaTime * speed;
	}

	void GetTarget()
	{
		if (triggerTarget.GetTarget() == null)
			target = cores[GetNearestCore()];
		else
			target = triggerTarget.GetTarget();
	}

	int GetNearestCore()
	{
		float leastDistance = 999999f;
		int index = -1;

		for (int i = 0; i < cores.Length; i++)
		{
			float dist = Vector3.Distance(cores[i].transform.position, transform.position);
			if (leastDistance > dist)
			{
				leastDistance = dist;
				index = i;
			}
		}
			
		return index;
	}

	private void OnTriggerEnter(Collider other)
	{
		
	}
}
