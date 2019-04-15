using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBoxScript : MonoBehaviour
{
	public GameObject selectionCube;
	public Transform boxTransform;
	[Space]

	public CursorRaycast cursorRaycast;
	public GameObject cursorIndicator;
	[Space]

	[SerializeField] private Transform pointOfPlane = null;
	
	private Plane plane;

	private Vector3 startPosition = Vector3.zero;
	private Vector3 endPosition = Vector3.zero;

	static public SelectionBoxScript instance;
	
	void Start()
    {
		Vector3 upVector = Vector3.up;
		plane = new Plane(upVector, pointOfPlane.position);

		instance = this;
	}

	void Update()
	{
		Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (plane.Raycast(mRay, out float mouseDistance))
			boxTransform.position = mRay.GetPoint(mouseDistance);

		if (Input.GetMouseButtonUp(0))
		{
			ResetSelectBox();
		}
		else if (startPosition != Vector3.zero)
		{
			endPosition = boxTransform.position;

			boxTransform.localScale = new Vector3(
				startPosition.x - endPosition.x,
				boxTransform.localScale.y,
				startPosition.z - endPosition.z
			);

			boxTransform.position = new Vector3(
				startPosition.x - (boxTransform.localScale.x / 2f),
				boxTransform.position.y,
				startPosition.z - (boxTransform.localScale.z / 2f)
			);
		}
		else if (Input.GetMouseButtonDown(0))
		{
			startPosition = boxTransform.position;
			cursorIndicator.SetActive(false);
			selectionCube.SetActive(true);
		}
	}

	public void ResetSelectBox()
	{
		startPosition = Vector3.zero;
		boxTransform.localScale = new Vector3(0f, boxTransform.localScale.y, 0f);
		cursorIndicator.SetActive(true);
		selectionCube.SetActive(false);
	}

}
