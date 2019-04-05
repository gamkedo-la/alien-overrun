using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDestroy : MonoBehaviour
{
	public float delay = 5f;

    void Start()
    {
        
    }
	
    void Update()
    {
		if (delay <= 0f) Destroy(gameObject);
		else delay -= Time.deltaTime;
    }
}
