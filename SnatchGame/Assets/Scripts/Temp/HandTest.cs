using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandTest : MonoBehaviour
{
    private PlayerControl armControl;
    [SerializeField]
    private ArmState arm;
    private InputAction handAction;

    //private Material handColorNormal;

    //[SerializeField]
    //private Material handColorActive;

    //private MeshRenderer render;



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

       
    }


    void FixedUpdate()
    {
        //HandActivation();
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item") && handAction.IsPressed())
        {
            AddToInventory(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Item") && handAction.IsPressed())
        {
            
            AddToInventory(other);
        }
    }

    //Double check this can work
    private void AddToInventory(Collider other)
    {
        GameObject stolenItem = other.GameObject();

        stolenItem.SetActive(false);

       

    }
}
