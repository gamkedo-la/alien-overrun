/**
 * Description: Glowing resource cube that travels to a location
 * Authors: Dominick Aiudi
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMini : MonoBehaviour
{
    [SerializeField] private Vector3 destination = new Vector3(5.0f, 5.0f, 5.0f);
    [SerializeField] private float speed = 0.1f;
    private Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(0.0f, 1.0f, 0.0f);
        currentPos = transform.position;
    }

    // Update is called once per frame
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
