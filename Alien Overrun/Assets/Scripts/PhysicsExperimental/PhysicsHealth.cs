using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHealth : MonoBehaviour
{
	public HP coreHp;
	public float hp = 1f;

	public float defMass = 100f;

	public PhysicsDestroyCondition condition;

	private Rigidbody rb;

    void Start()
    {
		rb = GetComponent<Rigidbody>();
    }
	
    void Update()
    {
		rb.mass = defMass * coreHp.CurrentHP;

		if (hp <= 0f) Destroy(gameObject);
    }

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Environment")
			condition.IncrementCount();
	}
}
