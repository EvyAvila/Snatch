using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionText : MonoBehaviour
{
    private TextMesh ControllerText;

    [SerializeField]
    private bool FirstInstruction;

    // Start is called before the first frame update
    void Start()
    {
       ControllerText= GetComponent<TextMesh>();
        DisplayText(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(FirstInstruction)
        {
            DisplayText(255);
            StartCoroutine(RemoveText());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            DisplayText(255);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DisplayText(255);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !FirstInstruction)
        {
            DisplayText(0);
            this.gameObject.SetActive(false);
        }
        
    }

    IEnumerator RemoveText()
    {
        yield return new WaitForSeconds(5);
        DisplayText(0);
        this.gameObject.SetActive(false);
    }
    
    private void DisplayText(int num)
    {
        ControllerText.color = new Color(219, 219, 156, num);
    }

}
