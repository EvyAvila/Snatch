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

    [SerializeField]
    private int speed;
    [SerializeField]
    private int rotateSpeed;

    public float timeRemaining { get; set; }
    private float timeDefault;

    public float CooldownTimer { get; set; }
    private float CooldownDefault;

    public float DetectionMeterCount;

    public bool PenaltyActive { get; private set; }

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

        DetectionMeterCount = 0;
        
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {

        var value = moveAction.ReadValue<Vector3>();
        Direction.z = value.y; //I don't know why it won't take z

        transform.Rotate(0, -value.x * RotateSpeed * Time.deltaTime, 0);

        if(!moveAction.IsPressed())
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero; 
        }

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
        //Z = 1 for moving backwards
        switch (PenaltyActive)
        {
            case true:
                //Debug.Log("Punished");
                if(CooldownTimer > 0 && Direction.z != 1)
                {
                   // Debug.Log("cooldown in progress");
                    CooldownTimer -= Time.deltaTime;
                    DetectionMeterCount -= Time.deltaTime;
                }
                else if(CooldownTimer > 0 && Direction.z > 0 || Direction.z == 1)
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
                if (Direction.z > 0 && timeRemaining > 0) //Count down timer if using backwards
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
}
