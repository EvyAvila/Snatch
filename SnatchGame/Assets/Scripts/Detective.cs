using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Detective : Entity
{
    private bool FollowPlayer;
    private bool LostPlayer;
    private bool DoubleIncreasePoints;
 
    private GameObject Player;

    [SerializeField]
    private float StalkingSpeed;

    private PlayerUI playerUI;

    [SerializeField]
    private GameObject Item;

    public float timeRemaining { get; set; }

    [SerializeField]
    private Material npcColorDetection;
    private Material npcColorNormal;
    private Renderer rend;

    //public Vector3 detectivePosition { get; private set; }

    public Transform detectivePosition;

    // Start is called before the first frame update
    void Start()
    {
        FollowPlayer = false;
        LostPlayer = false; 
        Player = GameObject.Find("BasePlayer");
        DoubleIncreasePoints = false;

        timeRemaining = 60;

        if (playerUI == null)
        {
            playerUI = Player.GetComponent<PlayerUI>();
        }

        Speed = StalkingSpeed;

        Item = transform.Find("Object").gameObject; //Based from https://stackoverflow.com/questions/25763587/how-can-i-find-child-gameobject

        npcColorNormal = GetComponent<Renderer>().material;
        rend = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!Item.activeInHierarchy)
        {
            DoubleIncreasePoints = true;
            StartCoroutine(Timer());
            if (FollowPlayer && !LostPlayer)
            {
                this.transform.LookAt(Player.transform);

                this.transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Speed * Time.deltaTime);

                FollowPlayerTimer();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            DetectionActive(1);
        }

        
    }

    private void DetectionActive(float num)
    {
        rend.material = npcColorDetection;
        StartCoroutine(DetectionIncrease(num));
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DetectionActive(1f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rend.material = npcColorNormal;
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(2);
        if (DoubleIncreasePoints && !FollowPlayer)
        {
            playerUI.DetectionAmount += 2;
        }
        FollowPlayer = true;

        StopAllCoroutines();
    }

    IEnumerator DetectionIncrease(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        playerUI.DetectionAmount++;

        StopAllCoroutines();
    }

    private void FollowPlayerTimer()
    {
        //Timer help from https://gamedevbeginner.com/how-to-make-countdown-timer-in-unity-minutes-seconds/
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            LostPlayer = true;
        }
    }

   
}
