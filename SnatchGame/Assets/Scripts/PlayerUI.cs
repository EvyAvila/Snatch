using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState { Lose, Win, Active, Market} //Active means the player is playing in game

public class PlayerUI : MonoBehaviour, IDetectionCount
{
    public PlayerState playerState;

    [SerializeField]
    private TextMeshProUGUI DetectionText;

    [SerializeField]
    private TextMeshProUGUI EndingStateText;

    [SerializeField]
    private TextMeshProUGUI InventoryText;

    [SerializeField]
    private TextMeshProUGUI DetectiveFollowText;

    [SerializeField]
    private TextMeshProUGUI PenaltyText;

    public int DetectionAmount { get; set; }
    private float DetectionMaxValue;
    private float CooldownMaxValue;
    
    private int DetectionMax;

    private PlayerController player;

    public bool PlayerAttention { get; private set; }

    private Detective detective;
    private bool removeText;


    public Audio[] GameAudio;

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

        PlayerAttention = false;

        DetectionText.transform.Find("DetectionBar").GetComponent<Slider>().maxValue = DetectionMax;
        InventoryText.transform.Find("InventoryBar").GetComponent<Slider>().maxValue = player.StolenItemsTotal;
        PenaltyText.transform.Find("PenaltyMeter").GetComponent<Slider>().value = 0;
        
        detective = GameObject.Find("Detective").GetComponent<Detective>();

        DetectiveFollowText.text = "";
        removeText = false;

        GameAudio[0].PlayAudio();
    }

    // Update is called once per frame
    void Update()
    {
        //testAudio.GamePlayMusic();
        if (DetectionAmount <= DetectionMax)
        {
            DetectionText.text = DetectionString();

        }

        //Lose
        if (DetectionAmount == DetectionMax)
        {
            //EndingStateText.text = EndingString("Lose");
            playerState = PlayerState.Lose;
            GameAudio[0].StopAudio();
           
            //Time.timeScale = 0;
        }

        //Win
        if (player.StolenItemsTotal == player.StolenItems.Count)
        {
            //EndingStateText.text = EndingString("Completed the level");
            playerState = PlayerState.Win;
            GameAudio[0].StopAudio();

        }

        switch (playerState)
        {
            case PlayerState.Lose:

                if(!GameAudio[1].audioName.isPlaying) //checking isPlaying help from https://stackoverflow.com/questions/49451295/audio-not-playing-in-unity
                {
                    GameAudio[1].PlayAudio();
                }
                
                EndingStateText.text = EndingString("Lose \nPress any button to restart game.");
                break;
            case PlayerState.Active:
                
                EndingStateText.text = string.Empty;
                break;
            case PlayerState.Win:
                if (!GameAudio[2].audioName.isPlaying)
                {
                    GameAudio[2].PlayAudio();
                }
                EndingStateText.text = EndingString("Completed the level");
                break;

        }

        InventoryText.text = InventoryString();

        if (PenaltyText.transform.Find("PenaltyMeter").GetComponent<Slider>().value == 0)
        {
            PenaltyText.text = string.Empty;
        }
        else if (player.PenaltyActive)
        {
            PenaltyText.text = "Lay low, you're being suspicious";
        }
        else
        {
            PenaltyText.text = "Warning...";
        }

        ChangeSlider();

        PlayerAttention = player.PenaltyActive;
        DetectiveFollowUI();
    }

    private void DetectiveFollowUI()
    {
        if (detective.followPlayer && !detective.lostPlayer)
        {
            DetectiveFollowText.text = "You are being followed...";
        }
        else if (!removeText && detective.lostPlayer)
        {
            DetectiveFollowText.text = "You slipped away...";
            StartCoroutine(RemoveText());
        }
        else if (removeText && detective.lostPlayer)
        {
            DetectiveFollowText.text = "";
        }
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
        DetectiveFollowText.gameObject.SetActive(condition);
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

    public IEnumerator DetectionIncrease(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (!GameAudio[3].audioName.isPlaying)
        {
            GameAudio[3].PlayAudio();
        }
        DetectionAmount += 2;

        StopAllCoroutines();
    }

    IEnumerator RemoveText()
    {
        yield return new WaitForSeconds(2);
        removeText = true;
        //DetectiveFollowText.text = "";
        StopAllCoroutines();
    }

    
}

public interface IDetectionCount
{
    IEnumerator DetectionIncrease(float waitTime);
    
}