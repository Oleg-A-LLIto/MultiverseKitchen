using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fry_trigger : MonoBehaviour
{
    public bool top;
    GameObject patty;
    // Start is called before the first frame update
    void Start()
    {
        patty = transform.parent.gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("hot"))
        {
            patty.GetComponent<fryable>().onFry(top);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
