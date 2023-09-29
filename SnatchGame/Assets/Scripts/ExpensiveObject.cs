using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpensiveObject : MonoBehaviour
{
    private Material objColorNormal;

    [SerializeField]
    private Material objColorActive;

    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        objColorNormal = GetComponent<Renderer>().material;
        rend = GetComponent<MeshRenderer>();
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
