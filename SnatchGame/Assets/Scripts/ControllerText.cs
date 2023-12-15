using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerText : MonoBehaviour
{
    //[SerializeField]
    //private TextMesh[] ControllerTextObj;

    [SerializeField]
    private int WaitTime;

    [SerializeField]
    private Transform PlayerSpawnLocation;

    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private Civilian DummyTut;

    bool PlayerHasTeleported;

    void Start()
    {
        //ControllerTextObj = GetComponent<TextMesh[]>();
        //DisplayText(0);
        StartCoroutine(Timer());
        PlayerHasTeleported = false;
    }

    void Update()
    {
        //TokenText.transform.Find("Token").GetComponent<TextMeshProUGUI>().text = TokenString();
        if (!DummyTut.transform.Find("Object").GetComponent<Transform>().gameObject.activeInHierarchy && !PlayerHasTeleported)
        {
            StartCoroutine(ChangePosition());
        }

        
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(WaitTime);
        //DisplayText(255);
        StopAllCoroutines();
    }

    IEnumerator ChangePosition()
    {
        yield return new WaitForSeconds(2);
        Player.transform.position = PlayerSpawnLocation.position;
        DummyTut.gameObject.SetActive(false);
        PlayerHasTeleported= true;
        StopAllCoroutines();

    }


    
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //DisplayText(0);
            
        }
    }
}
