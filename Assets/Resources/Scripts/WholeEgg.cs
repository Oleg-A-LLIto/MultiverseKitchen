using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WholeEgg : MonoBehaviour
{
    [SerializeField]
    float terminalVelocity = 5;
    [SerializeField]
    GameObject crackedEgg;
    bool alreadyCracked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (alreadyCracked)
        {
            return;
        }
        if (collision.relativeVelocity.magnitude > terminalVelocity)
        {
            alreadyCracked = true;
            Debug.Log("Egg cracked");
            GameObject thisEggCracked = Instantiate(crackedEgg);
            thisEggCracked.transform.position = transform.position;
            thisEggCracked.transform.rotation = transform.rotation;
            if(GetComponent<Interactable>().attachedToHand != null)
            {
                Hand holdingHand = GetComponent<Interactable>().attachedToHand;
                holdingHand.DetachObject(gameObject, true);
                Debug.Log(thisEggCracked.transform.GetChild(0).name);
                holdingHand.AttachObject(thisEggCracked.transform.GetChild(0).gameObject,GrabTypes.Grip);
                /*
                thisEggCracked.GetComponentInChildren<Interactable>().attachedToHand = GetComponent<Interactable>().attachedToHand;
                thisEggCracked.transform.GetChild(0).parent = transform.parent;
                GetComponent<Interactable>().attachedToHand = null;
                transform.parent = null;
                */
            }
            Destroy(gameObject);
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody othersRB = null;
        float relativeVelocity;
        if (other.TryGetComponent<Rigidbody>(out othersRB))
        {
            relativeVelocity = (this.GetComponent<Rigidbody>().velocity - othersRB.velocity).magnitude;
        }
        else
        {
            relativeVelocity = this.GetComponent<Rigidbody>().velocity.magnitude;
        }
        if (relativeVelocity > terminalVelocity)
        {
            Debug.Log("Egg cracked");
            GameObject thisEggCracked = Instantiate(crackedEgg);
            if (GetComponent<Interactable>().attachedToHand != null)
            {
                thisEggCracked.GetComponentInChildren<Interactable>().attachedToHand = GetComponent<Interactable>().attachedToHand;
            }
            Destroy(gameObject);
        }
    }
    */

    // Update is called once per frame
    void Update()
    {
        
    }
}
