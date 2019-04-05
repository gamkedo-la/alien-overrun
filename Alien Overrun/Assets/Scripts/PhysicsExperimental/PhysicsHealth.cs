using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHealth : MonoBehaviour
{
	public float hp = 1f;

	public float defMass = 100f;

	private Rigidbody rb;

    void Start()
    {
		rb = GetComponent<Rigidbody>();
    }
	
    void Update()
    {
		rb.mass = defMass * hp;

		if (hp <= 0.01f) Destroy(gameObject);
    }
}
