/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingEdging : MonoBehaviour
{
   
    [SerializeField]
    private float raycastDistance= 1;
    [SerializeField]
    private LayerMask hitLayer;
   
    [SerializeField]
    private Vector3 forceDown;
    private Rigidbody rb;

    [SerializeField]
    private float seconds;
	
	void Start () 
    {
       rb = GetComponent<Rigidbody>();
       
	}
	

    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, Vector3.right);
        RaycastHit[] hits = Physics.RaycastAll (ray, raycastDistance, hitLayer);

        Debug.DrawLine(transform.position, transform.position + Vector3.right * raycastDistance, Color.green);


        foreach(RaycastHit hit in hits)
        {
            
            Debug.Log("You touched something");
            StartCoroutine (EdgeBug());


        }

    }

    IEnumerator EdgeBug()
    {
        
            
            Debug.Log("You touched something");

            yield return new WaitForSeconds(seconds);

            rb.AddForce (forceDown , ForceMode.Impulse);


    }

}*/
