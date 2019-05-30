using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skateboard : MonoBehaviour
{
    //If an obstacle is rideable make it an IRideable
    //[SerializeField]
    //private 

    [SerializeField]
    [Tooltip("The player prefab that's riding the skateboard, its not visible")]
    private GameObject skateboardPlayer;

    [SerializeField]
    private GameObject skateBoardPrefab;

    [SerializeField]
    private GameObject normalPlayer;

    [SerializeField]
    private GameObject skateboard;

    public bool isRiding = false;
    private bool skating = true;


    private void OnEnable()
    {
        StartCoroutine(TrackSwipe());
    }

    IEnumerator TrackSwipe()
    {
        while (skating)
        {
            //Input.GetKeyDown(KeyCode.DownArrow) && player.CharacterController.isGrounded
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    isRiding = true;
                    skateboard.SetActive(true);
                    normalPlayer.SetActive(false);
                    skateboardPlayer.SetActive(true);
                    skateboard.SetActive(true);

                }
                else
                {
                    isRiding = false;
                    skateboard.SetActive(false);
                    normalPlayer.SetActive(true);
                    skateboardPlayer.SetActive(false);
                    skateboard.SetActive(false);
                }

            }
            yield return null;
        }

    }

}