using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerUI playerStatus;
    public BlackMarketUI bmStatus;
    
    public Transform ChangePosition;
    
    private PlayerController Player;
    
    private bool isTeleporting = false;

    [SerializeField]
    private GameObject NPCCollection;

    //[SerializeField]
    //private GameObject NPCPositions;
    
    [SerializeField]
    private GameObject Detective;

    [SerializeField]
    private GameObject AnimalCollection;

    AnimalInteraction[] AnimalSet;
    //Rating system



    void Start()
    {
        //NPCCollection.SetActive(true);
        Player = GameObject.Find("BasePlayer").GetComponent<PlayerController>();
        ChangePosition.localRotation = Player.transform.localRotation;
        bmStatus.enabled = false;
        playerStatus.enabled = true;

        AnimalSet = AnimalCollection.GetComponentsInChildren<AnimalInteraction>();
        //AnimalSet = transform.Find(tag).GetComponent<AnimalInteraction>();
    }

    // Update is called once per frame
    void Update()
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
                    SetLevel(false);
                    //Move the position of the player into the black market area
                    break;
                case PlayerState.Market:
                    bmStatus.enabled = true;
                    break;
            }
        }

        if(playerStatus.playerState == PlayerState.Lose)
        {
            ReturnToMenu();
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

    private void SetLevel(bool condition)
    {
        //NPCCollection.GetComponentInChildren<GameObject>().SetActive(condition);
        NPCCollection.SetActive(condition);
        Detective.SetActive(condition);
        //AnimalCollection.SetActive(condition);
        
    }
 
    public void ResetObjects()
    {
        NPCCollection.SetActive(true);
        Detective.SetActive(true);
        
        Civilian[] NPCSet = NPCCollection.GetComponentsInChildren<Civilian>();
        
        for (int i = 0; i < NPCSet.Length; i++)
        {
            
            if (!NPCSet[i].transform.Find("Object").gameObject.activeSelf)
            {
                ResetGame(NPCSet[i].gameObject);
            }
            
        }
        ResetGame(Detective);
        
        Detective.transform.localPosition = Detective.GetComponent<Detective>().detectivePosition.localPosition;

    }

    private void ResetGame(GameObject obj)
    {
        obj.transform.Find("Object").gameObject.SetActive(true);
        obj.transform.Find("Object").GetComponent<ExpensiveObject>().SetObject();
    }

    public void ResetAnimals()
    {
        foreach(var v in AnimalSet)
        {
            v.DisplayVisable();
           
        }
    }

    private void ReturnToMenu()
    {
        if(Player.selectAction.IsPressed())
        {
            SceneManager.LoadScene(1);
            //Debug.Log("Reset works");
        }  
        
    }
}
