using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour 
{
    [SerializeField]
    private float speed;
    //[SerializeField]
    //private GameObject[] spinners;

    private void Awake()
    {
        //Transform[] children = GetComponentsInChildren<Transform>();
        //spinners = new GameObject[children.Length];

        //for (int i = 0; i < spinners.Length; i++)
        //{
        //    spinners[i] = children[i].gameObject;
        //}
    }


    void Update () 
    {
        //for (int i = 0; i < spinners.Length; i++)
        //{
        //    if (spinners[i] != null)
        //    {
        //        spinners[i].transform.eulerAngles += new Vector3 (0f, 0f, speed);
        //    }
        //}

        float dt = Time.deltaTime;
        transform.eulerAngles += new Vector3(0f, 0f, -speed * dt);
	}
}