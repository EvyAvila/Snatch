using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AnimalInteraction : MonoBehaviour
{
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = GameObject.Find("BasePlayer").GetComponent<PlayerController>();
      
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("*Animal sound");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("*Animal stops sound");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Play is touching the animal");
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        if(player.StolenItems.Count > 0)
        {
            player.StolenItems.RemoveAt(0);
            Debug.Log("Player loses a stolen item");
        }
        else
        {
            player.Speed = player.Speed / 2;
            Debug.Log("Player speed was reduced");
            
        }

        this.gameObject.SetActive(false);
    }

    public void DisplayVisable()
    {
        this.gameObject.SetActive(true);
    }

    
}
