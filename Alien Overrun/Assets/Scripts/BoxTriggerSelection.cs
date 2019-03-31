using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTriggerSelection : MonoBehaviour
{
	public CursorRaycast cursorRaycast;

    void Start()
    {
    }
	
    void Update()
    {
        
    }
	
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Building"
			&& !cursorRaycast.IsObjectSelected(collision.gameObject))
		{
			cursorRaycast.AddToSelection(collision.gameObject);
		}
	}

	private void OnTriggerExit(Collider collision)
	{
		if (collision.gameObject.tag == "Building"
			&& cursorRaycast.IsObjectSelected(collision.gameObject))
		{
			cursorRaycast.RemoveFromSelection(collision.gameObject);
		}
	}
}
