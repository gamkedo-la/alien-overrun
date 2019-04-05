using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWithTimer : MonoBehaviour
{
	public float delay = 1f;
	public GameObject objectToCreate;

	private float timer = 0f;

    void Start()
    {
        
    }
	
    void Update()
    {
		if (timer <= 0f)
		{
			Instantiate(objectToCreate, transform.position, Quaternion.Euler(0f, 0f, 0f));
			timer = delay;
		}
		else
		{
			timer -= Time.deltaTime;
		}
    }
}
