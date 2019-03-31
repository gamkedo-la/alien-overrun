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

	private List<GameObject> selectionIndicator = new List<GameObject>();

	private Plane plane;

	private GameObject hoverSelection = null;

	public List<GameObject> LockedSelection() { return lockedSelection; }
	private List<GameObject> lockedSelection = new List<GameObject>();

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

			if (lockedSelection.Count <= 0)
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

			if (lockedSelection.Count <= 0)
			{
				hoverSelectionInfoUI.SetActive(false);
				rend.material = defaultCursorMaterial;
			}
		}
	}



	public bool IsObjectSelected( GameObject obj )
	{
		foreach (var sel in lockedSelection)
			if (sel == obj) return true;

		return false;
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

			if (lockedSelection.Count > 0)
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
				if (lockedSelection.Count > 0)
				{
					foreach( var sel in lockedSelection )
						sel.GetComponent<Building>().Indicator.HideRange();

					lockedSelection.Clear();
				}

				if (selectionIndicator.Count > 0)
				{
					foreach (var ind in selectionIndicator)
						Destroy(ind);

					selectionIndicator.Clear();
				}

				if(!IsObjectSelected( hoverSelection ))
					AddToSelection( hoverSelection );
			}
			else if (lockedSelection.Count > 0)
			{
				DeselectAll();
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
		else if (lockedSelection.Count <= 1)
		{
			Building building = lockedSelection[0].GetComponent<Building>();
			HP hp = lockedSelection[0].GetComponent<HP>();

			lockedInfo1.text = "Building: " + building.BuildingName + "\nBuild Cost: " + building.BuildCost + "\nPlace Distance:" + building.PlaceDistance;
			lockedInfo2.text = "Hit Points: " + hp.MaxHP + "/" + hp.CurrentHP + "\nBuild Time: " + building.BuildTime;
		}
		else if (lockedSelection.Count > 1)
		{
			int totalBuildCost = 0;
			foreach (var sel in lockedSelection)
				totalBuildCost += sel.GetComponent<Building>().BuildCost;

			float avgMaxHP = 0f;
			foreach (var sel in lockedSelection)
				avgMaxHP += sel.GetComponent<HP>().MaxHP;
			avgMaxHP /= lockedSelection.Count;

			float avgCurrentHP = 0f;
			foreach (var sel in lockedSelection)
				avgCurrentHP += sel.GetComponent<HP>().CurrentHP;
			avgCurrentHP /= lockedSelection.Count;

			lockedInfo1.text = "Total Buildings: " + lockedSelection.Count + "\nBuild Cost: " + totalBuildCost;
			lockedInfo2.text = "Avg. Hit Points: " + avgMaxHP + "/" + avgCurrentHP;
		}
	}



	public void AddToSelection( GameObject addSel )
	{
		lockedSelection.Add(addSel);
		rend.material = lockedCursorMaterial;

		hoverSelectionInfoUI.SetActive(false);
		lockedSelectionInfoUI.SetActive(true);

		selectionIndicator.Add(Instantiate(lockedSelectionIndicator, addSel.transform.position, Quaternion.Euler(0f, 0f, 0f)));
		addSel.GetComponent<Building>().Indicator.ShowRange(true);
	}

	public void RemoveFromSelection(GameObject remSel)
	{
		foreach (var ind in selectionIndicator)
		{
			if (ind.transform.position == remSel.transform.position)
			{
				selectionIndicator.Remove(ind);
				Destroy(ind);
				break;
			}
		}

		remSel.GetComponent<Building>().Indicator.HideRange();
		lockedSelection.Remove(remSel);

		if (lockedSelection.Count <= 0)
		{
			rend.material = defaultCursorMaterial;
			lockedSelectionInfoUI.SetActive(false);
		}
	}

	public void DeselectAll()
	{
		foreach (var sel in lockedSelection)
			sel.GetComponent<Building>().Indicator.HideRange();

		lockedSelection.Clear();

		rend.material = defaultCursorMaterial;

		lockedSelectionInfoUI.SetActive(false);

		if (selectionIndicator.Count > 0)
		{
			foreach (var ind in selectionIndicator)
				Destroy(ind);

			selectionIndicator.Clear();
		}
	}
}
