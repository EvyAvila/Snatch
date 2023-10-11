using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerText : MonoBehaviour
{
    [SerializeField]
    private TextMesh[] ControllerTextObj;

    [SerializeField]
    private int WaitTime;

    void Start()
    {
        //ControllerTextObj = GetComponent<TextMesh[]>();
        DisplayText(0);
        StartCoroutine(Timer());
    }

    void Update()
    {
        
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(WaitTime);
        DisplayText(255);
        StopAllCoroutines();

    }

    private void DisplayText(int num)
    {
        foreach(var v in ControllerTextObj)
        {
            v.color = new Color(219, 219, 156, num);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            DisplayText(0);
        }
    }
}
