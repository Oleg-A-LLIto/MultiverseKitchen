using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderOnOff : MonoBehaviour
{
    public void TurnOnTheColliderPickup()
    {
        GetComponent<MeshCollider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = false;
    }

    public void TurnOnTheColliderLeave()
    {
        GetComponent<Rigidbody>().useGravity = true;
    }
}
