using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateboardRamp : MonoBehaviour
{
    [SerializeField]
    private int playerLayer = 9;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private RigidPlayer player;

    [SerializeField]
    private float decaySpeed = 0.5f;

    [Header("Settings")]
    [Tooltip("Determines how much the ramp affects speed")]
    [SerializeField]
    private float speedMultiplier = 2f;

    [SerializeField]
    private bool entered = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == playerLayer && !entered)
        {
            targetTransform = collision.transform.root;
            player = targetTransform.GetComponent<RigidPlayer>();
            if (player.Skating)
            {
                entered = true;
                player.MaxSpeed *= speedMultiplier;
                player.CurrentSpeed *= speedMultiplier;
                player.isOnObstacle = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log(targetTransform);
        if (collision.transform.root == targetTransform)
        {
            if (player.Skating && !player.Grounded)
            {
                StartCoroutine("RetainAirSpeed");
            }
            player.StartRotateBack();
        }
    }

    IEnumerator RetainAirSpeed()
    {
        player.isOnObstacle = false;
        float startMax = player.MaxSpeed;
        float startTime = Time.time;
        while (player.MaxSpeed > player.BaseMaxSpeed)
        {
            player.MaxSpeed = Mathf.Lerp(startMax, player.BaseMaxSpeed, (Time.time - startTime) * decaySpeed);
            yield return null;
        }
    }
}
