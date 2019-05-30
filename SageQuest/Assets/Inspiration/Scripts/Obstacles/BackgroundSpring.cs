using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpring : MonoBehaviour {

	private GameObject player;
	public float distanceToActivate;
    public AudioClip Boing;

	void Start () {
		player = GameObject.Find ("_RigidPlayer 1");
	}
	void Update () {
		if (Vector3.Distance (transform.position, player.transform.position) <= distanceToActivate) {
			player.gameObject.transform.GetComponent<RigidPlayer> ().MoveIntoBackground ();
			SoundManager.instance.Play(Boing, "sfx");
		}
	}
}