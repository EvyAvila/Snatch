using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;

enum ArmState { Left, Right}

public class PlayerArm : Player
{
    private PlayerControl armControl;
    
    private InputAction armAction;
    //private InputAction handAction;

    [SerializeField]
    private ArmState arm;

    private GameObject PlayerBase;

    //[SerializeField]
    //private GameObject PlayerHand;
    
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
                //handAction = armControl.Player.LeftHand;
                break;
            case ArmState.Right:
                armAction = armControl.Player.RightArm;
                //handAction= armControl.Player.RightHand;
                break;
        }
        
        armAction.Enable();
        //handAction.Enable();
    }

    private void OnDisable()
    {
        armAction.Disable();
        //handAction.Disable();
    }

    void Start()
    {
        Speed = 2;
        RotateSpeed = 50;
        PlayerBase = GameObject.Find("Base");
       
    }
    
    void FixedUpdate()
    {
        MoveArm();
        
    }

    private void MoveArm()
    {
        var value = armAction.ReadValue<Vector3>();
        Direction.z = -value.x; //left and right
        Direction.x = -value.y; //forward and backward
       
        if(Direction.x != 0 || Direction.z != 0)
        {
            transform.Rotate(-Direction * RotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, PlayerBase.transform.rotation.y, 0);
        }

        
    }

    

    



}
