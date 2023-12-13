using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArmMoveTest : Player
{
    private PlayerControl armControl;

    private InputAction armAction;


    [SerializeField]
    private ArmState arm;

   

    [SerializeField]
    private int rs;

   

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
       
    }

    void FixedUpdate()
    {
        MoveArm();

    }


    private void MoveArm()
    {
        var value = armAction.ReadValue<Vector3>();

        //Debug.Log(value);
        switch (arm)
        {
            case ArmState.Left:
                Direction.z = value.y; //forward and back
                Direction.x = value.x; //left and right
                break;
            case ArmState.Right:

                Direction.z = -value.y; //forward and back
                Direction.x = -value.x; //left and right
                break;
        }

       

        if (Direction.x != 0 || Direction.z != 0)
        {
            transform.Rotate(Direction * RotateSpeed * Time.deltaTime);
            //transform.eulerAngles += -Direction * RotateSpeed * Time.deltaTime;
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

    }
}
