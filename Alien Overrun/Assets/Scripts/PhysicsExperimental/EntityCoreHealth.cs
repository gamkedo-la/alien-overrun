using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCoreHealth : MonoBehaviour
{
	private float hp = 1f;

    void Start()
    {
        
    }
	
    void Update()
    {
		if (hp <= 0f) Destroy(gameObject);
    }

	public void SubtractHp(float value) { hp -= value; }
}
