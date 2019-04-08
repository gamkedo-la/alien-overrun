using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMove : MonoBehaviour
{
	public Vector3 moveValue = Vector3.zero;

    void Start()
    {
        
    }
	
    void Update()
    {
		transform.position += moveValue * Time.deltaTime;
    }
}
