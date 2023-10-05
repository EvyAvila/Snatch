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

    public int DetectionAmount { get; set; }
    private int DetectionMax;

    

    // Start is called before the first frame update
    void Start()
    {
        DetectionMax = 10;
        DetectionText.text = DetectionString();

        EndingStateText.text = "";
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
    }

    private string EndingString(string condition)
    {
        return "You " + condition;
    }

    private string DetectionString()
    {
        return DetectionAmount + "/" + DetectionMax + " detection";
    }
}
