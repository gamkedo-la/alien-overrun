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

	[SerializeField] private GameObject cursorIndicator = null;
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

	private Camera cam = null;

	void Start()
	{
		Vector3 upVector = Vector3.up;
		plane = new Plane(upVector, pointOfPlane.position);
		cam = Camera.main;
		rend = cursorIndicator.GetComponent<MeshRenderer>();

		GetUIComponents();
	}
	void Update()
	{
		CheckSelectionExistence();
		UpdateCursorPositionAndEntityInfo();
		SelectionControl();
	}

	private void OnTriggerEnter(Collider collision)
	{
		if ( (collision.gameObject.CompareTag (Tags.Building) || collision.gameObject.CompareTag( Tags.Resource ))
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
		if ( (collision.gameObject.CompareTag( Tags.Building ) || collision.gameObject.CompareTag( Tags.Resource ))
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

	public bool IsObjectSelected(GameObject obj)
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



	private void CheckSelectionExistence()
	{
		//Case: Hover selected building gets destroyed/removed
		if (hoverSelection == null)
		{
			if (lockedSelection.Count <= 0)
			{
				hoverSelectionInfoUI.SetActive(false);
				rend.material = defaultCursorMaterial;
			}
		}

		//Case: Locked selected building gets destroyed/removed
		for(int i = 0; i < lockedSelection.Count; i++)
		{
			if (lockedSelection[i] == null)
				lockedSelection.Remove(lockedSelection[i]);
		}
	}

	private void UpdateCursorPositionAndEntityInfo()
	{
		Ray mRay = cam.ScreenPointToRay(Input.mousePosition);

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
					foreach (var sel in lockedSelection)
					{
						Building building = sel.GetComponent<Building>();
						if(building != null)
							building.Indicator.HideRange();
					}

					lockedSelection.Clear();
				}

				if (selectionIndicator.Count > 0)
				{
					foreach (var ind in selectionIndicator)
						Destroy(ind);

					selectionIndicator.Clear();
				}

				if (!IsObjectSelected(hoverSelection))
					AddToSelection(hoverSelection);
			}
			else if (lockedSelection.Count > 0)
			{
				DeselectAll();
			}
		}
		/*
		else if (Input.GetMouseButtonDown(1)) //deselects with right click
		{
			if (lockedSelection.Count > 0)
			{
				DeselectAll();
			}
		}
		*/
	}



	private void SetInfo( bool isLocked = false )
	{
		hoverInfo1.text = "";
		hoverInfo2.text = "";
		lockedInfo1.text = "";
		lockedInfo2.text = "";

		if (!isLocked)
		{
			Building building = hoverSelection.GetComponent<Building>();
			if (building != null)
			{
				HP hp = hoverSelection.GetComponent<HP>();

				hoverInfo1.text = "Building: " + building.BuildingName + "\nBuild Cost: " + building.BuildCost;
				hoverInfo2.text = "Hit Points: " + hp.MaxHP + "/" + hp.CurrentHP + "\nBuild Time: " + building.BuildTime;
			}
			else
			{
				string typeString = "";
				int type = (int)hoverSelection.transform.parent.gameObject.GetComponent<Resource>().ResourceType;

				if (type == 0) typeString = "Mineral";
				//Set Type Info here when there are more resources types

				hoverInfo1.text = "Resource Type: " + typeString;
				hoverInfo2.text = "Amount: " + hoverSelection.transform.parent.gameObject.GetComponent<Resource>().GetCurrentResources();
			}
		}
		else if (lockedSelection.Count <= 1)
		{
			Building building = lockedSelection[0].GetComponent<Building>();
			if (building != null)
			{
				HP hp = lockedSelection[0].GetComponent<HP>();

				lockedInfo1.text = "Building: " + building.BuildingName + "\nBuild Cost: " + building.BuildCost + "\nPlace Distance: " + building.PlaceDistance;
				lockedInfo2.text = "Hit Points: " + Mathf.FloorToInt(hp.MaxHP) + "/" + Mathf.FloorToInt(hp.CurrentHP) + "\nBuild Time: " + building.BuildTime;
			}
			else
			{
				string typeString = lockedSelection[0].transform.parent.gameObject.GetComponent<Resource>().ResourceType.ToString( );
				lockedInfo1.text = "Resource Type: " + typeString;
				lockedInfo2.text = "Amount: " + lockedSelection[0].transform.parent.gameObject.GetComponent<Resource>().GetCurrentResources();
			}
		}
		else if (lockedSelection.Count > 1)
		{
			int totalBuildings = 0;
			int totalResources = 0;
			int totalResourceAmount = 0;
			int totalBuildCost = 0;
			float avgMaxHP = 0f;
			float avgCurrentHP = 0f;
			foreach (var sel in lockedSelection)
			{
				Building building = sel.GetComponent<Building>();
				if (building != null)
				{
					totalBuildings++;
					totalBuildCost += building.BuildCost;

					avgMaxHP += sel.GetComponent<HP>().MaxHP;
					avgCurrentHP += sel.GetComponent<HP>().CurrentHP;
				}
				else
				{
					totalResources++;
					totalResourceAmount += sel.transform.parent.gameObject.GetComponent<Resource>().GetCurrentResources();
				}
			}

			avgMaxHP /= lockedSelection.Count;
			avgCurrentHP /= lockedSelection.Count;

			lockedInfo1.text = (totalBuildings > 0 ? "Total Buildings: " + totalBuildings + "\nBuild Cost: " + totalBuildCost + "\n" : "") + (totalResources > 0 ? "Total Resources: " + totalResources + "\n" : "");
			lockedInfo2.text = (totalBuildings > 0 ? "Avg. Hit Points: " + Mathf.FloorToInt(avgMaxHP) + "/" + Mathf.FloorToInt(avgCurrentHP) + "\n" : "") + (totalResources > 0 ? "Total Res. Amount: " + totalResourceAmount + "\n" : "");
			//Find a way to show resources and their total amount
		}
	}



	public void AddToSelection( GameObject addSel )
	{
		lockedSelection.Add(addSel);
		rend.material = lockedCursorMaterial;

		hoverSelectionInfoUI.SetActive(false);
		lockedSelectionInfoUI.SetActive(true);

		GameObject newSelInd = Instantiate(lockedSelectionIndicator, addSel.transform.position, Quaternion.Euler(0f, 0f, 0f));
		selectionIndicator.Add(newSelInd);
		newSelInd.transform.parent = addSel.transform;
		Building building = addSel.GetComponent<Building>();
		if(building != null)
			building.Indicator.ShowRange(true);
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

		Building building = remSel.GetComponent<Building>();
		if(building != null)
			building.Indicator.HideRange();
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
		{
			Building building = sel.GetComponent<Building>();
			if (building != null)
				building.Indicator.HideRange();
		}

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

	public void DestroySelection()
	{
		foreach (var sel in lockedSelection)
			Destroy(sel);

		lockedSelection.Clear();

		rend.material = defaultCursorMaterial;

		if (selectionIndicator.Count > 0)
		{
			foreach (var ind in selectionIndicator)
				Destroy(ind);

			selectionIndicator.Clear();
		}
	}
}
