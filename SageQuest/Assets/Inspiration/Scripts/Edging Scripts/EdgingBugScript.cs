/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//add this into "rigidplayer" 
public class EdgingBugScript : MonoBehaviour
{
   
    [SerializeField]
    private float forceDown;
    [SerializeField]
//<<<<<<< HEAD
    private LayerMask hitLayer;
   
    SerializeField]
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
//<<<<<<< HEAD
        //RaycastHit[] hits = Physics.RaycastAll (ray, raycastDistance, hitLayer);

        RaycastHit[] hits = Physics.RaycastAll (ray, raycastDistance, hitLayer);


//=======
       // RaycastHit[] hits = Physics.RaycastAll (ray, raycastDistance, hitLayer);

//>>>>>>> master
        Debug.DrawLine(transform.position, transform.position + Vector3.right * raycastDistance, Color.green);


        foreach(RaycastHit hit in hits)
        {
            
            Debug.Log("You touched something");
            StartCoroutine (EdgeBug());


        }

//<<<<<<< HEAD

//        if (Physics.SphereCast(rb, RigidPlayer.height/2, transform.right, out hit, 10));
       // {
            
        //}

//=======
//>>>>>>> master
    }

    IEnumerator EdgeBug()
    {
        
            
            Debug.Log("You touched something");

            yield return new WaitForSeconds(seconds);

            rb.AddForce (forceDown , ForceMode.Impulse);

//=======
    //private int seconds;


    private RaycastHit hit;
    private int count;
    private bool isCounting = false;
    private GameObject player;

    void Update () {
        Debug.DrawRay (transform.position, Vector3.left, Color.green, 1f);
        if (Physics.SphereCast (transform.position, 1f, Vector3.left, out hit, 1f)) {
            if (hit.transform.gameObject.tag == "Player") {
                isCounting = true;
                player = hit.transform.gameObject;
            } else {
                isCounting = false;
                count = 0;
            }
        }

        if (isCounting) {
            count++;
        }
//>>>>>>> master

        if (count >= seconds * 30) {
            Debug.Log ("MOVE");
            player.GetComponent<Rigidbody> ().AddForce (new Vector3 (0f, -forceDown * 10f, 0f) , ForceMode.Impulse);
            count = 0;
            isCounting = false;
        }
    }
}*/