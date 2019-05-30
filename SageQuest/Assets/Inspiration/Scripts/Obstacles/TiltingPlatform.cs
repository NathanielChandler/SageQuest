using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltingPlatform : MonoBehaviour
{
   // public float tiltSpeed = 1.0f;
    public Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	
	void OnCollisionEnter (Collision collision)
    {
        /*
        if (collision.collider.tag == "Player")
        {
            rigidbody.AddRelativeForce(-Vector3.up * tiltSpeed);
        }
        */
        rb.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
    }
}
