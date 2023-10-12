using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState { Lose, Win, Active, Market} //Active means the player is playing in game

public class PlayerUI : MonoBehaviour
{
    public PlayerState playerState;

    [SerializeField]
    private TextMeshProUGUI DetectionText;

    [SerializeField]
    private TextMeshProUGUI EndingStateText;

    [SerializeField]
    private TextMeshProUGUI InventoryText;

    public int DetectionAmount { get; set; }
    private int DetectionMax;

    private PlayerController player;

    private void OnEnable()
    {
        DetectionText.gameObject.SetActive(true);
        EndingStateText.gameObject.SetActive(true);
        InventoryText.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        DetectionText.gameObject.SetActive(false);
        EndingStateText.gameObject.SetActive(false);
        InventoryText.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        DetectionMax = 10;
        DetectionText.text = DetectionString();

        EndingStateText.text = "";

        if (player == null)
        {
            player = GameObject.Find("BasePlayer").GetComponent<PlayerController>();
        }

        InventoryText.text = InventoryString();

        playerState = PlayerState.Active;
    }

    // Update is called once per frame
    void Update()
    {
        if(DetectionAmount <= DetectionMax)
        {
            DetectionText.text = DetectionString();
        }
        
        //Lose
        if(DetectionAmount == DetectionMax)
        {
            //EndingStateText.text = EndingString("Lose");
            playerState = PlayerState.Lose;
            //Time.timeScale = 0;
        }
        
        //Win
        if(player.StolenItemsTotal == player.StolenItems.Count)
        {
            //EndingStateText.text = EndingString("Completed the level");
            playerState = PlayerState.Win;

        }
        
        switch (playerState)
        {
            case PlayerState.Lose:
                EndingStateText.text = EndingString("Lose");
                break;
            case PlayerState.Active:
                EndingStateText.text = string.Empty;
                break;
            case PlayerState.Win:
                EndingStateText.text = EndingString("Completed the level");
                break;

        }


        InventoryText.text = InventoryString();
    }

    private string EndingString(string condition)
    {
        return "You " + condition;
    }

    private string DetectionString()
    {
        return DetectionAmount + "/" + DetectionMax + " detection";
    }

    private string InventoryString()
    {
        double total = 0;
        foreach(var v in player.StolenItems)
        {
            total += v.Value;
        }

        return player.StolenItems.Count + $"/{player.StolenItemsTotal} stolen item(s).  \nTotal worth: " + total.ToString("c");
    }

   
}
