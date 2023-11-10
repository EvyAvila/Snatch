using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TurnState { Left, Right }

public class VehicleMovement : MonoBehaviour
{
    [SerializeField]
    private TurnState turnState;

    [SerializeField]
    private int direction;

    private Vector3 Direction;

    [SerializeField]
    private float Speed;
    int turnDegree;

    private PlayerUI playerUI;
    //private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        Direction.y = direction;
        
        SetTurn();

        if (playerUI == null)
        {
            playerUI = GameObject.Find("BasePlayer").GetComponent<PlayerUI>();
        }
        /*
        if(player == null)
        {
            player = GameObject.Find("BasePlayer").GetComponent<PlayerController>();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Direction * Speed * Time.deltaTime);
    }

    private void SetTurn()
    {
        switch (turnState)
        {
            case TurnState.Left:
                //Direction = Vector3.down;
                turnDegree = -90;
                break;
            case TurnState.Right:
                //Direction = Vector3.up;
                turnDegree = 90;
                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Corner"))
        {
            transform.Rotate(0, 0, turnDegree);
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DetectionIncrease());
        }
    }

    IEnumerator DetectionIncrease()
    {
        yield return new WaitForSeconds(1);
        playerUI.DetectionAmount += 4;
              
        StopAllCoroutines();
    }
}
