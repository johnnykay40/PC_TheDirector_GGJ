using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterMovementGeneral : MonoBehaviour
{
    private Collision collision;
    [HideInInspector]
    public Rigidbody2D characterRb;
    private AnimationScript characterAnim;

    [Space]

    [Header("Stats")]
    [SerializeField] private float characterSpeed = 10;
    [SerializeField] private float characterJumpForce = 50;
    [SerializeField] private float characterSlideSpeed = 5;
    [SerializeField] private float characterWallJumpLerp = 10;
    [SerializeField] private float characterDashSpeed = 20;

    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;

    [Space]

    private bool groundTouch;
    private bool hasDashed;

    public int side = 1;

    [Space]
    [Header("Polish")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;

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

        if (collision.onWall && Input.GetButton("Fire3") && canMove)
        {
            if (side != collision.wallSide)
                characterAnim.Flip(side * -1);
            wallGrab = true;
            wallSlide = false;
        }

        if (Input.GetButtonUp("Fire3") || !collision.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (collision.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJumping>().enabled = true;
        }

        if (wallGrab && !isDashing)
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

        if (collision.onWall && !collision.onGround)
        {
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (!collision.onWall || collision.onGround)
            wallSlide = false;

        if (Input.GetButtonDown("Jump"))
        {
            characterAnim.SetTrigger("jump");

            if (collision.onGround)
                Jump(Vector2.up, false);
            if (collision.onWall && !collision.onGround)
                WallJump();
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

        WallParticle(y);

        if (wallGrab || wallSlide || !canMove)
            return;

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
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

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
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        dashParticle.Stop();
        characterRb.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (collision.onGround)
            hasDashed = false;
    }

    private void WallJump()
    {
        if ((side == 1 && collision.onRightWall) || side == -1 && !collision.onRightWall)
        {
            side *= -1;
            characterAnim.Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = collision.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        wallJumped = true;
    }

    private void WallSlide()
    {
        if (collision.wallSide != side)
            characterAnim.Flip(side * -1);

        if (!canMove)
            return;

        bool pushingWall = false;
        if ((characterRb.velocity.x > 0 && collision.onRightWall) || (characterRb.velocity.x < 0 && collision.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : characterRb.velocity.x;

        characterRb.velocity = new Vector2(push, -characterSlideSpeed);
    }

    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            characterRb.velocity = new Vector2(dir.x * characterSpeed, characterRb.velocity.y);
        }
        else
        {
            characterRb.velocity = Vector2.Lerp(characterRb.velocity, (new Vector2(dir.x * characterSpeed, characterRb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    private void Jump(Vector2 dir, bool wall)
    {
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        characterRb.velocity = new Vector2(characterRb.velocity.x, 0);
        characterRb.velocity += dir * characterJumpForce;

        particle.Play();
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

    void WallParticle(float vertical)
    {
        var main = slideParticle.main;

        if (wallSlide || (wallGrab && vertical < 0))
        {
            slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            main.startColor = Color.white;
        }
        else
        {
            main.startColor = Color.clear;
        }
    }

    int ParticleSide()
    {
        int particleSide = collision.onRightWall ? 1 : -1;
        return particleSide;
    }
}

