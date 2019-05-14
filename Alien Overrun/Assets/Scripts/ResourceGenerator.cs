using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
	[SerializeField] private ResourceType resourceType = ResourceType.Minerals;
	[SerializeField] private int amount = 5;
	[SerializeField] private float interval = 3;

	void Start( )
	{
		Invoke( "GenerateResources", interval );
	}

	private void GenerateResources( )
	{
		ResourceManager.Instance.AddResources( resourceType, amount );

		Invoke( "GenerateResources", interval );
	}
}
