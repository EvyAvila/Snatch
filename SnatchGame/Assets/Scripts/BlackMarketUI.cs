using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackMarketUI : MonoBehaviour
{
    [SerializeField]
    private Button ExchangeButton;

    /*
    [SerializeField]
    private Button UpgradeButton;

    [SerializeField]
    private Button PurchaseButton;*/

    [SerializeField]
    private Button ExitButton;

    [SerializeField]
    private TextMeshProUGUI TitleText;

    [SerializeField]
    private TextMeshProUGUI TokenText;

    private int TokenAmount;

    private PlayerController player;

    [SerializeField]
    private Transform[] ReturnPosition;

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

        ExchangeButton.onClick.AddListener(ExchangeButtonAction);
        //UpgradeButton.onClick.AddListener(UpgradeButtonAction);
        //PurchaseButton.onClick.AddListener(PurchaseButtonAction);
        ExitButton.onClick.AddListener(ExitButtonAction); //help from Unity Manual : https://docs.unity3d.com/530/Documentation/ScriptReference/UI.Button-onClick.html

        TokenText.text = TokenString();

        ExitButton.enabled = false;
    }

    
    void Update()
    {
        TokenText.text = TokenString();

        if (player.StolenItems.Count != player.StolenItemsTotal)
        {
            ExitButton.enabled = true;
        }
        
    }

    private string TokenString()
    {
        return "Token Amount: " + TokenAmount;
    }

    private void ExchangeButtonAction()
    {
        Debug.Log("Exchange button clicked");
        
        if(player.StolenItems.Count > 0)
        {
            TokenAmount += (int)Mathf.Round((int)player.StolenItems[0].Value / 10);

            player.StolenItems.RemoveAt(0);



            //TokenAmount++;
            /*for (int i = 0; i < player.StolenItems.Count; i++)
            {
                player.StolenItems.RemoveAt(i);
                TokenAmount++;
            }*/
        }
    }
    private void UpgradeButtonAction()
    {
        Debug.Log("Upgrade button clicked");
    }
    private void PurchaseButtonAction() 
    {
        Debug.Log("Purchase button clicked");
    }
    private void ExitButtonAction()
    {
        Debug.Log("Exit button clicked");
        player.GetComponent<PlayerUI>().playerState = PlayerState.Active;

        int random = Random.Range(0, ReturnPosition.Length);

        player.gameObject.transform.position = ReturnPosition[random].position;
    }

    private void SetUIActivation(bool condition)
    {
        ExchangeButton.gameObject.SetActive(condition);
        //UpgradeButton.gameObject.SetActive(condition);
        //PurchaseButton.gameObject.SetActive(condition);
        ExitButton.gameObject.SetActive(condition);
        TitleText.gameObject.SetActive(condition);
        TokenText.gameObject.SetActive(condition);
    }
}
