using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDestroyCondition : MonoBehaviour
{
	public int totalPhysicsEntitiesOnGround = 10;
	public float destroyDelay = 5f;
	public float sinkDelay = 2f;
	public float sinkRate = -0.5f;
	private int count = 0;

	private float setupTimer = 0.75f;

	private HP coreHp;
	private bool sink = false;

	void Start()
    {
		coreHp = GetComponent<HP>();
    }
	
    void Update()
    {
		if (sink == true && sinkDelay <= 0f)
		{
			GameObject fallInObject = Instantiate(this.gameObject, transform.position, transform.rotation);
			Destroy(fallInObject.GetComponent<Rigidbody>());
			Destroy(fallInObject.GetComponent<Collider>());

			TimeDestroy ddelay = fallInObject.AddComponent<TimeDestroy>();
			ddelay.delay = destroyDelay;
			ConstantMove cmove = fallInObject.AddComponent<ConstantMove>();
			cmove.moveValue = new Vector3(0f, sinkRate, 0f);

			for (int i = 0; i < fallInObject.transform.childCount; i++)
			{
				GameObject mesh = fallInObject.transform.GetChild(i).gameObject;
				if (mesh.name == "Mesh")
				{
					for (int o = 0; o < mesh.transform.childCount; o++)
					{
						GameObject meshPiece = mesh.transform.GetChild(o).gameObject;
						Destroy(meshPiece.GetComponent<PhysicsHealth>());
						Destroy(meshPiece.GetComponent<Rigidbody>());
						Destroy(meshPiece.GetComponent<Collider>());
					}
					break;
				}
			}
			coreHp.ChangeHP(-10f);
		}
		else if (sink == true)
		{
			sinkDelay -= Time.deltaTime;
		}
		else if (count >= totalPhysicsEntitiesOnGround)
		{
			sink = true;
		}

		if(setupTimer > 0f)
			setupTimer -= Time.deltaTime;
	}

	public void IncrementCount() { if(setupTimer <= 0f) count++; }
	public bool IsDestroyed() { return sink; }
}
