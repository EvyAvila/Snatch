using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

enum DirectionState { North, South, East, West, Idle, Fall }

public class Civilian : Entity
{
    [SerializeField]
    private DirectionState directionState;

    private DirectionState originalState;

    [SerializeField]
    private int timerChangeDirection;

    [SerializeField]
    private float walkingSpeed;

    private int rotationNumber;

    private PlayerUI playerUI;

    [SerializeField]
    private Material npcColorDetection;
    private Material npcColorNormal;
    private Renderer rend;

    private float timeRemaining;
    private float timeDefault;

    [SerializeField]
    private bool Fall;

    private Rigidbody rig;

    void Start()
    {
        timeRemaining = 5;
        timeDefault = timeRemaining;

        Speed = walkingSpeed;

        Fall = false;
        originalState = directionState;

        SetDirection();

        if (playerUI == null)
        {
            playerUI = GameObject.Find("BasePlayer").GetComponent<PlayerUI>();
        }

        npcColorNormal = GetComponent<Renderer>().material;
        rend = GetComponent<MeshRenderer>();

        Physics.gravity = new Vector3(0, -70, 0); //From https://forum.unity.com/threads/global-gravity-setting.610/

        rig = GetComponent<Rigidbody>();
        rig.freezeRotation = true;
    }

    void FixedUpdate()
    {
        MoveAround();

        if(directionState == DirectionState.Fall && Fall) //if state is fall and fall is true
        {
            ResetRotation();
        }
 
        //LookAtPlayer();
    }

    private void MoveAround()
    {
        if (directionState == DirectionState.Idle)
        {
            ForceStopMovement();
        }

        transform.Translate(Direction * Speed * Time.deltaTime);

        StartCoroutine(ChangeDirection());
    }

    private void SetDirection()
    {
        Direction = Vector3.forward;
        switch (directionState)
        {
            case DirectionState.North:
                rotationNumber = 0;
                break;
            case DirectionState.South:
                rotationNumber = -180;
                break;
            case DirectionState.East:
                rotationNumber = 90;
                break;
            case DirectionState.West:
                rotationNumber = 270;
                break;
            case DirectionState.Idle:
                Direction.z = 0;
                Direction.x = 0;
                rotationNumber = 0;
                break;
        }
        transform.Rotate(0, rotationNumber, 0);

    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(timerChangeDirection); //waiting for seconds

        int StopChance = Random.Range(0, 2);


        switch (directionState) //Change the direction
        {
            case DirectionState.North:
            case DirectionState.South:
            case DirectionState.West:
            case DirectionState.East:
                if (StopChance == 1) //NPC is stopping
                {
                    int stop = Random.Range(1, 6);
                    StartCoroutine(TempStop(stop));
                }
                else //NPC is moving
                {
                    Speed = walkingSpeed;
                    transform.Rotate(0, 180, 0);
                }
                break;
        }

        StopAllCoroutines();
    }

    IEnumerator TempStop(int num)
    {
        Speed = 0;
        yield return new WaitForSeconds(num);
        StopAllCoroutines();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DetectionActive(1);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DetectionActive(2.5f);
        }
    }

    private void DetectionActive(float num)
    {
        rend.material = npcColorDetection;
        FallAction();

        StartCoroutine(DetectionIncrease(num));
    }

    //Should this also be included in detective script? 
    private void FallAction()
    {
        Fall = true;
        directionState = DirectionState.Fall;

        rig.freezeRotation = false; //Based from https://discussions.unity.com/t/how-do-i-unfreeze-z-rotation-in-a-script-and-then-freeze-it-again/194326

        this.transform.Rotate(90 * 0.5f * Time.deltaTime, 0, 0);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rend.material = npcColorNormal;
        }
    }

    IEnumerator DetectionIncrease(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        playerUI.DetectionAmount++;
        StopAllCoroutines();
    }

    private void ForceStopMovement()
    {
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Direction.x = 0;
        Direction.z = 0;
    }

    private void ResetRotation()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            this.transform.rotation = new Quaternion(0,0,0,0);
            Fall = false;
            timeRemaining = timeDefault;
            directionState = originalState;
            rig.freezeRotation = true;
        }
    }
    /*
    private void LookAtPlayer()
    {
        var temp = this.directionState;
        var tempRot = this.transform.rotation;

        if(playerUI.PlayerAttention) //If penalty active => player has attention
        {
            Speed = 0;
            directionState = DirectionState.Idle;
            this.transform.LookAt(playerUI.GetComponent<PlayerController>().gameObject.transform);
        }
        else if(!playerUI.PlayerAttention && transform.rotation == tempRot)
        {
            Speed = walkingSpeed;
            this.directionState = temp;
            transform.Rotate(0, rotationNumber, 0);

        }
    }*/

}
