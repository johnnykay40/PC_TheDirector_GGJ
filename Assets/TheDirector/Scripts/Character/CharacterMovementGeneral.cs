using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterMovementGeneral : MonoBehaviour
{
    private Collision collision;
   
    public Rigidbody2D characterRb;
    private AnimationScript characterAnim;

    [Space]

    [Header("Stats")]
    [SerializeField] private float characterSpeed;
    [SerializeField] private float characterJumpForce;
    [SerializeField] private float characterSlideSpeed;
    [SerializeField] private float characterWallJumpLerp;
    [SerializeField] private float characterDashSpeed;

    [Space]
    [Header("Booleans")]
    public bool canMove;
    
    
    public bool isDashing;

    [Space]

    private bool groundTouch;
    private bool hasDashed;

    public int side = 1;

    [Space]
    [Header("Polish")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    

    void Start()
    {
        collision = GetComponent<Collision>();
        characterRb = GetComponent<Rigidbody2D>();
        characterAnim = GetComponentInChildren<AnimationScript>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        characterAnim.SetHorizontalMovement(x, y, characterRb.velocity.y);
              

        if (collision.onGround && !isDashing)
        {            
            GetComponent<BetterJumping>().enabled = true;
        }

        if (!isDashing)
        {
            characterRb.gravityScale = 0;
            if (x > .2f || x < -.2f)
                characterRb.velocity = new Vector2(characterRb.velocity.x, 0);

            float speedModifier = y > 0 ? .5f : 1;

            characterRb.velocity = new Vector2(characterRb.velocity.x, y * (characterSpeed * speedModifier));
        }
        else
        {
            characterRb.gravityScale = 3;
        }        

        if (Input.GetButtonDown("Jump"))
        {
            characterAnim.SetTrigger("jump");

            if (collision.onGround)
                Jump(Vector2.up, false);            
        }

        if (Input.GetButtonDown("Fire1") && !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw);
        }

        if (collision.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!collision.onGround && groundTouch)
        {
            groundTouch = false;
        }                

        if (x > 0)
        {
            side = 1;
            characterAnim.Flip(side);
        }
        if (x < 0)
        {
            side = -1;
            characterAnim.Flip(side);
        }

    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = characterAnim.sr.flipX ? -1 : 1;

        jumpParticle.Play();
    }

    private void Dash(float x, float y)
    {
        
        hasDashed = true;

        characterAnim.SetTrigger("dash");

        characterRb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        characterRb.velocity += dir.normalized * characterDashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

        dashParticle.Play();
        characterRb.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;        
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        dashParticle.Stop();
        characterRb.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;        
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (collision.onGround)
            hasDashed = false;
    }    
        

    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;        
    }

    private void Jump(Vector2 dir, bool wall)
    {
        
        characterRb.velocity = new Vector2(characterRb.velocity.x, 0);
        characterRb.velocity += dir * characterJumpForce;
        
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void RigidbodyDrag(float x)
    {
        characterRb.drag = x;
    }
    
    }

    


