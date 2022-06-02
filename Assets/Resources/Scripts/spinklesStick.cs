using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class spinklesStick : MonoBehaviour
{
    [SerializeField] float stickTimeout = 0.3f;
    [SerializeField] int angleThreshold = 170;
    bool stuck = false;
    Transform cakeRef;
    Collision currentCollision;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (stuck)
        {
            return;
        }
        if (GetComponent<Interactable>().attachedToHand)
        {
            return;
        }
        dippable cake;
        if(!collision.collider.TryGetComponent<dippable>(out cake))
        {
            return;
        }
        
        if (Vector3.Angle(cake.transform.up*-1, (transform.position - cake.transform.position)) > angleThreshold)
        {
            return;
        }
        
        if (cake.dipped)
        {
            stuck = true;
            cakeRef = cake.sprinkleContainer;
            StartCoroutine(stickToGlaze(stickTimeout));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (stuck)
        {
            stuck = false;
            StopAllCoroutines();
        }
            
    }

    IEnumerator stickToGlaze(float timer)
    {
        for(float i = 0; i < timer; i += Time.deltaTime)
        {
            yield return 0;
        }
        if (stuck)
        {
            //if (Vector3.Angle(cakeRef.transform.up, (transform.position - cakeRef.transform.position)) <= angleThreshold)
            //{
                Destroy(GetComponent<Throwable>());
                Destroy(GetComponent<Interactable>());
                Destroy(GetComponent<Rigidbody>());
                transform.parent = cakeRef;
            //}
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
