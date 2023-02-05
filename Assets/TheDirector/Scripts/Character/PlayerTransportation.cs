using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransportation : MonoBehaviour
{
    [SerializeField] private float moveVelocity = 1f;
    private Teleporter currentTeleporter;
    private Rigidbody2D rb;
    private bool isMoving;

    // Update is called once per frame
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isMoving)
            return;

        rb.velocity = (currentTeleporter.transform.position - transform.position).normalized * moveVelocity;
        if((currentTeleporter.transform.position - transform.position).magnitude < 0.5f)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
        } 
    }

    public void MoveTo(Teleporter teleporter)
    {
        if (isMoving)
            return;

        if (currentTeleporter != null && !currentTeleporter.IsDestination(teleporter))
            return;

        currentTeleporter = teleporter;
        isMoving = true;
    }
}
