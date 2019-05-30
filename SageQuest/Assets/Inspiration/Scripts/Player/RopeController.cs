using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour {
    public Transform cableConnectedTo;
    public Transform plug;

    private LineRenderer lineRenderer;

    public List<Vector3> allCableSections = new List<Vector3> ();

    private float cableLength = 1f;
    private float plugMass = 100f;
    SpringJoint springJoint;

    void Start () {
        springJoint = cableConnectedTo.GetComponent<SpringJoint> ();
        lineRenderer = GetComponent<LineRenderer> ();
        UpdateSpring ();
        plug.GetComponent<Rigidbody> ().mass = plugMass;
    }
	
	void Update () {
        DisplayRope ();
    }

    private void UpdateSpring () {
        float density = 7750f;
        float radius = 0.02f;

        float volume = Mathf.PI * radius * radius * cableLength;

        float ropeMass = volume * density;

        ropeMass += plugMass;
        float ropeForce = ropeMass * 9.81f;

        float kRope = ropeForce / 0.01f;
        springJoint.spring = kRope * 1.0f;
        springJoint.damper = kRope * 0.8f;

        springJoint.maxDistance = cableLength;
    }

    private void DisplayRope () {
        float ropeWidth = 0.2f;

        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
        Vector3 A = cableConnectedTo.position;
        Vector3 D = plug.position;
        Vector3 B = A + cableConnectedTo.up * (-(A - D).magnitude * 0.1f);
        Vector3 C = D + plug.up * ((A - D).magnitude * 0.5f);

        BezierCurve.GetBezierCurve (A, B, C, D, allCableSections);


        Vector3 [] positions = new Vector3 [allCableSections.Count];

        for (int i = 0; i < allCableSections.Count; i++) {
            positions[i] = allCableSections[i];
        }


        lineRenderer.positionCount = positions.Length;

        lineRenderer.SetPositions(positions);
    }
}