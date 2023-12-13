using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHand : Player
{
   
    private PlayerControl armControl;
    [SerializeField]
    private ArmState arm;
    private InputAction handAction;

    //private Material handColorNormal;

    //[SerializeField]
    //private Material handColorActive;

    //private MeshRenderer render;

    private PlayerController player;

    public Audio GameAudio;

    private void Awake()
    {
        armControl = new PlayerControl();
        
    }

    private void OnEnable()
    {
        switch (arm)
        {
            case ArmState.Left:
               
                handAction = armControl.Player.LeftHand;
                break;
            case ArmState.Right:
               
                handAction = armControl.Player.RightHand;
                break;
        }

        
        handAction.Enable();
    }

    private void OnDisable()
    {
        
        handAction.Disable();
    }

    void Start()
    {
        //handColorNormal = GetComponent<Renderer>().material;
        //render = GetComponent<MeshRenderer>();

        if(player == null)
        {
            player = GameObject.Find("BasePlayer").GetComponent<PlayerController>();
        }
        //handColorBase = handColorNormal;
        
        
    }

    
    void FixedUpdate()
    {
        //HandActivation();
    }

    /*
    private void HandActivation()
    {
        if(handAction.IsPressed())
        {
            render.material = handColorActive;
        }
        else
        {
            render.material = handColorNormal;
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item") && handAction.IsPressed())
        {
            AddToInventory(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Item") && handAction.IsPressed())
        {
            AddToInventory(other);
        }
    }

    private void AddToInventory(Collider other)
    {
        GameObject stolenItem = other.GameObject();
        
        player.StolenItems.Add(stolenItem.GetComponent<ExpensiveObject>());

        stolenItem.SetActive(false);

        if (!GameAudio.audioName.isPlaying)
        {
            GameAudio.PlayStart();
        }
           
    }
}
