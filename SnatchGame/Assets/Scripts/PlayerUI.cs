using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    /*
    [SerializeField]
    private Slider PenalitySlider;*/

    [SerializeField]
    private TextMeshProUGUI PenaltyText;

    public int DetectionAmount { get; set; }
    private float DetectionMaxValue;
    private float CooldownMaxValue;
    
    private int DetectionMax;

    private PlayerController player;

    public bool PlayerAttention { get; private set; }

    private void OnEnable()
    {
        SetUIActivation(true);
    }

    private void OnDisable()
    {
        if(DetectionText != null) //For some reason was getting a MissingException error when I playe and stop right away
        {
            SetUIActivation(false);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        DetectionMax = 20;
        DetectionText.text = DetectionString();

        EndingStateText.text = "";

        if (player == null)
        {
            player = GameObject.Find("BasePlayer").GetComponent<PlayerController>();
        }

        InventoryText.text = InventoryString();

        playerState = PlayerState.Active;

        DetectionMaxValue = player.timeRemaining;
        CooldownMaxValue = player.CooldownTimer;

        //PenalitySlider.value = 0;
        PlayerAttention = false;

        DetectionText.transform.Find("DetectionBar").GetComponent<Slider>().maxValue = DetectionMax;
        InventoryText.transform.Find("InventoryBar").GetComponent<Slider>().maxValue = player.StolenItemsTotal;
        PenaltyText.transform.Find("PenaltyMeter").GetComponent<Slider>().value = 0;
        
        //PenalitySlider.maxValue = player.timeRemaining;
    }

    // Update is called once per frame
    void Update()
    {
        if (DetectionAmount <= DetectionMax)
        {
            DetectionText.text = DetectionString();
            
        }

        //Lose
        if (DetectionAmount == DetectionMax)
        {
            //EndingStateText.text = EndingString("Lose");
            playerState = PlayerState.Lose;
            //Time.timeScale = 0;
        }

        //Win
        if (player.StolenItemsTotal == player.StolenItems.Count)
        {
            //EndingStateText.text = EndingString("Completed the level");
            playerState = PlayerState.Win;

        }

        switch (playerState)
        {
            case PlayerState.Lose:
                EndingStateText.text = EndingString("Lose \nPress any button to restart game.");
                break;
            case PlayerState.Active:
                EndingStateText.text = string.Empty;
                break;
            case PlayerState.Win:
                EndingStateText.text = EndingString("Completed the level");
                break;

        }
        
        InventoryText.text = InventoryString();

        if (PenaltyText.transform.Find("PenaltyMeter").GetComponent<Slider>().value == 0)
        {
            PenaltyText.text = string.Empty;
        }
        else if(player.PenaltyActive)
        {
            PenaltyText.text = "Lay low, you're being suspicious";
        }
        else
        {
            PenaltyText.text = "Warning...";
        }

        ChangeSlider();

        PlayerAttention = player.PenaltyActive;
    }

    private string EndingString(string condition)
    {
        return "You " + condition;
    }

    private string DetectionString()
    {
        //return DetectionAmount + "/" + DetectionMax + " detection";
        DetectionText.transform.Find("DetectionBar").GetComponent<Slider>().value = DetectionAmount;
        return "Detection:";
    }

    private string InventoryString()
    {
        double total = 0;
        foreach (var v in player.StolenItems)
        {
            total += v.Value;
        }

        InventoryText.transform.Find("InventoryBar").GetComponent<Slider>().maxValue = player.StolenItemsTotal;//This is here to make sure it updates when the user gets the upgrade
        InventoryText.transform.Find("InventoryBar").GetComponent<Slider>().value = player.StolenItems.Count;

        //return player.StolenItems.Count + $"/{player.StolenItemsTotal} stolen item(s).  \nTotal worth: " + total.ToString("c");
        return "Inventory: \nTotal worth: " + total.ToString("c");
    }
    private void SetUIActivation(bool condition)
    {
        DetectionText.gameObject.SetActive(condition);
        EndingStateText.gameObject.SetActive(condition);
        InventoryText.gameObject.SetActive(condition);
        PenaltyText.gameObject.SetActive(condition);
    }

    private void ChangeSlider()
    {
        switch (player.PenaltyActive)
        {
            case true:
                PenaltyText.transform.Find("PenaltyMeter").GetComponent<Slider>().maxValue = CooldownMaxValue;
                PenaltyText.transform.Find("PenaltyMeter").GetComponent<Slider>().value = player.DetectionMeterCount;

                StartCoroutine(DetectionIncrease(4));

                break;
            case false:
                PenaltyText.transform.Find("PenaltyMeter").GetComponent<Slider>().maxValue = DetectionMaxValue;
                PenaltyText.transform.Find("PenaltyMeter").GetComponent<Slider>().value = player.DetectionMeterCount;
                break;
        } 
    }

    IEnumerator DetectionIncrease(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        DetectionAmount += 2;

        StopAllCoroutines();
    }
}