using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isFacingRight = true;

    [Header("Collision Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Movement Physics")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpSpeed;

    void Awake()
    {
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        bool isGrounded = CalculateIsGrounded(this.groundCheck, this.groundCheckRadius, this.whatIsGround);
        float xInput = ProcessInput(isGrounded, this.movementSpeed, this.jumpSpeed);

        // Flip player sprite if player is not facing right
        this.isFacingRight = CalculateIsFacingRight(this.isFacingRight, xInput);
        this.spriteRenderer.flipX = (!isFacingRight);

        SetAnimationParameters(this.rigidBody.velocity.x, this.rigidBody.velocity.y, isGrounded);
    }

    // Checks if there is an overlap between groundCheck circle with 'Ground' layer living in the platform.
    private bool CalculateIsGrounded(Transform groundCheck, float groundCheckRadius, LayerMask whatIsGround)
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private float ProcessInput(bool isGrounded, float movementSpeed, float jumpSpeed) // TODO jumpSpeed =? jumpVelocity, speed => velocity in general
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yVelocity = rigidBody.velocity.y;

        // Player can only jump if grounded
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded))
        {
            yVelocity = jumpSpeed;
        }

        MovePlayer(xInput, this.movementSpeed, this.rigidBody, yVelocity);

        return xInput;
    }

    private void MovePlayer(float xInput, float movementSpeed, Rigidbody2D rigidBody, float yVelocity)
    {
        Debug.Log(string.Format("Movement(xInput {0}, movementSpeed {1}, rigidBody {2})", xInput, movementSpeed, rigidBody));
        rigidBody.velocity = new Vector2(xInput * movementSpeed, yVelocity);
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

    // Project Settings -> Time -> Fixed Timestamp
    // EG: Fixed Timestamp = .02 -> 50 FixedUpdates per second
    // It is better to do any physics changes in FixedUpdate because it is guaranteed to run on the set interval and is not tied to FPS.
    private void FixedUpdate()
    {
        // TODO
    }

    /*
     * Draw a wireframe around 'groundCheck' to aid us in visualizing isGrounded check. 
     * This is a visual cue only, it's not actually used for decision-making.
     */
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}