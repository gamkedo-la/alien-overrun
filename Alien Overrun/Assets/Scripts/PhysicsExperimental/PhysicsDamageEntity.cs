using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDamageEntity : MonoBehaviour
{
	public float damageValue = 0.1f;
	public float coreDamageValue = 0.01f;

    void Start()
    {
        
    }
	
    void Update()
    {
        
    }

	private void OnCollisionEnter(Collision other)
	{
		PhysicsHealth health = other.gameObject.GetComponent<PhysicsHealth>();
		if (health != null)
		{
			health.hp -= damageValue;
			health.coreHp.ChangeHP(-coreDamageValue);
			Destroy(this);
		}
	}
}
