using System.Collections;
using System.Collections.Generic;
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
        yield return new WaitForSeconds(timerChangeDirection);


        if(directionState != DirectionState.Idle)
        {
            transform.Rotate(0, 180, 0);
        }

        

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
}
