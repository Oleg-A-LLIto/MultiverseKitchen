using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerAssembly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("burger_component"))
        {
            if (!other.attachedRigidbody.isKinematic)
            {
                if(!other.gameObject.TryGetComponent<FixedJoint>(out _))
                {
                    other.transform.parent = transform.parent;
                    FixedJoint fj = other.gameObject.AddComponent<FixedJoint>();
                    fj.connectedBody = transform.parent.GetComponent<Rigidbody>();
                    fj.enablePreprocessing = false;
                    fj.breakForce = 400;
                    fj.breakTorque = 400;
                    Debug.Log(other.gameObject.name + " is jailed :B");
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("burger_component"))
        {
            if (!other.attachedRigidbody.isKinematic)
            {
                transform.position += transform.up * Time.deltaTime * 0.1f;
                Debug.Log("Going up: " + transform.position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
