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

    private Material handColorNormal;

    [SerializeField]
    private Material handColorActive;

    private MeshRenderer render;

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
        handColorNormal = GetComponent<Renderer>().material;
        render = GetComponent<MeshRenderer>();
        //handColorBase = handColorNormal;
    }

    
    void FixedUpdate()
    {
        HandActivation();
    }

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
    }

    //Do I still need this part, or keep stay?
    private void OnTriggerEnter(Collider other)
    {
        //var value = handAction.ReadValue<float>();

        if (other.gameObject.CompareTag("Item") && handAction.IsPressed())
        {
            Debug.Log(arm.ToString() + " hand is grabbing the item");

            GameObject stolenItem = other.GameObject();
            stolenItem.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Item") && handAction.IsPressed())
        {
            Debug.Log(arm.ToString() + " hand is grabbing the item");

            GameObject stolenItem = other.GameObject();
            stolenItem.SetActive(false);
        }
    }




}
