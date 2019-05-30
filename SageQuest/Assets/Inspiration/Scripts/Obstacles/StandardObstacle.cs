using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basic obstacle that doesn't move and slows the player.

public class StandardObstacle : MonoBehaviour, IObstacle
{
    //[Range(0, 1)]
    //public float slowPercent;
    [SerializeField]
    private int playerLayer = 9;
    bool hit;
    public AudioClip HurtSound;
    public void OnPlayerHit(RigidPlayer player)
    {
        player.SlowPlayer();
        if (!hit)
        {
            SoundManager.instance.Play(HurtSound, "sfx");
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