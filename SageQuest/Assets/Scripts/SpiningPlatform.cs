using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiningPlatform : MonoBehaviour
{
    public float speed = 1f;
    public bool rotateLeft = false;
	
    // Use this for initialization
	void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        RotateDirection();
    }

    void RotateDirection()
    {
        if (rotateLeft == false)
        {
            transform.Rotate(Vector3.back * speed * Time.deltaTime);
        }
        if (rotateLeft == true)
        {
            transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
