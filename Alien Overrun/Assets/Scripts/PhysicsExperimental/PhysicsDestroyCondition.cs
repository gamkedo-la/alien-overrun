using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDestroyCondition : MonoBehaviour
{
	public int totalPhysicsEntitiesOnGround = 10;
	private int count = 0;

	private HP coreHp;

	void Start()
    {
		coreHp = GetComponent<HP>();
    }
	
    void Update()
    {
		if (count >= totalPhysicsEntitiesOnGround)
		{
			GameObject fallInObject = Instantiate(this.gameObject, transform.position, transform.rotation);
			Destroy(fallInObject.GetComponent<Rigidbody>());
			Destroy(fallInObject.GetComponent<Collider>());

			TimeDestroy ddelay = fallInObject.AddComponent<TimeDestroy>();
			ddelay.delay = 20f;
			ConstantMove cmove = fallInObject.AddComponent<ConstantMove>();
			cmove.moveValue = new Vector3(0f, -0.2f, 0f);

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
    }

	public void IncrementCount() { if(coreHp.CurrentHP < 0.99f) count++; }
}
