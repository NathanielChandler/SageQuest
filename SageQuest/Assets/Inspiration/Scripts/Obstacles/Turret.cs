using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Turret : MonoBehaviour
{
    [SerializeField]
    private GameObject[] bullets;
    [SerializeField]
    private bool firing = true;
    [SerializeField]
    private float fireRate = 1f;

    private void OnEnable()
    {
        StartCoroutine(Firing());
    }

    IEnumerator Firing()
    {
        int bulletIndex = 0;
        while (firing)
        {
            bullets[bulletIndex].transform.localPosition = Vector3.zero;
            bullets[bulletIndex].transform.localEulerAngles = Vector3.zero;
            bullets[bulletIndex].SetActive(true);
            bulletIndex++;
            if (bulletIndex > bullets.Length - 1)
            {
                bulletIndex = 0;
            }
            yield return new WaitForSeconds(1f / fireRate);
        }
    }

}
	