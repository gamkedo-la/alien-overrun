﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CursorRaycast : MonoBehaviour
{
	public GameObject hoverSelectionInfoUI;

	public Material defaultCursorMaterial;
	public Material goodCursorMaterial;
	public Material badCursorMaterial;

	public GameObject lockedSelectionIndicator;

	[SerializeField] private GameObject cursorIndicator;
	[SerializeField] private Transform pointOfPlane = null;

	private GameObject selectionIndicator = null;

	private Plane plane;

	private GameObject hoverSelection = null;
	private GameObject lockedSelection = null;
	private MeshRenderer rend;

	private TextMeshProUGUI hoverInfo1;
	private TextMeshProUGUI hoverInfo2;

	void Start()
    {
		Vector3 upVector = Vector3.up;
		plane = new Plane(upVector, pointOfPlane.position);

		rend = cursorIndicator.GetComponent<MeshRenderer>();

		hoverInfo1 = hoverSelectionInfoUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		hoverInfo2 = hoverSelectionInfoUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
	}
	
    void Update()
    {
		Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (plane.Raycast(mRay, out float mouseDistance))
		{
			transform.position = mRay.GetPoint(mouseDistance);

			if (lockedSelection != null)
			{
				cursorIndicator.transform.position = mRay.GetPoint(mouseDistance);

				Building building = lockedSelection.GetComponent<Building>();
				HP hp = lockedSelection.GetComponent<HP>();

				hoverInfo1.text = "Building: " + building.BuildingName + "\nBuild Cost: " + building.BuildCost;
				hoverInfo2.text = "Hit Points: " + hp.MaxHP + "/" + hp.CurrentHP + "\nBuild Time: " + building.BuildTime;
			}
			else if (hoverSelection != null)
			{
				cursorIndicator.transform.position = hoverSelection.transform.position;

				Building building = hoverSelection.GetComponent<Building>();
				HP hp = hoverSelection.GetComponent<HP>();

				hoverInfo1.text = "Building: " + building.BuildingName + "\nBuild Cost: " + building.BuildCost;
				hoverInfo2.text = "Hit Points: " + hp.MaxHP + "/" + hp.CurrentHP + "\nBuild Time: " + building.BuildTime;
			}
			else
			{
				cursorIndicator.transform.position = mRay.GetPoint(mouseDistance);
			}
		}

		if (Input.GetMouseButtonDown(0))
		{
			if (hoverSelection != null)
			{
				lockedSelection = hoverSelection;
				rend.material = goodCursorMaterial;

				hoverSelectionInfoUI.SetActive(true);

				if(selectionIndicator != null)
					DestroyImmediate(selectionIndicator);
				selectionIndicator = null;

				selectionIndicator = Instantiate(lockedSelectionIndicator, lockedSelection.transform.position, Quaternion.Euler(0f, 0f, 0f));
			}
			else
			{
				lockedSelection = null;
				rend.material = defaultCursorMaterial;

				hoverSelectionInfoUI.SetActive(false);

				if (selectionIndicator != null)
					DestroyImmediate(selectionIndicator);
				selectionIndicator = null;
			}
		}
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Building"
			&& hoverSelection == null)
		{
			hoverSelection = collision.gameObject;

			if (lockedSelection == null)
			{
				hoverSelectionInfoUI.SetActive(true);
				rend.material = badCursorMaterial;
			}
		}
	}

	private void OnTriggerExit(Collider collision)
	{
		if (collision.gameObject.tag == "Building"
			&& hoverSelection == collision.gameObject)
		{
			hoverSelection = null;

			if (lockedSelection == null)
			{
				hoverSelectionInfoUI.SetActive(false);
				rend.material = defaultCursorMaterial;
			}
		}
	}
}
