using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private bool elevating = false;

    [SerializeField]
    [Tooltip("Controls whether or not elevator moves back and forth between two positions")]
    private bool pinging = false;

    /*[SerializeField]
    [Tooltip("Start point for bouncing")]
    private Vector3 startPoint;

    [SerializeField]
    [Tooltip("End point for bouncing")]
    private Vector3 endPoint;*/
    private Vector3 startPos;

    [SerializeField]
    [Tooltip("Limit for ping pong only")]
    private float distance;

    [SerializeField]
    [Tooltip("Controls the rate at which the elevator moves, negative values moves the elevator downwards")]
    private float elevationSpeed = 1f;

    [SerializeField]
    [Tooltip("Controls direction of movement")]
    private Vector3 elevateDirection = Vector2.up;

    

    private void OnEnable()
    {
        startPos = transform.position;
        CheckMode();
    }

    private void CheckMode()
    {
        if (elevating == true)
        {
            if (pinging)
            {
                Pinging();
            }
            else
            {
                Elevate();
            }

        }
    }

    private void Pinging()
    {
        StartCoroutine(PingPong());
    }

    private void Elevate()
    {
        // Put audio here
        StartCoroutine(Elevating());
    }

    IEnumerator Elevating()
    {
        
        while (elevating)
        {
            transform.position += elevateDirection * elevationSpeed * Time.fixedDeltaTime;
            yield return null;
        }
    }

    IEnumerator PingPong()
    {
        Vector3 dir = elevateDirection.normalized;
        while (pinging)
        {
            transform.position = startPos + (dir * Mathf.PingPong(Time.time * elevationSpeed, distance));
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Eh?");
        //Debug.Log(collision.gameObject.name);
        if (elevating == false)
        {
            elevating = true;
            CheckMode();
        }
    }

    /*IEnumerator Detecting()
    {
        RaycastHit hit;
        Vector3 rayDir = endRay.transform.position - startRay.transform.position;
        float distance = Vector3.Distance(endRay.transform.position, startRay.transform.position);
        while (!elevating)
        {
            if (Physics.Raycast(startRay, rayDir, out hit, distance))
            {

            }
            yield return null;
        }
    }*/

}
