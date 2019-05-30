using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public Renderer rend;
    public Rigidbody rb;
    public float blinkTime = 0.01f;
    public AudioClip warning;

	void Start ()
    {
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
	}

     void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.tag == "Player")
        //{
        //    StartCoroutine("BlinkThenFall");
        //}

        transform.parent = transform;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SoundManager.instance.Play(warning, "sfx");
            StartCoroutine("BlinkThenFall");
        }
    }
	
    IEnumerator BlinkThenFall()
    {
        for (int i = 0; i < 3; i++)
        {
            rend.enabled = false;
            yield return new WaitForSeconds(blinkTime);
            rend.enabled = true;
            yield return new WaitForSeconds(blinkTime);
        }
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
    }
}
