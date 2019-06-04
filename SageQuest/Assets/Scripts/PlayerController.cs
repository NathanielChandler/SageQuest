using UnityEngine;
using System;


public class PlayerController : MonoBehaviour
{
    //Animation Stuff
    public Animator animator;

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;
    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;
    public ContactPoint2D[] contactPoints;
    public ContactFilter2D contactFilter;
    public int numberOfHerbs = 0;

    private bool grounded = false;
    private bool positiveXCollision = false;
    private bool negativeXCollision = false;
    private Rigidbody2D rigidBody;
    private bool onPlatform = false;


    // Use this for initialization
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        contactPoints = new ContactPoint2D[10];
        rigidBody.GetContacts(contactPoints);

        if (contactPoints[0].collider != null && !onPlatform)
        {

            foreach (ContactPoint2D point in contactPoints)
            {
                if (point.normal.y == 1 && point.collider.CompareTag("Solid"))
                {
                    grounded = true;
                    break;
                }
                else
                {
                    grounded = false;
                }
            }

            foreach (ContactPoint2D point in contactPoints)
            {
                if(point.normal.x < 0 && point.collider.CompareTag("Solid"))
                {
                    positiveXCollision = true;
                    break;
                } 
                else
                {
                    positiveXCollision = false;
                }

                if (point.normal.x > 0 && point.collider.CompareTag("Solid"))
                {
                    negativeXCollision = true;
                    break;
                }
                else
                {
                    negativeXCollision = false;
                }
            }
        }
        else
        {
            positiveXCollision = false;
            negativeXCollision = false;
            grounded = onPlatform;
        }


        if (Input.GetButtonDown("Jump") && grounded)
        {
            animator.SetBool("IsJumping", true);
            rigidBody.AddForce(new Vector2(0f, jumpForce));
            grounded = false;
        }

    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        if (!positiveXCollision && !negativeXCollision)
        {
            //Speed up player to max Speed
            if (horizontalInput * rigidBody.velocity.x < maxSpeed)
                rigidBody.AddForce(Vector2.right * horizontalInput * moveForce);

            if (Mathf.Abs(rigidBody.velocity.x) > maxSpeed)
                rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * maxSpeed, rigidBody.velocity.y);

            // Slow Down Player
            if (horizontalInput == 0 && rigidBody.velocity.x != 0)
            {
                if (rigidBody.velocity.x  > 1)
                {
                    rigidBody.AddForce(Vector2.left * (moveForce / 2));
                }
                else if (rigidBody.velocity.x < -1)
                {
                    rigidBody.AddForce(Vector2.right * (moveForce / 2));
                }
                else
                {
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                }
            }
        }

        else if (positiveXCollision && horizontalInput < 0)
        {
            rigidBody.AddForce(Vector2.right * horizontalInput * moveForce);
        }

        else if (negativeXCollision && horizontalInput > 0)
        {
            rigidBody.AddForce(Vector2.right * horizontalInput * moveForce);
        }

        //Flip Player Model
        if (horizontalInput > 0 && !facingRight)
            Flip();
        else if (horizontalInput < 0 && facingRight)
            Flip();

        
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Moving Platform"))
        {
            rigidBody.transform.SetParent(collision.gameObject.transform);
            onPlatform = true;
            animator.SetBool("IsJumping", false);
        }
        if (collision.gameObject.name.Contains("spinning"))
        {
            onPlatform = true;
            animator.SetBool("IsJumping", false);
        }
        if (collision.gameObject.name.Contains("Ground"))
        {
            animator.SetBool("IsJumping", false);
        }
        if (collision.gameObject.name.Contains("Herb"))
        {
            numberOfHerbs += 1;
        }

        Debug.Log(grounded);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Moving Platform"))
        {
            rigidBody.transform.SetParent(null);
            onPlatform = false;
        }
    }
}