using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {
	
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

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");

		camera = GetComponent<Camera>();
		if (followTarget == null)
		{
			Debug.Log("Follow target was null, defaulting to Rung");
			followTarget = player.transform;
		}

        /*Plane[] planes = GeometryUtility.CalculateFrustumPlanes (Camera.main);

        GameObject p = GameObject.CreatePrimitive (PrimitiveType.Plane);
        p.name = "Background Plane";
        p.transform.position = -planes[5].normal * planes[5].distance;
        p.transform.rotation = Quaternion.FromToRotation (Vector3.up, planes[5].normal);
		p.transform.localScale = new Vector3 (310f, 1f, 200f);
		p.GetComponent<Renderer> ().material = backgroundMaterial;
		backPlane = p;
		backPlane.transform.parent = Camera.main.transform;
		Camera.main.GetComponent<Camera> ().farClipPlane = 1500f;*/
	}

	void LateUpdate () {
		//backPlane.transform.position = new Vector3 (Camera.main.transform.position.x, backPlane.transform.position.y, backPlane.transform.position.z);
		planes = GetPlanes();
		float x = Mathf.Clamp(followTarget.position.x, leftOfLevel + (camera.transform.position - planes[0].ClosestPointOnPlane(camera.transform.position)).x,
																	  rightOfLevel - (planes[1].ClosestPointOnPlane(camera.transform.position) - camera.transform.position).x);
		float y = Mathf.Clamp(followTarget.position.y, bottomOfLevel + camera.orthographicSize, topOfLevel - camera.orthographicSize);
		transform.position = new Vector3(x + X_Offset, y + Y_Offset, Z_Offset);
		/*if (followTarget.GetComponent<RigidPlayer> ().freezeDelay) {
			transform.position = new Vector3 (transform.position.x + followTarget.GetComponent<RigidPlayer> ().transform.position.x, transform.position.y + followTarget.GetComponent<RigidPlayer> ().transform.position.y, transform.position.z + followTarget.GetComponent<RigidPlayer> ().transform.position.z);
		}*/
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