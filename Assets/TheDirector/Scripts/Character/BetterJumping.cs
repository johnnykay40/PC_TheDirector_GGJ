using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumping : MonoBehaviour
{
    private Rigidbody2D characterRb;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    void Start()
    {
        characterRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (characterRb.velocity.y < 0)
        {
            characterRb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (characterRb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            characterRb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
