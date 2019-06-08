using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
	[SerializeField] private ResourceType resourceType = ResourceType.Minerals;
	[SerializeField] private int amount = 5;
	[SerializeField] private float interval = 3;
	[SerializeField] private Color color = Color.white;

	void Start( )
	{
		Invoke( "GenerateResources", interval );
	}

	private void GenerateResources( )
	{
		ResourceManager.Instance.AddResources( resourceType, amount );
		FloatingTextService.Instance.ShowFloatingText( transform.position + Vector3.up * 4, amount.ToString( ), 1, color, 1f );

		Invoke( "GenerateResources", interval );
	}
}
