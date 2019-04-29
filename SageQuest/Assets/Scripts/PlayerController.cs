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
    // private bool collided = false;
    private bool positiveXCollision = false;
    private bool negativeXCollision = false;
    private Rigidbody2D rigidBody;


    // Use this for initialization
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        contactPoints = new ContactPoint2D[10];
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.GetContacts(contactPoints);

        if (contactPoints.Length > 0) {

            foreach (ContactPoint2D point in contactPoints)
            {
                if(point.normal.y == 1 && point.collider.CompareTag("Solid"))
                {
                    grounded = true;
                    break;
                } else
                {
                    grounded = false;
                }
            }

            // grounded = (contactPoints[0].normal.y == 1 || contactPoints[1].normal.y == 1) && contactPoints[0].collider.CompareTag("Solid");
            // collided = contactPoints[0].normal.x != 0 && contactPoints[0].collider.CompareTag("Solid");
            positiveXCollision = contactPoints[0].normal.x < 0 && contactPoints[0].collider.CompareTag("Solid");
            negativeXCollision = contactPoints[0].normal.x > 0 && contactPoints[0].collider.CompareTag("Solid");
        } else
        {
            grounded = false;
            positiveXCollision = false;
            negativeXCollision = false;
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
            grounded = false;
        }

        Debug.Log(grounded);
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (!positiveXCollision && !negativeXCollision){ 
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

        if(!positiveXCollision && horizontalInput < 0)
        {
            rigidBody.AddForce(Vector2.right * horizontalInput * moveForce);
        }

        if (!negativeXCollision && horizontalInput > 0)
        {
            rigidBody.AddForce(Vector2.right * horizontalInput * moveForce);
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
