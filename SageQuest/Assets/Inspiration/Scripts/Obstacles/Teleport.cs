using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    private Camera cam;
    [SerializeField]
    private RigidPlayer player;
    [SerializeField]
    public float slowDown = 0.2f;
    [SerializeField]
    private float slowDownLength = 2f;
    [SerializeField]
    private float counter = 0f;
    [SerializeField]
    private float slowMoTimeAllowed = 4;

    private Vector3 camOffset;

    private CameraFollower camFollow;

    void Awake()
    {
        slowMoTimeAllowed *= slowDown;
        player = FindObjectOfType<RigidPlayer>();
        cam = FindObjectOfType<Camera>();
        camFollow = cam.GetComponent<CameraFollower>();
        camOffset = camFollow.ReturnOffset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, -cam.transform.position.z));
            Vector3 teleportPosition = new Vector3(mousePos.x, mousePos.y, 0);
            player.transform.position = teleportPosition;
            Time.timeScale = slowDown;
            //Time.fixedDeltaTime = 0.02f * Time.timeScale;
            Debug.Log(Time.timeScale);
            StartCoroutine(SlowTimer());
        }

    }


    IEnumerator SlowTimer()
    {
        camFollow.enabled = false;
        float startTime = Time.time;
        Vector3 startCamPos = cam.transform.position;
        while (Time.time - startTime < slowMoTimeAllowed)
        {
            cam.transform.position = Vector3.Lerp(startCamPos,
            player.transform.position + camOffset, (Time.time - startTime) / slowMoTimeAllowed);
            yield return null;
        }
        //yield return new WaitForSeconds(slowMoTimeAllowed);
        camFollow.enabled = true;
        Time.timeScale = 1;
        Debug.Log(Time.timeScale);
    }
}
