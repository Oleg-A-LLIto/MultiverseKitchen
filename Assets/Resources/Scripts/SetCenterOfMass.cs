using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCenterOfMass : MonoBehaviour
{
    public Transform CenterOfMass;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = CenterOfMass.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
