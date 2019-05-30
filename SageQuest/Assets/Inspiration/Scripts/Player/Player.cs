using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private CharacterController characterController;

    [Header("Physics")]
    [SerializeField]
    private float drag = 0.35f;
    [SerializeField]
    private float gravity = 9.81f; 
    public Vector3 velocity = Vector3.zero;

    [Header("Movement")]
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float jumpPower;

    [Header("Jetpack")]
    //[Header("Fuel Tracking")]
    [SerializeField]
    private float remainingFuel = 100f;

    [Range(0.1f, 2f)]
    [SerializeField]
    [Tooltip("Controls how quickly fuel is drained")]
    private float fuelUsage = 0.5f;

    [Range(0.1f, 2f)]
    [SerializeField]
    [Tooltip("Controls how quickly fuel is refilled")]
    private float fuelFill = 0.5f;

    [SerializeField]
    private float verticalAcceleration;
    //private bool acceleratingJetpack = false;
    private bool packing = true;

    [Header("Jetpack Visuals/Audio")]
    [SerializeField]
    private ParticleSystem partSys;

    [SerializeField]
    private Transform fuelBar;

    [SerializeField]
    private AudioSource playerAudio;

    [SerializeField]
    private GameObject jetpackPrefab;

    [SerializeField]
    private Vector3 jetPackPos = new Vector3(-0.64f, 0f, 0f);

    [Header("Spring Boots")]
    [SerializeField]
    [Tooltip("Spring boost")]
    private float jumpMultiplier = 2;

    [SerializeField]
    [Tooltip("Number of frames player has, to press jump for a boost after landing")]
    private int frameWindow = 3;

    [SerializeField]
    private int framesPassed = 2000;

    [Header("Background Spring")]
    [SerializeField]
    private float levelBackgroundMoveDistance = 25f;



    private bool hasJumped = false;
    private bool callJump = false;
    private bool isPressingUp = false;
    private bool isInBackground = false;

    //debug
    private Coroutine something;

    private void Awake () {
        characterController = GetComponent<CharacterController> ();
        velocity = Vector3.zero;
    }

    void Update () {
        //Gravity
        if (!characterController.isGrounded) {
            //player.Velocity.y -= gravity * Time.deltaTime;
            SetYVelocity (velocity.y - (gravity * Time.deltaTime));
        }

        //Movement
        //player.velocity.x += acceleration * Time.deltaTime;
        SetXVelocity (velocity.x + acceleration * Time.deltaTime);
        //player.velocity.x = Mathf.Clamp (player.velocity.x, 0, maxSpeed);
        SetXVelocity (Mathf.Clamp (velocity.x, 0, maxSpeed));

        //Score Stuff
        GameManager.Instance.Score += 1 + (int)velocity.x;

        //Spring Jumping
        if (hasJumped == true) {
            if (framesPassed < frameWindow) {
                if (Input.GetMouseButtonDown (0) && characterController.isGrounded) {
                    callJump = true;
                }
                framesPassed++;
            } else if (framesPassed == frameWindow) {
                framesPassed = 0;
                hasJumped = false;
            }
        }

        //Jumping
        if (Input.GetMouseButtonDown (0)) {
            if (characterController.isGrounded) {
                SetYVelocity (0);
                AddForce (new Vector3 (0, jumpPower, 0));
                hasJumped = true;
            }
        }

        if (callJump) {
            velocity += new Vector3 (0, jumpMultiplier, 0);
            SetYVelocity (velocity.y * jumpMultiplier);
            framesPassed = 0;
            hasJumped = false;
            callJump = false;
        }

        if (Input.GetKey (KeyCode.UpArrow)) {
            isPressingUp = true;
        } else {
            isPressingUp = false;
        }

        Move (velocity);
    }

    public void Move (Vector3 v) {
        velocity *= Mathf.Clamp01 (1f - drag * Time.deltaTime);
        characterController.Move (v);
    }

    public void AddForce (Vector3 v) {
        velocity += v;
    }

    public void SlowPlayer (float slowPercent) {
        //player.velocity.x *= slowPercent;
        SetXVelocity (velocity.x * slowPercent);

        if (velocity.x < minSpeed) {
            //player.velocity.x = minSpeed;
            SetXVelocity (minSpeed);
        }
    }

    public void SetYVelocity (float value) {
        velocity.y = value;
    }

    public void SetXVelocity (float value) {
        velocity.x = value;
    }

    //Jetpack functionality
    void FixedUpdate () {
        if (isPressingUp && !characterController.isGrounded) {
            if (remainingFuel > 0) {
                DrainPack ();
                if (partSys != null) {
                    if (!partSys.isPlaying) {
                        partSys.Play ();
                        //playerAudio.Play ();
                    }
                }
                BoostUp ();
            } else {
                StartRefill ();
            }
        } else {
            StartRefill ();
        }
    }

    IEnumerator Refilling () {
        while (!isPressingUp || characterController.isGrounded) {
            RefillPack ();
            yield return new WaitForFixedUpdate();
        }
        something = null;
    }

    private void StartRefill () {
        if (partSys != null) {
            if (partSys.isPlaying) {
                partSys.Stop ();
                //playerAudio.Stop ();
            }
        }
        if (something == null) {
            something = StartCoroutine (Refilling ());

        }
    }

    private void BoostUp () {
        velocity += new Vector3 (0f, verticalAcceleration, 0f);
    }

    private void RefillPack () {
        remainingFuel += (remainingFuel + fuelFill) * Time.fixedDeltaTime;
        remainingFuel = Mathf.Clamp (remainingFuel, 0f, 100f);
        fuelBar.transform.localScale = new Vector3 (1, remainingFuel / 100f, 1);
    }

    private void DrainPack () {
        remainingFuel -= (remainingFuel - fuelUsage) * Time.fixedDeltaTime;
        remainingFuel = Mathf.Clamp (remainingFuel, 0f, 100f);
        fuelBar.transform.localScale = new Vector3 (1, remainingFuel / 100f, 1);
    }

    //Background spring
    public void MoveIntoBackground () {
        if (!isInBackground) {
            transform.position = new Vector3 (transform.position.x, transform.position.y + 1.5f, levelBackgroundMoveDistance);
            isInBackground = true;
        } else {
            transform.position = new Vector3 (transform.position.x, transform.position.y + 1.5f, 0f);
            isInBackground = false;
        }
    }
}