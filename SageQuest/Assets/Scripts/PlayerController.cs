using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;
    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;
    public ContactPoint2D[] contactPoints;
    public ContactFilter2D contactFilter;

    private bool grounded = false;
    private bool collided = false;
    private Rigidbody2D rigidBody;


    // Use this for initialization
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        contactPoints = new ContactPoint2D[2];
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.GetContacts(contactPoints);

        if (contactPoints.Length > 0) {
            grounded = contactPoints[0].normal.y == 1;
            collided = contactPoints[0].normal.x != 0;
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (!collided){ 
            //Speed up player to max Speed
            if (horizontalInput * rigidBody.velocity.x < maxSpeed)
                rigidBody.AddForce(Vector2.right * horizontalInput * moveForce);

            if (Mathf.Abs(rigidBody.velocity.x) > maxSpeed)
                rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * maxSpeed, rigidBody.velocity.y);

            // Slow Down Player
            if (horizontalInput == 0 && rigidBody.velocity.x != 0)
            {
                if (rigidBody.velocity.x > 1)
                {
                    rigidBody.AddForce(Vector2.left * (moveForce / 4));
                }
                else if (rigidBody.velocity.x < -1)
                {
                    rigidBody.AddForce(Vector2.right * (moveForce / 4));
                }
                else
                {
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                }
            }
        }
        //Flip Player Model
        if (horizontalInput> 0 && !facingRight)
            Flip();
        else if (horizontalInput< 0 && facingRight)
            Flip();

        if (jump)
        {
            rigidBody.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
