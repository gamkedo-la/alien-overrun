using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class EditOptionsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	public enum EditOptions
	{
		Move,
		Delete,
		Repair,
		Upgrade
	};

	public EditOptions option = EditOptions.Move;

	public CursorRaycast cursorRaycast;

	public bool usingWorldSpaceCanvas = false;

	public AudioClip hoverSound;

	[HideInInspector] public bool hover = false;

	private AudioSource aud = null;

	void Start()
	{
		aud = GetComponent<AudioSource>();
		if (aud == null)
			aud = FindObjectOfType<AudioSource>();
	}

	void Update()
	{
		if (cursorRaycast.LockedSelection().Count <= 0)
			transform.parent.gameObject.SetActive(false);

		if (hover && Input.GetMouseButtonDown(0))
		{
			if (option == EditOptions.Move)
			{
				if (cursorRaycast.selectionMoveObject.transform.childCount > 0)
				{
					cursorRaycast.PlaceMoveSelection(!cursorRaycast.IsMoveSelectionPlaceable());
				}
				else
				{
					cursorRaycast.MoveSelection();
				}
			}
			else if (option == EditOptions.Delete)
			{
				cursorRaycast.DestroySelection();
				hover = false;
			}
			else if (option == EditOptions.Repair)
			{
				cursorRaycast.RepairSelection( );
			}
			else if (option == EditOptions.Upgrade)
			{

			}
		}

		hover = usingWorldSpaceCanvas ? false : hover;

		int cost = 0;
		if (option == EditOptions.Move)
			cost = -cursorRaycast.GetSelectionMoveCost();
		else if (option == EditOptions.Delete)
			cost = cursorRaycast.GetSelectionDeleteCost();
		else if (option == EditOptions.Repair)
			cost = -cursorRaycast.GetSelectionRepairCost();
		//else if (option == EditOptions.Upgrade)
		
		transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (cost == 0 ? "" : cost.ToString() + " Min");
	}

	void OnMouseOver()
	{
		if (enabled && aud != null)
			aud.PlayOneShot(hoverSound);
		hover = true;
	}

	private void OnMouseExit()
	{
		hover = false;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (enabled && aud != null)
			aud.PlayOneShot(hoverSound);
		hover = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hover = false;
	}
}
