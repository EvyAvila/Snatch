using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerClass : MonoBehaviour, ISelectHandler
{
    //Based from https://discussions.unity.com/t/checking-if-button-is-highlighted-with-a-controller-no-mouse/223282/2
    [SerializeField]
    private TextMeshProUGUI PurchaseDescriptionText;

    private void Awake()
    {
        if(PurchaseDescriptionText == null)
        {
            PurchaseDescriptionText = GameObject.Find("PurchaseDescription").GetComponent<TextMeshProUGUI>();
           
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(eventData.selectedObject)
        {
            DisplayText(this.gameObject.name);
        }
    }


    private void DisplayText(string name)
    {
        switch (name)
        {
            case "ReduceDetection":
                PurchaseDescriptionText.text = "Decrease your detection meter by 1 point";
                break;
            case "Boots":
                PurchaseDescriptionText.text = "Boots to increase your speed movement which will allow you to steal items more efficiently";
                break;
            case "Gloves":
                PurchaseDescriptionText.text = "Gloves to increase your arm movement speed to Snatch items faster";
                break;
            case "Jacket":
                PurchaseDescriptionText.text = "A Jacket to give you extra pocket space to store more stolen items";
                break;
            case "Return":
                PurchaseDescriptionText.text = "";
                break;
        }

    }
}
