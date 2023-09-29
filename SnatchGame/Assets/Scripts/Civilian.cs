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

    //[SerializeField]
    private int rotationNumber;

    //private int RandomDirection;

    //private float DirectionTime;
    //private float DirectionRate = 5000f; //5 seconds

    void Start()
    {
        Speed = walkingSpeed;
        SetDirection();
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
                break;
        }
        transform.Rotate(0, rotationNumber, 0);
        /*
        RandomDirection = Random.Range(1, 5);
        switch(RandomDirection)
        {
            case 1:
                Direction.z = 1;
                break;
            case 2:
                Direction.z = -1;
                break;
            case 3:
                Direction.x = 1;
                break;
            case 4:
                Direction.x = -1;
                break;
        }*/
    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(timerChangeDirection);

        transform.Rotate(0, 180, 0);

        StopAllCoroutines();
    }
}
