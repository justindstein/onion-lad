using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    //public Rigidbody2D rb; // Shows up in inspector and is editable
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    [SerializeField] private float moveSpeed; // Shows up in inspector but other scripts won't be able to access it
    [SerializeField] private float jumpForce;

    private float xInput; // Does not appear in inspector

    [Header("Collision Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Retrieving field without clicking and dragging in inspector UI
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame / 60 fps -> 60 updates per second, 120 fps -> 120 updates per second
    // FPS can vary
    void Update()
    {
        AnimationController(); // TODO: this should happen after everything else, isGrounded
        FlipController();

        CollisionChecks();
        xInput = Input.GetAxisRaw("Horizontal"); // versus Input = Input.GetAxis("Horizontal");
        Movement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void AnimationController()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void FlipController()
    {
        if (xInput != 0)
            facingRight = (xInput > 0);

        //transform.Rotate(0, 180, 0);
        sr.flipX = (!facingRight);
    }

    private void Jump()
    {
        if(isGrounded)
            rb.velocity = new Vector2(xInput * moveSpeed, jumpForce);
    }

    private void Movement()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }

    // Project Settings -> Time -> Fixed Timestamp
    // EG: Fixed Timestamp = .02 -> 50 FixedUpdates per second
    // It is better to do any physics changes in FixedUpdate because it is guaranteed to run on the set interval and is not tied to FPS.
    private void FixedUpdate() 
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log("Space was pressed");
        //}

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    Debug.Log("Space is pressed");
        //}

        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    Debug.Log("Space was released");
        //}
    }


    // Draw a wireframe around 'groundCheck' to aid us in visualizing isGrounded check. This is a visual cue only,
    // // is not actually used for decision-making.
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    // Checks if there is an overlap between groundCheck circle with 'Ground' layer living in the platform.
    private void CollisionChecks()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
}
