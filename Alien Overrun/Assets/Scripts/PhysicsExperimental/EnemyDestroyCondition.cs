using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyCondition : MonoBehaviour
{
	public GameObject destroyParticles;

	void Start()
    {
        
    }
	
    void Update()
    {
        
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<PhysicsDamageEntity>() != null)
		{
			Instantiate(destroyParticles, transform.position, transform.rotation);
			Destroy(transform.parent.parent.gameObject);
			Destroy(collision.gameObject);
		}
	}
}
