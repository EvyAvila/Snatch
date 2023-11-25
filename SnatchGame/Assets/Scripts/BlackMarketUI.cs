using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlackMarketUI : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Image TitleText;

    [SerializeField]
    private Image TokenText;

    private int TokenAmount;

    private PlayerController player;
    //private Detective detective;

    [SerializeField]
    private Transform[] ReturnPosition;

    [SerializeField]
    private GameObject PurchasePanel;

    [SerializeField]
    private Button[] ItemsToBuy;

    [SerializeField]
    private Button[] MenuButtons;

    [SerializeField]
    private GameObject[] ItemGameObjects;

    [SerializeField]
    private TextMeshProUGUI DenyPurchaseText;

    private int[] ItemCost = {10, 30, 20, 50 };

    private GameManager gm;

    private PlayerUI playerUI;
    
    //[SerializeField]
    //private TextMeshProUGUI PurchaseDescriptionText;
    //private bool SetButtonSelection;
    

    #endregion
    private void OnEnable()
    {
        SetUIActivation(true);
    }

    private void OnDisable()
    {
        SetUIActivation(false);
    }

    void Start()
    {
        //SetButtonSelection = true;
        player = GetComponent<PlayerController>();        
        //detective= GameObject.Find("Detective").GetComponent<Detective>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        for(int i = 0; i < MenuButtons.Length; i++)
        {
            int buttonNum = i;
            MenuButtons[i].onClick.AddListener(() => Menu(buttonNum));
        }

        for (int i = 0; i < ItemsToBuy.Length; i++)
        {
            int buttonNum = i;
            ItemsToBuy[i].onClick.AddListener(() => Items(buttonNum)); //Assistance from Chat.gpt and part from Unity Manual : https://docs.unity3d.com/530/Documentation/ScriptReference/UI.Button-onClick.html
        }

        //TokenText.text = TokenString();
        TokenText.transform.Find("Token").GetComponent<TextMeshProUGUI>().text = TokenString();

        MenuButtons[MenuButtons.Length - 1].enabled = false;
        
        MenuButtons[0].Select();
        
        PurchasePanel.SetActive(false);
        
        for(int i = 0; i < ItemsToBuy.Length - 1; i++)
        {
            ItemsToBuy[i].transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = ItemCost[i] + " tokens";
        }
        //Physics.gravity = new Vector3(0, -70, 0);
        playerUI = GetComponent<PlayerUI>();

        DenyText(0, DenyPurchaseText);

        //PurchaseDescriptionText.text = string.Empty;

        

       
    }

    void Update()
    {
       

        TokenText.transform.Find("Token").GetComponent<TextMeshProUGUI>().text = TokenString();

        //if (player.StolenItems.Count != player.StolenItemsTotal && !PurchasePanel.activeInHierarchy )
        if (player.StolenItems.Count != player.StolenItemsTotal && !PurchasePanel.activeInHierarchy )
        {
            MenuButtons[MenuButtons.Length-1].enabled = true;
            
        }
        else
        {
            MenuButtons[MenuButtons.Length - 1].enabled = false;
            
        }
    }
    
    private string TokenString()
    {
        return "Tokens: " + TokenAmount;
    }

    private void Menu(int num)
    {
        switch (num)
        {
            case 0:
                ExchangeAction();
                break;
            case 1:
                PurchaseAction();
                break;
            case 2:
                ExitAction();
                break;      
        }
    }

    #region MenuActions
    private void ExchangeAction()
    {       
        if(player.StolenItems.Count > 0)
        {
            TokenAmount += (int)Mathf.Round((int)player.StolenItems[0].Value / 10);

            player.StolenItems.RemoveAt(0);
        }
    }
    private void PurchaseAction() 
    {
        PurchasePanel.SetActive(true);
        DenyPurchaseText.gameObject.SetActive(false);
        SetUIEnabled(false);
        BuyingSection();

    }
    private void ExitAction()
    {
        player.GetComponent<PlayerUI>().playerState = PlayerState.Active;

        int random = Random.Range(0, ReturnPosition.Length);

        player.gameObject.transform.position = ReturnPosition[random].position;

        gm.ResetObjects();

        MenuButtons[0].Select();
    }
    #endregion
    #region UIActive
    private void SetUIActivation(bool condition)
    {
        foreach(var v in MenuButtons)
        {
            v.gameObject.SetActive(condition);
        }
        
        TitleText.gameObject.SetActive(condition);
        TokenText.gameObject.SetActive(condition);
        DenyPurchaseText.gameObject.SetActive(condition);
        //PurchaseDescriptionText.gameObject.SetActive(condition);

        foreach(var v in ItemsToBuy)
        {
            v.gameObject.SetActive(condition);
        }
    }
    private void SetUIEnabled(bool condition)
    {
        foreach(var v in MenuButtons)
        {
            v.enabled= condition;
        }
    }
    #endregion
    
    private void BuyingSection()
    {
        //ItemsToBuy[ItemsToBuy.Length - 1].Select();
        ItemsToBuy[0].Select();
    }

    private void Items(int num)
    {
        //DisplayPurchaseDescription(num);
        
       switch(num)
       {
            case 0:
                ReductionDetectionCalculation(ItemCost[0]);
                break;
            case 1: //Boots
                Calculation(ItemCost[1], ItemsToBuy, num, ItemGameObjects[0]);
                break;
            case 2: //Gloves
                Calculation(ItemCost[2], ItemsToBuy, num, ItemGameObjects[1]);
                if (ItemGameObjects[1].activeSelf)
                {
                    ItemGameObjects[2].SetActive(true);
                }
                break;
            case 3: //jacket
                Calculation(ItemCost[3], ItemsToBuy, num, ItemGameObjects[3]);
                break;
            case 4: //Return
                PurchasePanel.SetActive(false);
                SetUIEnabled(true);
                MenuButtons[0].Select();
                break;
       }
    }

    private void Calculation(int price, Button[] b, int position, GameObject g)
    {
        if(price > TokenAmount)
        {
            //Debug.Log("Not enough tokens");
            StartCoroutine(DisplayDenyPurchaseText(0));
        }
        else
        {
            TokenAmount -= price;
            b[position].enabled = false;
            ItemsToBuy[ItemsToBuy.Length - 1].Select();
            
            g.SetActive(true);
            b[position].transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = string.Empty;

            ChangeStats(g);
        }
    }

    private void ChangeStats(GameObject item)
    {
        if (item.CompareTag("Boots"))
        {
            player.Speed = player.Speed + 5;
        }
        else if(item.CompareTag("Jacket"))
        {
            //lower timer of detective following player
            //detective.timeRemaining = detective.timeRemaining / 2;
            player.StolenItemsTotal = player.GetTotal; 
        }
        else if(item.CompareTag("Gloves"))
        {
            //increase speed of arm movement
            float armRotateSpeed = player.gameObject.transform.Find("RightArm").GetComponent<PlayerArm>().RotateSpeed;
           
            //Is there another way to do this? 
            player.gameObject.transform.Find("RightArm").GetComponent<PlayerArm>().RotateSpeed = armRotateSpeed + 15;
            player.gameObject.transform.Find("LeftArm").GetComponent<PlayerArm>().RotateSpeed = armRotateSpeed + 15;
        }
    }

    IEnumerator DisplayDenyPurchaseText(int num)
    {
        DenyText(num, DenyPurchaseText);
        DenyPurchaseText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        DenyPurchaseText.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    private void ReductionDetectionCalculation(int price)
    {
        if(price > TokenAmount) //if they have enough tokens
        {
            StartCoroutine(DisplayDenyPurchaseText(0));
        }
        else
        {
            if(playerUI.DetectionAmount > 0) //if the detection is greater than 0
            {
                //Debug.Log(playerUI.DetectionAmount.ToString());
                StartCoroutine(DisplayDenyPurchaseText(2));
                TokenAmount -= price;
                playerUI.DetectionAmount--;
            }
            else
            {
                StartCoroutine(DisplayDenyPurchaseText(1));
                //Debug.Log("Your detection is at the lowest.");
            }
        }
    }

    private string DenyText(int changetext, TextMeshProUGUI displaytext)
    {
        switch(changetext)
        {
            case 0:
            default:
                displaytext.text = "Not enough tokens...";
                break;
            case 1:
                displaytext.text = "Detection is at lowest point";
                break;
            case 2:
                displaytext.text = "Detection reduced by 1";
                break;

        }

        return displaytext.text;
    }
}