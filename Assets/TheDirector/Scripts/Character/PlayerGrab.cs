using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public float distance = 0f;
    public Transform holdpoint;

    // Use the layer(s) for all objects that shall be grab able
    public LayerMask grabbable;

    private Transform currentlyGrabbedObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!currentlyGrabbedObject)
            {
                Collider2D hit = Physics2D.OverlapCircle(transform.position, distance, grabbable);

                if (hit)
                {
                    currentlyGrabbedObject = hit.transform;
                }
            }
            else // release currently grabbed object
            {
                currentlyGrabbedObject = null;
            }
        }

        if (currentlyGrabbedObject)
        {
            currentlyGrabbedObject.position = holdpoint.position;
        }
    }
}
