using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject bonsaiTree1;
    public GameObject bonsaiTree2;
    public GameObject bonsaiTree3;
    public GameObject bonsaiTree4;

    public bool playerWin = false;


    public int herbsGathered;

	// Use this for initialization
	void Start ()
    {

		
	}
	
	// Update is called once per frame
	void Update ()
    {

        herbsGathered = player.GetComponent<PlayerController>().numberOfHerbs;

        if (herbsGathered == 2)
        {
            bonsaiTree1.SetActive(false);
            bonsaiTree2.SetActive(true);
        }
        if (herbsGathered == 3)
        {
            bonsaiTree2.SetActive(false);
            bonsaiTree3.SetActive(true);
        }
        if (herbsGathered == 4)
        {
            bonsaiTree3.SetActive(false);
            bonsaiTree4.SetActive(true);
            playerWin = true;
        }
    }
}
