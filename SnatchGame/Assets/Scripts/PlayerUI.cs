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

    [SerializeField]
    private Slider DetectionSlider;

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

        DetectionSlider.value = 0;
        PlayerAttention = false;
        //DetectionSlider.maxValue = player.timeRemaining;
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

        ChangeSlider();

        PlayerAttention = player.PenaltyActive;
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
        foreach (var v in player.StolenItems)
        {
            total += v.Value;
        }

        return player.StolenItems.Count + $"/{player.StolenItemsTotal} stolen item(s).  \nTotal worth: " + total.ToString("c");
    }
    private void SetUIActivation(bool condition)
    {
        DetectionText.gameObject.SetActive(condition);
        EndingStateText.gameObject.SetActive(condition);
        InventoryText.gameObject.SetActive(condition);
        DetectionSlider.gameObject.SetActive(condition);
    }

    private void ChangeSlider()
    {
        switch (player.PenaltyActive)
        {
            case true:
                DetectionSlider.maxValue = CooldownMaxValue;
                DetectionSlider.value = player.DetectionMeterCount;

                StartCoroutine(DetectionIncrease(4));

                break;
            case false:
                DetectionSlider.maxValue = DetectionMaxValue;
                DetectionSlider.value = player.DetectionMeterCount;
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