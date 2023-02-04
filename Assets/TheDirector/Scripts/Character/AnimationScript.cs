using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator characterAnim;
    private CharacterMovementGeneral move;
    private Collision collision;
    [HideInInspector]
    public SpriteRenderer sr;

    void Start()
    {
        characterAnim = GetComponent<Animator>();
        collision = GetComponentInParent<Collision>();
        move = GetComponentInParent<CharacterMovementGeneral>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        characterAnim.SetBool("onGround", collision.onGround);        
        characterAnim.SetBool("canMove", move.canMove);
        characterAnim.SetBool("isDashing", move.isDashing);

    }

    public void SetHorizontalMovement(float x, float y, float yVel)
    {
        characterAnim.SetFloat("HorizontalAxis", x);
        characterAnim.SetFloat("VerticalAxis", y);
        characterAnim.SetFloat("VerticalVelocity", yVel);
    }

    public void SetTrigger(string trigger)
    {
        characterAnim.SetTrigger(trigger);
    }

    public void Flip(int side)
    {
        
            if (side == -1 && sr.flipX)
                return;

            if (side == 1 && !sr.flipX)
            {
                return;
            }
        

        bool state = (side == 1) ? false : true;
        sr.flipX = state;
    }
}
