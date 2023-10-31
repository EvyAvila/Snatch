using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpensiveObject : ExpensiveBase
{
    public GameObject FloatingTextPrefab;

    private Material objColorNormal;

    [SerializeField]
    private Material objColorActive;

    private Renderer rend;

    [SerializeField]
    private string objName;
    
    void Start()
    {
        Name = objName;
        objColorNormal = GetComponent<Renderer>().material;
        rend = GetComponent<MeshRenderer>();
        SetObject();
        //Debug.Log(Value);
    }

    
    void FixedUpdate()
    {
        //Debug.Log(Value.ToString("c"));
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Hand"))
        {
            rend.material = objColorActive;
            
            FloatingTextPrefab.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            rend.material = objColorNormal;
            FloatingTextPrefab.SetActive(false);
        }
    }

    public void SetObject()
    {
        Value = Random.Range(50, 150);
        rend.material = objColorNormal;
        FloatingTextPrefab.SetActive(false);
        FloatingTextPrefab.transform.position = this.transform.position;
    }
}
