using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private new Camera camera;

    [SerializeField] private Transform followTarget;

    [SerializeField] private float topOfLevel;
    [SerializeField] public float bottomOfLevel;
    [SerializeField] public float leftOfLevel;
    [SerializeField] public float rightOfLevel;
    [SerializeField] public Material backgroundMaterial;
    private Plane[] planes;

    [SerializeField]
    private float X_Offset;
    [SerializeField]
    private float Y_Offset;
    [SerializeField]
    private float Z_Offset;
    private GameObject player;
    private GameObject backPlane;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        camera = GetComponent<Camera>();
        if (followTarget == null)
        {
            Debug.Log("Follow target was null, defaulting to Rung");
            followTarget = player.transform;
        }
        
    }

    void LateUpdate()
    {
        //backPlane.transform.position = new Vector3 (Camera.main.transform.position.x, backPlane.transform.position.y, backPlane.transform.position.z);
        planes = GetPlanes();
        float x = Mathf.Clamp(followTarget.position.x, leftOfLevel + (camera.transform.position - planes[0].ClosestPointOnPlane(camera.transform.position)).x,
                                                                      rightOfLevel - (planes[1].ClosestPointOnPlane(camera.transform.position) - camera.transform.position).x);
        float y = Mathf.Clamp(followTarget.position.y, bottomOfLevel + camera.orthographicSize, topOfLevel - camera.orthographicSize);
        transform.position = new Vector3(x + X_Offset, y + Y_Offset, Z_Offset);
    }

    private Plane[] GetPlanes()
    {
        return GeometryUtility.CalculateFrustumPlanes(camera);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector2(leftOfLevel, topOfLevel), new Vector2(rightOfLevel, topOfLevel));
        Gizmos.DrawLine(new Vector2(leftOfLevel, bottomOfLevel), new Vector2(rightOfLevel, bottomOfLevel));
        Gizmos.DrawLine(new Vector2(leftOfLevel, topOfLevel), new Vector2(leftOfLevel, bottomOfLevel));
        Gizmos.DrawLine(new Vector2(rightOfLevel, topOfLevel), new Vector2(rightOfLevel, bottomOfLevel));
    }

    public Vector3 ReturnOffset()
    {
        return new Vector3(X_Offset, Y_Offset, Z_Offset);
    }

}