using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Catapult : MonoBehaviour
{

    public GameObject spawnOnHit = null;
    public float launchForce = 10f;
    private Rigidbody rb;
    private HingeJoint hinge;

    private void OnCollisionEnter( Collision collision )
	{
		if(collision.gameObject.CompareTag("Enemy")) {

            Debug.Log("Catapult touched an Enemy! Turning on motor!");
            if (spawnOnHit) Instantiate(spawnOnHit, collision.contacts[0].point, Quaternion.identity);
            hinge.useMotor = true;
            // FIXME: turn off in 1 second, reset position
        }
	}
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        hinge = gameObject.GetComponent<HingeJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (rb) rb.AddTorque(transform.right * launchForce); // rotate
    }
}
