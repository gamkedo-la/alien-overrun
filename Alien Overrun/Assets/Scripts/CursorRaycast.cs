using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CursorRaycast : MonoBehaviour
{
	public GameObject hoverSelectionInfoUI;
	public GameObject lockedSelectionInfoUI;
	[Space]

	public Material defaultCursorMaterial;
	public Material hoverCursorMaterial;
	public Material lockedCursorMaterial;
	[Space]

	public GameObject lockedSelectionIndicator;
	[Space]

	[SerializeField] private GameObject cursorIndicator;
	[SerializeField] private Transform pointOfPlane = null;

	private GameObject selectionIndicator = null;

	private Plane plane;

	private GameObject hoverSelection = null;
	private GameObject lockedSelection = null;
	private MeshRenderer rend;

	private TextMeshProUGUI hoverInfo1;
	private TextMeshProUGUI hoverInfo2;

	private TextMeshProUGUI lockedInfo1;
	private TextMeshProUGUI lockedInfo2;

	void Start()
    {
		Vector3 upVector = Vector3.up;
		plane = new Plane(upVector, pointOfPlane.position);

		rend = cursorIndicator.GetComponent<MeshRenderer>();

		GetUIComponents();
	}
	
    void Update()
    {
		UpdateCursorPositionAndEntityInfo();
		SelectionControl();
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
				rend.material = hoverCursorMaterial;
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



	private void GetUIComponents()
	{
		hoverInfo1 = hoverSelectionInfoUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		hoverInfo2 = hoverSelectionInfoUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

		lockedInfo1 = lockedSelectionInfoUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		lockedInfo2 = lockedSelectionInfoUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
	}



	private void UpdateCursorPositionAndEntityInfo()
	{
		Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (plane.Raycast(mRay, out float mouseDistance))
		{
			transform.position = mRay.GetPoint(mouseDistance);

			if (lockedSelection != null)
			{
				cursorIndicator.transform.position = mRay.GetPoint(mouseDistance);
				SetInfo(true);
			}
			else if (hoverSelection != null)
			{
				cursorIndicator.transform.position = hoverSelection.transform.position;
				SetInfo();
			}
			else
				cursorIndicator.transform.position = mRay.GetPoint(mouseDistance);
		}
	}



	private void SelectionControl()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (hoverSelection != null)
			{
				if (lockedSelection != null)
					lockedSelection.GetComponent<Building>().Indicator.HideRange();

				lockedSelection = hoverSelection;
				rend.material = lockedCursorMaterial;

				hoverSelectionInfoUI.SetActive(false);
				lockedSelectionInfoUI.SetActive(true);

				if (selectionIndicator != null)
					DestroyImmediate(selectionIndicator);
				selectionIndicator = null;

				selectionIndicator = Instantiate(lockedSelectionIndicator, lockedSelection.transform.position, Quaternion.Euler(0f, 0f, 0f));
				lockedSelection.GetComponent<Building>().Indicator.ShowRange(true);
			}
			else if (lockedSelection != null)
			{
				lockedSelection.GetComponent<Building>().Indicator.HideRange();

				lockedSelection = null;
				rend.material = defaultCursorMaterial;

				lockedSelectionInfoUI.SetActive(false);

				if (selectionIndicator != null)
					DestroyImmediate(selectionIndicator);
				selectionIndicator = null;
			}
		}
	}



	private void SetInfo( bool isLocked = false )
	{
		if (!isLocked)
		{
			Building building = hoverSelection.GetComponent<Building>();
			HP hp = hoverSelection.GetComponent<HP>();

			hoverInfo1.text = "Building: " + building.BuildingName + "\nBuild Cost: " + building.BuildCost;
			hoverInfo2.text = "Hit Points: " + hp.MaxHP + "/" + hp.CurrentHP + "\nBuild Time: " + building.BuildTime;
		}
		else
		{
			Building building = lockedSelection.GetComponent<Building>();
			HP hp = lockedSelection.GetComponent<HP>();

			lockedInfo1.text = "Building: " + building.BuildingName + "\nBuild Cost: " + building.BuildCost + "\nPlace Distance:" + building.PlaceDistance;
			lockedInfo2.text = "Hit Points: " + hp.MaxHP + "/" + hp.CurrentHP + "\nBuild Time: " + building.BuildTime;
		}
	}
}
