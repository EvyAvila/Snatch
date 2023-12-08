using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AnimalInteraction : MonoBehaviour
{
    private PlayerController player;

    private Audio GameAudio;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = GameObject.Find("BasePlayer").GetComponent<PlayerController>();
      
        }

        if(GameAudio == null)
        {
            GameAudio = GameObject.Find("AnimalInteraction").GetComponent<Audio>();
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
            GameAudio.PlayAudio();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameAudio.StopAudio();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        if(player.StolenItems.Count > 0)
        {
            player.StolenItems.RemoveAt(0);
        }
        else
        {
            player.Speed = player.Speed / 2;
        }

        GameAudio.StopAudio();
        this.gameObject.SetActive(false);
    }

    public void DisplayVisable()
    {
        this.gameObject.SetActive(true);
    }

    
}
