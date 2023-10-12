using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerUI playerStatus;
    public BlackMarketUI bmStatus;
    
    public Transform ChangePosition;
    
    private PlayerController Player;
    private bool isTeleporting = false;
    
    void Start()
    {
        Player = GameObject.Find("BasePlayer").GetComponent<PlayerController>();
        ChangePosition.localRotation = Player.transform.localRotation;
        bmStatus.enabled = false;
        playerStatus.enabled = true;    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isTeleporting)
        {
            switch (playerStatus.playerState)
            {
                case PlayerState.Lose:
                    Time.timeScale = 0;
                    break;
                case PlayerState.Active:
                    Time.timeScale = 1;
                    playerStatus.enabled = true;
                    bmStatus.enabled = false;
                    break;
                case PlayerState.Win:
                    StartCoroutine(TeleportPlayer());
                    //Move the position of the player into the black market area
                    break;
                case PlayerState.Market:
                    bmStatus.enabled = true;
                    break;
            }
        }

       
        //Debug.Log(playerStatus.playerState);
    }

    //Alternated with help from Chat.GPT
    private IEnumerator TeleportPlayer()
    {
        isTeleporting = true;

        yield return new WaitForSeconds(2f);

        Player.transform.localPosition = ChangePosition.localPosition;
        
        isTeleporting = false;
        
        playerStatus.playerState = PlayerState.Market;

        playerStatus.enabled = false;
        
    }

  
 


}
