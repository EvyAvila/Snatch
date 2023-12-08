using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TurnState { Left, Right }

public class VehicleMovement : MonoBehaviour, IDetectionCount
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
    
    public Audio[] GameAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        GameAudio = new Audio[2];

        GameAudio[0] = GameObject.Find("Detection").GetComponent<Audio>();
        GameAudio[1] = GameObject.Find("RanOverbyVehicle").GetComponent<Audio>();

        Direction.y = direction;
        
        SetTurn();

        if (playerUI == null)
        {
            playerUI = GameObject.Find("BasePlayer").GetComponent<PlayerUI>();
        }
       
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
                turnDegree = -90;
                break;
            case TurnState.Right:
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
            GameAudio[1].PlayStart();
            StartCoroutine(DetectionIncrease(1));
        }
    }

    public IEnumerator DetectionIncrease(float waitForTimer)
    {
        yield return new WaitForSeconds(waitForTimer);
        if (!GameAudio[0].audioName.isPlaying)
        {
            GameAudio[0].PlayStart();
        }
        playerUI.DetectionAmount += 4;
              
        StopAllCoroutines();
    }
}
