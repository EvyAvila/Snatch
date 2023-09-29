using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Player
{
    private PlayerControl controls;

    private InputAction moveAction;

    //private float rotateSpeed = 10;

    private void Awake()
    {
        controls = new PlayerControl(); 
    }

    private void OnEnable()
    {
        moveAction = controls.Player.Move;
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    void Start()
    {
        Speed = 4;
        RotateSpeed = 40;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {

        var value = moveAction.ReadValue<Vector3>();
        Direction.z = value.y; //I don't know why it won't take z

        /*
        if(Direction.z > 0 || Direction.z < 0 || value.x > 0 || value.x < 0) 
        {
            playerState = PlayerState.Move;
        }
        else
        {
            playerState = PlayerState.Idle;
        }*/

        transform.Rotate(0, -value.x * RotateSpeed * Time.deltaTime, 0);
        transform.Translate(Direction * Speed * Time.deltaTime);
    }
}
