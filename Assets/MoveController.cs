using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    //public Rigidbody2D rb; // Shows up in inspector and is editable
    public Rigidbody2D rb;

    [SerializeField] private float moveSpeed; // Shows up in inspector but other scripts won't be able to access it
    [SerializeField] private float jumpForce;

    private float xInput; // Does not appear in inspector
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Retrieving field without clicking and dragging in inspector UI
    }

    // Update is called once per frame / 60 fps -> 60 updates per second, 120 fps -> 120 updates per second
    // FPS can vary
    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        Movement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
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
}
