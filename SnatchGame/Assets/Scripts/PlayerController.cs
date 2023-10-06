using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Player
{
    private PlayerControl controls;

    private InputAction moveAction;


    public List<ExpensiveObject> StolenItems;
    public int StolenItemsTotal;

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
        StolenItems = new List<ExpensiveObject>();
        
        FindTotal();
        
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
        transform.Translate(Direction * Speed * Time.deltaTime);
    }

    private void FindTotal()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Item");

        StolenItemsTotal = obj.Length;
    }
}
