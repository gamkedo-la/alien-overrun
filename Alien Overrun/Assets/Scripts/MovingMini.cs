/**
 * Description: Glowing resource cube that travels to a location.
 * Authors: Dominick Aiudi, Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;

public class MovingMini : MonoBehaviour
{
	[SerializeField] private Vector3 posOffset = new Vector3(0, 0.3f, 0);
    [SerializeField] private float speed = 0.1f;

	private Vector3 destination;
    private Vector3 currentPos;

    void Start()
    {
        transform.position += posOffset;
        currentPos = transform.position;

		destination = BuildingManager.Instance.GetNearestCoreCastleOrZero( transform.position ).Target + posOffset;
	}

    void Update()
    {
        transform.position = currentPos;

        if (Mathf.Abs(currentPos.x - destination.x) < 0.2f &&
            Mathf.Abs(currentPos.y - destination.y) < 0.2f &&
            Mathf.Abs(currentPos.z - destination.z) < 0.2f)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 direction = currentPos - destination;
            direction = direction.normalized;

            if (currentPos.x < destination.x)
                currentPos.x += Mathf.Abs(speed * direction.x);
            else
                currentPos.x -= Mathf.Abs(speed * direction.x);

            if (currentPos.y < destination.y)
                currentPos.y += Mathf.Abs(speed * direction.y);
            else
                currentPos.y -= Mathf.Abs(speed * direction.y);

            if (currentPos.z < destination.z)
                currentPos.z += Mathf.Abs(speed * direction.z);
            else
                currentPos.z -= Mathf.Abs(speed * direction.z);
        }
    }
}
