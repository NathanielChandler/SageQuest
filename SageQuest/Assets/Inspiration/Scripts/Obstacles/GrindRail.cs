using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindRail : MonoBehaviour
{

    [SerializeField]
    private Vector3 startPosition;

    [Tooltip("This will determine where the player will drop off if they never jump off")]
    [SerializeField]
    private Transform endTransform;

    [SerializeField]
    private float yOffset = 1f;

    [SerializeField]
    private int playerLayer;

    private RigidPlayer player;

    private Transform targetTransform;

    [SerializeField]
    private bool grinding = false;

    [SerializeField]
    private Collider thisColl;


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == playerLayer)
        {
            targetTransform = other.transform.root;
            player = targetTransform.GetComponent<RigidPlayer>();
            Debug.Log(other.gameObject.name);
            if (player.Skating)
            {
                Vector3 impactPoint = other.ClosestPoint(transform.position);
                float length = (impactPoint - endTransform.position).magnitude;
                startPosition = endTransform.position - (length * transform.right);
                targetTransform.position = startPosition;
                //player.ConstrainPlayer();
                //StartCoroutine(Grind());
                Solidify();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Liquify();
    }

    private void Solidify()
    {
        thisColl.isTrigger = false;
    }

    private void Liquify()
    {
        thisColl.isTrigger = true;
    }

    IEnumerator Grind()
    {
        Debug.Log("grinding");
        while (!Input.GetMouseButtonDown(0) && targetTransform.position.x < endTransform.position.x)
        {
            player.transform.position += transform.right * player.CurrentSpeed * Time.deltaTime;
            player.isOnObstacle = true;
            //Vector3 directionalBase = new Vector3(0f, 4f, 3f );
            yield return null;
        }
        player.UnConstrainPlayer();
        player.isOnObstacle = false;
    }
}
