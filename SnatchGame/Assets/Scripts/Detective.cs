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

    private float timeRemaining;

    [SerializeField]
    private Material npcColorDetection;
    private Material npcColorNormal;
    private Renderer rend;

    public Transform detectivePosition;

    private float timeRemain;
    private float timeDefault;

    [SerializeField]
    private bool Fall;

    private Rigidbody rig;

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

        timeRemain = 5;
        timeDefault = timeRemain;
        Fall = false;
        Physics.gravity = new Vector3(0, -70, 0); //From https://forum.unity.com/threads/global-gravity-setting.610/

        rig = GetComponent<Rigidbody>();
        rig.freezeRotation = true;
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

        if(Fall)
        {
            ResetPosition();
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
        FallAction();
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

    private void FallAction()
    {
        Fall = true;

        rig.freezeRotation = false; //Based from https://discussions.unity.com/t/how-do-i-unfreeze-z-rotation-in-a-script-and-then-freeze-it-again/194326

        this.transform.Rotate(90 * 0.5f * Time.deltaTime, 0, 0);
    }


    private void ResetPosition()
    {
        if (timeRemain > 0)
        {
            timeRemain -= Time.deltaTime;
        }
        else
        {
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
            Fall = false;
            timeRemain = timeDefault;
            rig.freezeRotation = true;
        }
    }
   
}
