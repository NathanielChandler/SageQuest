using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class RigidPlayer : MonoBehaviour
{
    #region BasePlayer
    [Header("Physics")]
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    [Tooltip("For increasing fall speed, use negative values, ex. -500y")]
    private Vector3 gravityAdditive;

    [Header("Movement")]
    [SerializeField]
    [Tooltip("Controls how fast player accelerates")]
    private float acceleration;
    [SerializeField]
    [Tooltip("Sets minimum speed for player, AKA starting speed")]
    private float minSpeed;
    [SerializeField]
    [Tooltip("Player's top speed")]
    public float maxSpeed;
    [SerializeField]
    [Tooltip("What the Player's top speed will revert to after jumping a ramp")]
    private float baseMaxSpeed = 45;
    [SerializeField]
    [Tooltip("Player speed read-out")]
    public float currentSpeed;


    [SerializeField]
    [Tooltip("Player's jump power")]
    private float jumpPower;

    [Header("Hazard Settings")]
    [SerializeField]
    private bool scoreFrozen = false;
    [SerializeField]
    [Tooltip("How long Dial-ups freeze the player's score")]
    public float scoreFreezeTime = 6.0f;
    #endregion

    public float MaxSpeed
    {
        get { return maxSpeed; }
        set { maxSpeed = value; }
    }

    public float CurrentSpeed
    {
        get { return currentSpeed; }
        set { currentSpeed = value; }
    }

    public bool Grounded
    {
        get { return grounded; }
        set { grounded = value; }
    }

    public float BaseMaxSpeed
    {
        get { return baseMaxSpeed; }
        set { baseMaxSpeed = value; }
    }

    #region Powerup Settings
    [Header("Jetpack")]
    //[Header("Fuel Tracking")]
    [SerializeField]
    private bool jetpackUnlocked = false;

    [SerializeField]
    private float remainingFuel = 100f;

    [Range(10f, 100f)]
    [SerializeField]
    [Tooltip("Controls how quickly fuel is drained")]
    private float fuelUsage = 0.5f;

    [Range(10f, 100f)]
    [SerializeField]
    [Tooltip("Controls how quickly fuel is refilled")]
    private float fuelFill = 0.5f;

    [Range(10f, 100f)]
    [SerializeField]
    [Tooltip("Minimum fuel required before boosting again")]
    private float minFuel = 10f;
    [SerializeField]
    private float verticalAcceleration;
    //private bool acceleratingJetpack = false;
    private bool packing = true;
    [SerializeField]
    private bool boosting = false;

    [SerializeField]
    [Tooltip("Exhaust particle system")]
    private ParticleSystem partSys;

    [SerializeField]
    [Tooltip("Graphics for fuel bar")]
    private Transform fuelBar;


    [SerializeField]
    [Tooltip("Graphics for the actual jetpack")]
    private GameObject jetpackPrefab;

    [SerializeField]
    [Tooltip("Position of the jetpack (For particle system emitter location)")]
    private Vector3 jetPackPos = new Vector3(-0.64f, 0f, 0f);

    [Header("Double Jump")]
    [SerializeField]
    private bool springBUnlocked = false;
    [SerializeField]
    private GameObject springBPrefab;
    [SerializeField]
    private bool doubleJumpActive = false;
    [SerializeField]
    private bool doubleJumpUsed = false;
    [SerializeField]
    private bool springJumped = false;
    [SerializeField]
    private float doubleJumpCooldownTime = 3.0f;
    [SerializeField]
    [Tooltip("Double Jump boost")]
    private float doubleJumpMultiplier = 2.5f;

    [SerializeField]
    [Tooltip("Number of frames player has, to press jump for a boost after landing")]
    private int frameWindow = 3;

    [SerializeField]
    [Tooltip("Amount of frames that have passed")]
    private int framesPassed = 2000;

    [SerializeField]
    private bool jackRabbitActive = false;

    [SerializeField]
    private float downwardPush = 3f;



    [Header("Skateboard")]
    [SerializeField]
    private bool skateboardUnlocked = false;
    [SerializeField]
    private float skatingTime = 5f;
    [SerializeField]
    private GameObject skateboardPref;
    [SerializeField]
    private GameObject defaultPlayer;
    [SerializeField]
    private bool skating = false;
    [SerializeField]
    private float skateboardActiveTime = 5.0f;

    public bool isOnObstacle = false;

    public bool Skating
    {
        get { return skating; }
    }
    #endregion

    #region Jump Control
    [Header("GroundControl")]
    [SerializeField]
    private bool grounded = false;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private float rotMultiplier = 1f;

    [SerializeField]
    private float rotClamp = 15f;

    [SerializeField]
    private Vector3 rayDirection;

    [SerializeField]
    private Vector3[] rayStarts;

    private bool boostTime = false;
    private bool isPressingUp = false;
    #endregion

    [SerializeField]
    private bool isInBackground;

    [SerializeField]
    private float levelBackgroundMoveDistance;

    private RigidbodyConstraints currentConstraints;
    private GameObject hitObject;

    //Audio Stuff, for Robert P sound implementation ONLY//
    public AudioClip SpringBoot;
    public AudioClip JumpGrunt;
    public AudioClip SkateboardStart;
    bool slowed;
    public float previousX;

    //Edge Detection
    [SerializeField]
    private bool frameWait = false;
    private bool dialUpActive = false;
    //Animations Stuff (DONT TOUCH ANYONE)
    private bool updateIsJumping = false;
    private bool updateIsSpringJumping = false;
    private bool hasSpringed = false;
    private bool runningAnim = true;
    private bool landed = false;
    private float graphicsHeight;
    public bool freezeDelay = false;
    public GameObject graphics;
    //Score Stuff
    private GameObject uiManager;
    private bool mobile = false;
    private void Awake()
    {
        uiManager = GameObject.Find("_UISystem");
        currentConstraints = rb.constraints;
        graphicsHeight = transform.GetChild(0).transform.GetChild(0).transform.position.y;
        CheckPowerups();
        if (Application.platform == RuntimePlatform.WindowsPlayer)
            mobile = false;
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            mobile = true;
    }

    private void CheckPowerups()
    {
        //Load Loadout Serialized Values
        string jackRabbitKey = "Jack Rabbit_Unlocked";
        if (PlayerPrefs.HasKey(jackRabbitKey))
        {
            int unlocked = PlayerPrefs.GetInt(jackRabbitKey);

            if (unlocked == 1)
            {
                jackRabbitActive = true;
            }
        }

        string doubleJumpKey = "Double Jump_Unlocked";
        if (PlayerPrefs.HasKey(doubleJumpKey))
        {
            int unlocked = PlayerPrefs.GetInt(doubleJumpKey);

            if (unlocked == 1)
            {
                doubleJumpActive = true;
            }
        }

        string teleportKey = "Teleport_Unlocked";
        if (PlayerPrefs.HasKey(teleportKey))
        {
            int unlocked = PlayerPrefs.GetInt(teleportKey);

            if (unlocked == 1)
            {
                //Enable teleport functionality here.
            }
        }

        if (jetpackUnlocked)
        {
            jetpackPrefab.SetActive(true);
            StartCoroutine(JetpackRoutine());
        }

        if (springBUnlocked)
        {
            springBPrefab.SetActive(true);
        }
    }

    #region Constant Functions
    void Update()
    {
        //Anims
        if (updateIsJumping && grounded)
        {
            graphics.GetComponent<Animator>().SetBool("isFalling", false);
            updateIsJumping = false;
        }

        if (updateIsSpringJumping && grounded)
        {
            graphics.GetComponent<Animator>().SetBool("isFalling", false);
            updateIsSpringJumping = false;
        }

        if (runningAnim && !grounded && landed)
        {
            graphics.GetComponent<Animator>().SetBool("isFalling", false);
        }

        //Edge detection check
        if (!frameWait)
        {
            previousX = transform.position.x;
            StartCoroutine(checkX());
            frameWait = true;
        }

        //Debug.Log(rb.velocity);
        //Movement
        DetectGround();

        //Score Stuff
        if (!scoreFrozen && frameWait)
        {
            GameManager.Instance.Score += (int)Mathf.Round(1f + rb.velocity.x / 9);
        }
        if (uiManager.transform.GetChild(1).transform.GetChild(0).GetComponent<PauseButton>().GamePaused)
        {
            scoreFrozen = true;
        }
        else
        {
            scoreFrozen = false;
        }
        //currentSpeed = Mathf.Clamp(currentSpeed + acceleration, minSpeed, maxSpeed);
        //transform.Translate(new Vector3(currentSpeed, 0, 0) * Time.deltaTime);

        if (grounded == true)
        {
            landed = true;
            if (springBUnlocked)
            {
                if (doubleJumpUsed)
                {
                    doubleJumpUsed = false;
                }
            }
            if (mobile && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                Jump();
            }
            else if (!mobile && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Jump();
            }
        }

        if (grounded != true)
        {
            landed = false;
            if (springBUnlocked && !doubleJumpUsed && !skating)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Jump();
                    doubleJumpUsed = true;
                }
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            isPressingUp = true;
        }
        else
        {
            isPressingUp = false;
        }

        if (skateboardUnlocked && Input.GetKeyDown(KeyCode.DownArrow) && !freezeDelay)
        {
            if (!skating)
            {
                skateboardActiveTime = 5.0f;
                skating = true;
                StartCoroutine(skateDelay());
                graphics.GetComponent<Animator>().SetBool("isSkating", true);
                defaultPlayer.SetActive(true);
                skateboardPref.SetActive(true);
                GetComponent<CapsuleCollider>().enabled = false;
                // SoundManager.instance.Play(SkateboardStart, "sfx");
            }
            else
            {
                skating = false;
                graphics.GetComponent<Animator>().SetBool("isSkating", false);
                defaultPlayer.SetActive(false);
                skateboardPref.SetActive(false);
                GetComponent<CapsuleCollider>().enabled = true;
            }
        }
        if (!skating)
        {
            skating = false;
            graphics.GetComponent<Animator>().SetBool("isSkating", false);
            defaultPlayer.SetActive(false);
            skateboardPref.SetActive(false);
            GetComponent<CapsuleCollider>().enabled = true;
        }
    }

    IEnumerator skateDelay()
    {
        yield return new WaitForSeconds(skatingTime);
        skating = false;
    }

    IEnumerator checkX()
    {
        yield return new WaitForSeconds(0.1f);
        if (previousX == transform.position.x)
        {
            scoreFrozen = true;
            currentSpeed = 0f;
            rb.AddForce(Vector3.down * downwardPush, ForceMode.Impulse);
        }
        else
        {
            if (dialUpActive == false)
            {
                scoreFrozen = false;
            }
        }
        frameWait = false;
    }

    void FixedUpdate()
    {
        if (!grounded)
        {
            rb.AddForce(gravityAdditive * Time.fixedDeltaTime, ForceMode.Force);
        }

        currentSpeed = Mathf.Clamp(currentSpeed + (acceleration * Time.fixedDeltaTime), minSpeed, maxSpeed);
        if (runningAnim)
        {
            graphics.GetComponent<Animator>().speed = currentSpeed / 16f;
        }
        else
        {
            graphics.GetComponent<Animator>().speed = 1f;
        }
        rb.velocity = new Vector3(currentSpeed, rb.velocity.y, rb.velocity.z);
    }

    public void SlowPlayer()
    {
        currentSpeed = minSpeed;
        graphics.GetComponent<Animator>().SetBool("isDamage", true);
        StartCoroutine(AnimDelay("isDamage", 0.566f));
    }

    IEnumerator AnimDelay(string animType, float animTime)
    {
        runningAnim = false;
        yield return new WaitForSeconds(animTime);
        if (animType == "isDamage")
        {
            graphics.GetComponent<Animator>().SetBool("isDamage", false);
        }
        if (animType == "isJumping")
        {
            graphics.GetComponent<Animator>().speed = 1f;
            graphics.GetComponent<Animator>().SetBool("isJumping", false);
            if (!hasSpringed)
            {
                graphics.GetComponent<Animator>().SetBool("isFalling", true);
            }
            else
            {
                hasSpringed = false;
            }
            updateIsJumping = true;
        }
        if (animType == "isSpringjump")
        {
            graphics.GetComponent<Animator>().SetBool("isSpringjump", false);
            graphics.GetComponent<Animator>().SetBool("isFalling", true);
            runningAnim = true;
            updateIsSpringJumping = true;
        }
    }

    public void Jump()
    {
        Debug.Log("jump call");
        graphics.GetComponent<Animator>().SetBool("isJumping", true);
        StartCoroutine(AnimDelay("isJumping", 0.87f));
        //Debug.Log(framesPassed);
        //SetYVelocity(0);
        //AddForce(new Vector3(0, jumpPower * Time.fixedDeltaTime, 0));
        Vector3 jumpForce = Vector3.up * jumpPower;
        SoundManager.instance.Play(JumpGrunt, "sfx");
        if (springBUnlocked && grounded != true)
        {
            jumpForce *= doubleJumpMultiplier;
            SoundManager.instance.Play(SpringBoot, "sfx");
            graphics.GetComponent<Animator>().SetBool("isJumping", false);
            graphics.GetComponent<Animator>().SetBool("isSpringjump", true);
            hasSpringed = true;
            StartCoroutine(AnimDelay("isSpringjump", 1f));
        }
        rb.AddForce(jumpForce);
    }

    private void DetectGround()
    {
        RaycastHit hit;
        for (int i = 0; i < 3; i++)
        {
            //Debug.DrawRay(transform.position + rayStarts[i], rayDirection, Color.green, -rayDirection.y);
            if (Physics.Raycast(transform.position + rayStarts[i], rayDirection, out hit, -rayDirection.y, groundLayer))
            {
                hitObject = hit.transform.gameObject;
                if (hitObject.tag == "Obstacle")
                {
                    if (i == 0)
                    {
                        transform.up = hit.normal;
                    }
                    return;
                }
                grounded = true;
                break;
            }
            else
            {
                grounded = false;
            }
        }

    }

    public void StartRotateBack()

    {
        StartCoroutine("RotateBack");
    }



    IEnumerator RotateBack()
    {
        while (!grounded)
        {
            float rotation = rb.velocity.y * rotMultiplier;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Clamp(rotation, -rotClamp, rotClamp)));
            yield return null;
        }
        transform.rotation = Quaternion.identity;
    }


    #endregion

    #region Jetpack Functionality
    IEnumerator JetpackRoutine()
    {
        while (jetpackUnlocked)
        {
            if (!boosting && isPressingUp && !grounded && remainingFuel >= 10f)
            {
                boosting = true;
                StartCoroutine(Boosting());
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Boosting()
    {
        if (partSys != null)
        {
            if (!partSys.isPlaying)
            {
                //partSys.Play();
                //playerAudio.Play ();
            }
        }
        while (remainingFuel > 0 && !grounded)
        {
            if (isPressingUp)
            {
                BoostUp();
                DrainPack();
            }
            else
            {
                SilencePack();
            }
            yield return new WaitForFixedUpdate();
        }
        StartRefill();
    }

    private void SilencePack()
    {

    }

    IEnumerator Refilling()
    {
        while (!grounded)
        {
            yield return null;
        }
        boosting = false;
        while (!boosting)
        {
            RefillPack();
            yield return new WaitForFixedUpdate();
        }
    }

    private void StartRefill()
    {
        SilencePack();
        StartCoroutine(Refilling());
    }

    private void BoostUp()
    {
        rb.AddForce(new Vector3(0f, verticalAcceleration, 0f));
    }

    private void RefillPack()
    {
        remainingFuel += fuelFill * Time.fixedDeltaTime;
        remainingFuel = Mathf.Clamp(remainingFuel, 0f, 100f);
        if (remainingFuel >= 10 && boosting)
        {
            boosting = false;
        }
        fuelBar.transform.localScale = new Vector3(1, remainingFuel / 100f, 1);
    }

    private void DrainPack()
    {
        remainingFuel -= fuelUsage * Time.fixedDeltaTime;
        remainingFuel = Mathf.Clamp(remainingFuel, 0f, 100f);
        fuelBar.transform.localScale = new Vector3(1, remainingFuel / 100f, 1);
    }
    #endregion

    //Background spring
    public void MoveIntoBackground()
    {
        Debug.Log("buffalo");
        if (!isInBackground)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, levelBackgroundMoveDistance);
            isInBackground = true;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            isInBackground = false;
        }
    }

    public void FreezeScore()
    {
        StartCoroutine("DialUp");
    }

    IEnumerator DialUp()
    {
        dialUpActive = true;
        scoreFrozen = true;
        yield return new WaitForSeconds(scoreFreezeTime);
        scoreFrozen = false;
        dialUpActive = false;
    }

    public void ConstrainPlayer()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnConstrainPlayer()
    {
        rb.constraints = currentConstraints;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    GameObject colObj = collision.gameObject;
    //    if (!colObj.CompareTag("Ground"))
    //    {
    //        currentSpeed = minSpeed;
    //    }
    //}
}