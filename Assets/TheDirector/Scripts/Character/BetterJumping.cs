using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumping : MonoBehaviour
{
    private Rigidbody2D Enemy;  //se debe poder cambiar por enemy1(2) y usar una constante que mantenga el movimiento de salto
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    void Start()
    {
        Enemy = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (Enemy.velocity.y < 0)
        {
            Enemy.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (Enemy.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            Enemy.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
