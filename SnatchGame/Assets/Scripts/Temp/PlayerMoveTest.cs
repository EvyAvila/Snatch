using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveTest : Player
{
    private PlayerControl controls;
    private InputAction moveAction;

    private Animator animator;

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


    // Start is called before the first frame update
    void Start()
    {
        Speed = 5;
        RotateSpeed = 50;

        if(animator == null)
        {
            animator= GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        
        
        var value = moveAction.ReadValue<Vector3>();
        Direction.z = -value.y; //I don't know why it won't take z
        
        /*
        if(Direction.z == 1)
        {
            animator.SetBool("Fall", true);
        }
        else
        {
            animator.SetBool("Fall", false);
        }*/

        transform.Rotate(0, -value.x * RotateSpeed * Time.deltaTime, 0);

        if (!moveAction.IsPressed() )//&& TouchGround)
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        animator.SetFloat("Speed", Direction.z);
        transform.Translate(Direction * Speed * Time.deltaTime);


        //MoveBackDetection();

        Direction.Normalize();
    }
}
