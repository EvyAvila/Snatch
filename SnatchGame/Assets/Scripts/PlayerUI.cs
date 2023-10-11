using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI DetectionText;

    [SerializeField]
    private TextMeshProUGUI EndingStateText;

    [SerializeField]
    private TextMeshProUGUI InventoryText;

    public int DetectionAmount { get; set; }
    private int DetectionMax;

    private PlayerController player;
   

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
    }

    // Update is called once per frame
    void Update()
    {
        if(DetectionAmount <= DetectionMax)
        {
            DetectionText.text = DetectionString();
        }
        
        if(DetectionAmount == DetectionMax)
        {
            EndingStateText.text = EndingString("Lose");
            Time.timeScale = 0;
        }

        if(player.StolenItemsTotal == player.StolenItems.Count)
        {
            EndingStateText.text = EndingString("Completed the level");
            Time.timeScale = 0;
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
