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
    }

    public int GetValue()
    {
        
        DestroyObject(gameObject);
        return value--;
    }
}
