using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "JetPack Settings", menuName = "JetPackSettings", order = 1)]
public class JetpackSettings : ScriptableObject
{

    [Range(0.1f, 2f)]
    [SerializeField]
    [Tooltip("Controls how quickly fuel is drained")]
    public float fuelUsage = 0.5f;

    [Range(0.1f, 2f)]
    [SerializeField]
    [Tooltip("Controls how quickly fuel is refilled")]
    public float fuelFill = 0.5f;

    [SerializeField]
    public float verticalAcceleration;
    //private bool acceleratingJetpack = false;
    public bool packing = true;

    [SerializeField]
    public ParticleSystem partSys;

    [SerializeField]
    public Transform fuelBar;

    [SerializeField]
    public AudioSource jetpackAudio;

    [SerializeField]
    public GameObject jetpackPrefab;

    [SerializeField]
    public Vector3 jetPackPos = new Vector3(-0.64f, 0f, 0f);

}
