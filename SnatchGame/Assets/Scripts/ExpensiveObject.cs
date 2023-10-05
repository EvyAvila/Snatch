using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpensiveObject : ExpensiveBase
{
    private Material objColorNormal;

    [SerializeField]
    private Material objColorActive;

    private Renderer rend;

    [SerializeField]
    private string objName;
    
    //[SerializeField]
    //private double objValue;

    // Start is called before the first frame update
    void Start()
    {
        Name = objName;
        Value = Random.Range(10, 150);

        objColorNormal = GetComponent<Renderer>().material;
        rend = GetComponent<MeshRenderer>();

        Debug.Log(Name + " is worth: " + Value.ToString("c"));
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Hand"))
        {
            rend.material = objColorActive;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            rend.material = objColorNormal;
        }
    }
}
