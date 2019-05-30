using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialUpModem : MonoBehaviour, IObstacle
{
    [SerializeField]
    private int playerLayer = 9;

    public AudioClip DialUp;
    bool hit;

    public void OnPlayerHit(RigidPlayer player)
    {
        player.FreezeScore();
        if (!hit)
        {
            SoundManager.instance.Play(DialUp, "sfx");
            hit = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            Transform targetTransform = other.transform.root;
            RigidPlayer player = targetTransform.GetComponent<RigidPlayer>();
            OnPlayerHit(player);
        }
    }
}
