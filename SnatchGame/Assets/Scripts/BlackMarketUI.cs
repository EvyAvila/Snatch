using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JetBrains.Annotations;
using System.Linq;

public class BlackMarketUI : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private TextMeshProUGUI TitleText;

    [SerializeField]
    private TextMeshProUGUI TokenText;

    private int TokenAmount;

    private PlayerController player;

    [SerializeField]
    private Transform[] ReturnPosition;

    [SerializeField]
    private GameObject PurchasePanel;

    [SerializeField]
    private Button[] ItemsToBuy;

    [SerializeField]
    private Button[] MenuButtons;
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
        player = GetComponent<PlayerController>();        
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

        TokenText.text = TokenString();

        MenuButtons[MenuButtons.Length - 1].enabled = false;
        MenuButtons[0].Select();
        
        PurchasePanel.SetActive(false);
        
    }

    void Update()
    {
        TokenText.text = TokenString();

        if (player.StolenItems.Count != player.StolenItemsTotal && !PurchasePanel.activeInHierarchy )
        {
            MenuButtons[MenuButtons.Length-1].enabled = true;
        }
    }
    
    private string TokenString()
    {
        return "Token Amount: " + TokenAmount;
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
    /*private void UpgradeButtonAction()
    {
        Debug.Log("Upgrade button clicked");
    }*/
    private void PurchaseAction() 
    {
        PurchasePanel.SetActive(true);
        SetUIEnabled(false);
        BuyingSection();

    }
    private void ExitAction()
    {
        player.GetComponent<PlayerUI>().playerState = PlayerState.Active;

        int random = Random.Range(0, ReturnPosition.Length);

        player.gameObject.transform.position = ReturnPosition[random].position;
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
        ItemsToBuy[ItemsToBuy.Length - 1].Select();
    }

    private void Items(int num)
    {
       switch(num)
       {
            case 0:
                //Debug.Log("Boot bought");
                Calculation(10, ItemsToBuy, num);
                
                break;
            case 1:
                //Debug.Log("Gloves bought");
                Calculation(5, ItemsToBuy, num);
               
                break;
            case 2:
                //Debug.Log("Jacket bought");
                Calculation(15, ItemsToBuy, num);
                break;
            case 3:
                //Debug.Log("Return");
                PurchasePanel.SetActive(false);
                SetUIEnabled(true);
                MenuButtons[0].Select();
                break;
       }
    }

    private void Calculation(int price, Button[] b, int position)
    {
        if(price > TokenAmount)
        {
            Debug.Log("Not enough tokens");
        }
        else
        {
            TokenAmount -= price;
            b[position].enabled = false;
            ItemsToBuy[ItemsToBuy.Length - 1].Select();
        }
    }
}