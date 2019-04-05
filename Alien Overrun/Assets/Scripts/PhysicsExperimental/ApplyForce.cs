using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour
{
	public Vector3 force = Vector3.zero;

    void Start()
    {
		GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
    }
	
    void Update()
    {
        
    }
}
