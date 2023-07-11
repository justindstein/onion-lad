using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Collision Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Movement Physics")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpSpeed;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 inputMovementForce;
    private bool inputJump = false;

    private bool isFacingRight = true;

    void Awake()
    {
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        this.inputMovementForce = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        if (Input.GetKeyDown(KeyCode.Space)) this.inputJump = true;
    }

    /*
     * Draw a wireframe around 'groundCheck' to aid us in visualizing isGrounded check. 
     * This is a visual cue only, it's not actually used for decision-making.
     */
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void FixedUpdate()
    {
        bool isGrounded = IsGrounded(this.groundCheck, this.groundCheckRadius, this.whatIsGround);

        // Player movement
        this.rigidBody.velocity = new Vector2(inputMovementForce.x * movementSpeed, rigidBody.velocity.y);

        // Player jump
        if (this.inputJump && isGrounded)
            this.rigidBody.velocity = new Vector2(rigidBody.velocity.x, Vector2.up.y * jumpSpeed);
        this.inputJump = false;

        // Flip player sprite if player is not facing right
        this.isFacingRight = CalculateIsFacingRight(this.isFacingRight, this.rigidBody.velocity.x);
        this.spriteRenderer.flipX = (!isFacingRight);

        SetAnimationParameters(this.rigidBody.velocity.x, this.rigidBody.velocity.y, isGrounded);
    }

    // Checks if there is an overlap between groundCheck circle with 'Ground' layer living in the platform.
    private bool IsGrounded(Transform groundCheck, float groundCheckRadius, LayerMask whatIsGround)
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    /*
     * If xInput is zero, do not change direction of the player.
     * f xInput is nonzero, point the player to the right if they are moving to the right 
     * and point the player to the left if they are moving to the left.
     */
    private bool CalculateIsFacingRight(bool isFacingRight, float xInput)
    {
        return (xInput != 0)
            ? (xInput > 0)
            : isFacingRight;
    }

    private void SetAnimationParameters(float xVelocity, float yVelocity, bool isGrounded)
    {
        animator.SetFloat("xVelocity", xVelocity);
        animator.SetFloat("yVelocity", yVelocity);
        animator.SetBool("isGrounded", isGrounded);
    }

}