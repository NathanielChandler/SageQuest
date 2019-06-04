using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbScript : MonoBehaviour
{
    public int value = 1;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            value--;
        }
        if(value == 0)
        {
            Destroy(gameObject);
        }
    }

    public int GetValue()
    {
        if(value > 0)
        {
            return value;
        }
        else
        {
            return 0;
        }
    }
}
