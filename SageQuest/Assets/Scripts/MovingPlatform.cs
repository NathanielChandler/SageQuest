using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Transform platform;

    public float speed = 1f;
    public float backSpeed = 1f;
    public float distanceTravel = 1f;
    public float distanceTravel2 = 1f; //reference to send platform back

    //Checks direction the platform moves and postion
    public bool upRight = false; //true plafrom goes up false platform goes right
    
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
        distance += hasTraveledDistance ? Time.deltaTime : (-1 * Time.deltaTime);
    }

    //controls direction platform moves in
    public void CheckDirection()
    {
            if (this.upRight)
                CheckPostionUp();
            else
                CheckPostionRight();
    }

    //Sends platform up or down
    public void CheckPostionUp()
    {
        StartCoroutine((hasTraveledDistance ? GoBackUp() : MoveUp()));
    }

    //Sends Platform left or right
    public void CheckPostionRight()
    {
        StartCoroutine((hasTraveledDistance ? GoBackRight() : MoveRight()));
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