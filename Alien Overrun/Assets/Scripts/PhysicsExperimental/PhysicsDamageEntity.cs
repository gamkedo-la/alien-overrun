using UnityEngine;

public class PhysicsDamageEntity : MonoBehaviour
{
	[Header("Dust")]
	[SerializeField] private GameObject dust = null;
	[SerializeField] private float minSqrMag = 10f;
	[Header("Damage")]
	[SerializeField] private float damageValue = 0.1f;
	[SerializeField] private float coreDamageValue = 0.01f;

	private void OnCollisionEnter(Collision other)
	{
		if ( dust && other.relativeVelocity.sqrMagnitude >= minSqrMag )
			Instantiate( dust, transform.position, Quaternion.identity );

		PhysicsHealth health = other.gameObject.GetComponent<PhysicsHealth>();
		if (health != null)
		{
			health.hp -= damageValue;
			health.coreHp.ChangeHP(-coreDamageValue);
			Destroy(this);
		}
	}
}
