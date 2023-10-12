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
   

    [SerializeField]
    private ArmState arm;

    private GameObject PlayerBase;

    [SerializeField]
    private int rotateSpeed;
    
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
        RotateSpeed = rotateSpeed;
        PlayerBase = GameObject.Find("BasePlayer");
       
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
