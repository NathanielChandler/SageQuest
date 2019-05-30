using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour {

	public GameObject player;
	public float robertX = 550f;
	public float chrisX = 600f;
	public float darrenX = 1510f;
	public float oscarX = 330f;
	public float pedroX = 525f;
	public int levelIndex = 0;

	void Update () {
		if (player != null) {
			switch (levelIndex) {
				case 0:
					if (player.transform.position.x >= robertX) {
						//SceneManager.LoadScene ("1-2");
						levelIndex++;
					}
				break;
				case 1:
					if (player.transform.position.x >= chrisX) {
						//SceneManager.LoadScene ("1-3 V2");
						levelIndex++;
					}
				break;
				case 2:
					if (player.transform.position.x >= darrenX) {
						//SceneManager.LoadScene ("Level 1-4 (Remaster)");
						levelIndex++;
					}
				break;
				case 3:
					if (player.transform.position.x >= oscarX) {
						//SceneManager.LoadScene ("1-5 remaster");
						levelIndex++;
					}
				break;
				case 4:
					if (player.transform.position.x >= pedroX) {
						//SceneManager.LoadScene ("1-1RobertPattersonLevels");
						levelIndex++;
					}
				break;
			}
		}
		player = GameObject.FindGameObjectWithTag ("Player");
	}

}