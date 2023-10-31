using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Analytics;

enum DirectionState { North, South, East, West, Idle}

public class Civilian : Entity
{
    [SerializeField]
    private DirectionState directionState;

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

    void Start()
    {
        Speed = walkingSpeed;
        
        SetDirection();

        if(playerUI == null)
        {
            playerUI = GameObject.Find("BasePlayer").GetComponent<PlayerUI>();
        }

        npcColorNormal = GetComponent<Renderer>().material;
        rend = GetComponent<MeshRenderer>();
    }

    void FixedUpdate()
    {
        MoveAround();
        //LookAtPlayer();
    }

    private void MoveAround()
    {      
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

        /*
        if (directionState != DirectionState.Idle && StopChance == 0) //The NPC is not stopping
        {
            
        }*/



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
        StartCoroutine(DetectionIncrease(num));
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
