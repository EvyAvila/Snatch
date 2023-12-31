using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Player
{
    private PlayerControl controls;
    private InputAction moveAction;
    public InputAction selectAction { get; private set; }

    public List<ExpensiveObject> StolenItems;
    public int StolenItemsTotal { get; set; }
    public int GetTotal { get; set; }

    public Animator animator;

    [SerializeField]
    private int speed;
    [SerializeField]
    private int rotateSpeed;

    bool TouchGround;
    private bool Fall;

    private Rigidbody rig;

    private float fallRemaining;
    private float fallDefault;

    public float timeRemaining { get; set; }
    private float timeDefault;

    public float CooldownTimer { get; set; }
    private float CooldownDefault;

    public float DetectionMeterCount;

    public bool PenaltyActive { get; private set; }

    private PlayerUI playerUI;

    private float reductionSpeedTime;
    private float reductionSpeedDefault;

    private void Awake()
    {
        controls = new PlayerControl(); 
    }

    private void OnEnable()
    {
        moveAction = controls.Player.Move;
        moveAction.Enable();

        selectAction = controls.Player.Select;
        selectAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        selectAction.Disable();
    }

    void Start()
    {
        Speed = speed;
        RotateSpeed = rotateSpeed;
        StolenItems = new List<ExpensiveObject>();
        timeRemaining = 5;
        timeDefault = timeRemaining;

        CooldownTimer = 10;
        CooldownDefault = CooldownTimer;
        FindTotal();
        PenaltyActive= false;

        
        TouchGround = false;

        //Physics.gravity = new Vector3(0, -7, 0);
        rig = GetComponent<Rigidbody>();
        rig.freezeRotation = true;
        Fall = false;

        fallRemaining = 3;
        fallDefault = fallRemaining;

        playerUI = GetComponent<PlayerUI>();

        reductionSpeedTime = 5;
        reductionSpeedDefault = reductionSpeedTime;

        animator = GetComponent<Animator>();
        DetectionMeterCount = 0;
    }

    void FixedUpdate()
    {
        /*
        if(!TouchGround)
        {
           
            //this.transform.position += Vector3.down * 2 * Time.deltaTime;
        }*/

        if (Fall)
        {
            ResetRotation();
        }
        else if(playerUI.playerState == PlayerState.Active && !Fall)
        {
            MovePlayer();
        }

        if(Speed != speed)
        {
            ResetSpeed();
        }

        if(playerUI.playerState == PlayerState.Win)
        {
            animator.SetFloat("Speed", 0);
        }

    }

    private void MovePlayer()
    {

        var value = moveAction.ReadValue<Vector3>();
        Direction.z = -value.y; 

        transform.Rotate(0, -value.x * RotateSpeed * Time.deltaTime, 0);

        if(!moveAction.IsPressed() && TouchGround)
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero; 
        }

        animator.SetFloat("Speed", Direction.z);
        
        transform.Translate(Direction * Speed * Time.deltaTime);

        MoveBackDetection();

        Direction.Normalize();
    }

    private void FindTotal()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Item");
        GetTotal = obj.Length;

        //StolenItemsTotal = obj.Length;
        StolenItemsTotal = (int)Mathf.Round(obj.Length / 2);
    }

    public void MoveBackDetection()
    {
        //Z = -1 for moving backwards
        switch (PenaltyActive)
        {
            case true:
                //Debug.Log("Punished");
                if(CooldownTimer > 0 && Direction.z != -1)
                {
                   // Debug.Log("cooldown in progress");
                    CooldownTimer -= Time.deltaTime;
                    DetectionMeterCount -= Time.deltaTime;
                }
                else if(CooldownTimer > 0 && Direction.z < 0 || Direction.z == -1)
                {
                    //Debug.Log("Reset cooldown");
                    CooldownTimer = CooldownDefault;
                    DetectionMeterCount = CooldownDefault;
                }
                else if(CooldownTimer <= 0)
                {
                    //Debug.Log("cooldown over");
                    PenaltyActive = false;
                    CooldownTimer = CooldownDefault;
                    timeRemaining = timeDefault;
                    DetectionMeterCount = 0;
                }
                break;
            case false:
                if (Direction.z < 0 && timeRemaining > 0) //Count down timer if using backwards
                {
                    timeRemaining -= Time.deltaTime;
                    DetectionMeterCount += Time.deltaTime;
                    //Debug.Log("Moving back detecting...");
                }
                else if (Direction.z == 0 && timeRemaining > 0) //If player stops before timer, reset
                {
                    if(timeRemaining <= timeDefault)
                    {
                        timeRemaining += Time.deltaTime;
                        DetectionMeterCount -= Time.deltaTime;
                    }
                    //timeRemaining = timeDefault;
                    //DetectionMeterCount = 0;
                    //Debug.Log("Stopped moving back");
                }
                else if (timeRemaining <= 0) //If player goes too long
                {
                    //Debug.Log("Moving back too long");
                    PenaltyActive = true;
                }
                break;
        }


        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Vehicle"))
        {
            FallAction();
        }
       

        if(collision.gameObject.CompareTag("Ground"))
        {
            TouchGround= true;
        }
        else
        {
            TouchGround= false;
        }
    }


    private void FallAction()
    {
        Fall = true;
        animator.SetBool("Fall", Fall);
        rig.freezeRotation = false; //Based from https://discussions.unity.com/t/how-do-i-unfreeze-z-rotation-in-a-script-and-then-freeze-it-again/194326
        this.transform.position += Vector3.up * 15 * Time.deltaTime;
        this.transform.Rotate(0,0, -180 * 4 * Time.deltaTime);
    }

    private void ResetRotation()
    {
        if (fallRemaining > 0)
        {
            fallRemaining -= Time.deltaTime;
        }
        else
        {
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
            Fall = false;
            animator.SetBool("Fall", Fall);
            fallRemaining = fallDefault;
           
            rig.freezeRotation = true;
        }
    }

   

    public void ResetSpeed()
    {
        if (reductionSpeedTime > 0)
        {
            //Debug.Log("Speed Cooldown");
            reductionSpeedTime -= Time.deltaTime;
        }
        else
        {
            Speed = speed;
            reductionSpeedTime = reductionSpeedDefault;
            //Debug.Log("Player speed back to normal");
        }
    }

}
