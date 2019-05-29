using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Transform platform;

    public float speed = 1f;
    public float backSpeed = 1f;
    public float distanceTravel = 1f;
    public float distanceTravel2 = 1f; //reference to send platform back

    //Checks direction the platform moves and postion
    public bool right = false;
    public bool up = false;
    public bool hasTraveledDistance = false;

    //Direction of Movement
    private Vector3 moveUp = Vector2.up;
    private Vector3 moveRight = Vector2.right;

    private Vector3 initialPosition;

    private float distance = 1f; //use as refrence for

    // Use this for initialization
    void Start()
    {
        distance = distanceTravel;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        checkDistance();
        Distance();
    }
   
    //Controls Distance traveled
    public void Distance()
    {
        CheckDirection();
        if(distance <= distanceTravel2)
        {
            hasTraveledDistance = true;
        }
        if (distance >= distanceTravel)
        {
            hasTraveledDistance = false;
        }
    }

    public void checkDistance()
    {
        if (hasTraveledDistance == true)
        {
            distance += Time.deltaTime;
        }
        if (hasTraveledDistance == false)
        {
            distance -= Time.deltaTime;
        }
    }

    //controls direction platform moves in
    public void CheckDirection()
    {
        if (up == true)
        {
            CheckPostionUp();
        }
        else if (right == true)
        {
            CheckPostionRight();
        }
    }

    //Sends platform up or down
    public void CheckPostionUp()
    {
        if (hasTraveledDistance == false)
        {
            StartCoroutine(MoveUp());
        }
        else if (hasTraveledDistance == true)
        {
            StartCoroutine(GoBackUp());
        }
    }

    //Sends Platform left or right
    public void CheckPostionRight()
    {
        if (hasTraveledDistance == false)
        {
            StartCoroutine(MoveRight());
        }
        else if (hasTraveledDistance == true)
        {
            StartCoroutine(GoBackRight());
        }
    }

    IEnumerator MoveRight()
    {
        transform.position += moveRight * speed * Time.fixedDeltaTime;
        yield return null;
    }

    IEnumerator MoveUp()
    {
        transform.position += moveUp * speed * Time.fixedDeltaTime;
        yield return null;
    }

    //Sends platform back to inital postion
    IEnumerator GoBackUp()
    {
        transform.position += moveUp * backSpeed * Time.fixedDeltaTime;
        yield return null;
    }

    //Sends platform back to inital postion
    IEnumerator GoBackRight()
    {
        transform.position += moveRight * backSpeed * Time.fixedDeltaTime; 
        yield return null;
    }
}