/**
 * Description: Tries to find an oponent based on provided parameters.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventTransform: UnityEvent<Transform> { }

public class OponentFinder : MonoBehaviour
{
	enum OponentType
	{
		Enemy,
		Building
	}

	[SerializeField] private OponentType oponentType = OponentType.Building;
	[SerializeField] private float oponentFindCooldown = 1f;
	[SerializeField] private float findDistance = 4f;
	[SerializeField] private float attackDistance = 2f;
	[SerializeField] private UnityEventTransform onOponentFound = null;
	[SerializeField] private UnityEventTransform onInRange = null;
	[SerializeField] private UnityEvent onOponentNoLongetValid = null;

	private AbstractListManager oponentListManager;

	private GameObject currentOponent;
	private bool hadOponent = false;
	private bool wasInRange = false;

	void Start ()
	{
		Assert.IsNotNull( oponentListManager );
		InvokeRepeating( "DoSearch", oponentFindCooldown, oponentFindCooldown );
	}

	public float GetAttackDistance( ) => attackDistance;

	public void SetOponentListManager(AbstractListManager listManager)
	{
		oponentListManager = listManager;
	}

	private void DoSearch( )
	{
		bool validOponent = false;

		// Did we had an oponent and it's still valid?
		if ( hadOponent )
			validOponent = CheckIfOponentValid( currentOponent );

		// Try find new oponent if we have none (or just lost one)
		GameObject oldOponent = currentOponent;
		if ( !validOponent )
			currentOponent = TryFindNewOponent( );

		if ( oldOponent != currentOponent && currentOponent )
			onOponentFound.Invoke( currentOponent.transform );
		else if ( hadOponent && !currentOponent )
			onOponentNoLongetValid.Invoke( );

		if ( currentOponent )
			hadOponent = true;
		else
			hadOponent = false;

		if (currentOponent && CheckIfRange(currentOponent))
		{
			if ( !wasInRange )
				onInRange.Invoke( (currentOponent as GameObject).transform );
		}
		else
		{
			wasInRange = false;
		}
	}

	private bool CheckIfOponentValid( GameObject oponent )
	{
		if ( !oponent )
			return false;

		// Let's check all the requirements (one after another) and discard enemies that do not meet them
		if ( Vector3.Distance( oponent.transform.position, transform.position ) > findDistance )
			return false;

		return true;
	}

	private bool CheckIfRange( GameObject oponent )
	{
		if ( !oponent )
			return false;

		if ( Vector3.Distance( oponent.transform.position, transform.position ) > attackDistance )
			return false;

		return true;
	}

	private GameObject TryFindNewOponent( )
	{
		GameObject[] oponents = oponentListManager.GetGameObjects( );

		// Let's sort the list
		oponents = oponents.OrderBy( o => Vector3.Distance( o.transform.position, transform.position ) ).ToArray( );

		// Find first oponent that meets all of our requirements
		foreach ( var o in oponents )
		{
			if ( CheckIfOponentValid( o ) )
				return o;
		}

		// If we found none
		return null;
	}
}
