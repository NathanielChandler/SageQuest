using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed = 1f;

    [SerializeField]
    private int playerLayer = 9;

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * bulletSpeed);
    }

	private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.layer == 9)
        {
            Transform targetTransform = collision.transform.root;
            targetTransform.GetComponent<RigidPlayer>().SlowPlayer();
        }
        gameObject.SetActive(false);
	}

	/*private void OnCollisionEnter(Collision other)
	{
        if(other.gameObject.layer == 9)
        {
            Transform targetTransform = other.transform.root;
            targetTransform.GetComponent<RigidPlayer>().SlowPlayer();
        }
        gameObject.SetActive(false);
	}*/
}
