using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

enum ArmState { Left, Right}

public class PlayerArm : Player
{
    private PlayerControl armControl;
    
    private InputAction armAction;
   

    [SerializeField]
    private ArmState arm;

    private GameObject PlayerBase;

    [SerializeField]
    private int rs;

    private PlayerUI playerUI;
    
    private void Awake()
    {
        armControl = new PlayerControl();
    }

    private void OnEnable()
    {
        switch (arm)
        {
            case ArmState.Left:
                armAction = armControl.Player.LeftArm;
               
                break;
            case ArmState.Right:
                armAction = armControl.Player.RightArm;
                
                break;
        }
        
        armAction.Enable();
       
    }

    private void OnDisable()
    {
        armAction.Disable();
        
    }

    void Start()
    {
        RotateSpeed = rs;
        PlayerBase = GameObject.Find("BasePlayer");

        if(playerUI == null)
        {
            playerUI = PlayerBase.GetComponent<PlayerUI>();
        }
       
    }
    
    void FixedUpdate()
    {
        if(playerUI.playerState == PlayerState.Active)
        {
            MoveArm();
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, PlayerBase.transform.rotation.y, 0);
        }
        
    }


    private void MoveArm()
    {
        var value = armAction.ReadValue<Vector3>();

        //Debug.Log(value);
        Direction.z = -value.y; //forward and back
        Direction.x = value.x; //left and right

        if (Direction.x != 0 || Direction.z != 0)
        {
            transform.Rotate(Direction * RotateSpeed * Time.deltaTime);
            //transform.eulerAngles += -Direction * RotateSpeed * Time.deltaTime;
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, PlayerBase.transform.rotation.y, 0);
        }        
        
    }   
}
