using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransportation : MonoBehaviour
{
    [Range(20, 150), SerializeField] private float moveVelocity;
    private Teleporter currentTeleporter;
    private Rigidbody2D rigidbodyPlayer;
    private bool isMoving;

    private void Awake() => rigidbodyPlayer = GetComponent<Rigidbody2D>();

    void Update()
    {
        if (!isMoving)
            return;

        rigidbodyPlayer.velocity = (currentTeleporter.transform.position - transform.position).normalized * moveVelocity;
        if((currentTeleporter.transform.position - transform.position).magnitude < 0.5f)
        {
            isMoving = false;
            rigidbodyPlayer.velocity = Vector2.zero;
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
