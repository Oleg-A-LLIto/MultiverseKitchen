using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dippable : MonoBehaviour
{
    [SerializeField] Material dippedMaterial;
    [SerializeField] bool exclusive;
    [SerializeField] GameObject[] dippingAllowed;
    bool dipped = false;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        liquidDropCatcher dippingLiquid;
        if (!other.TryGetComponent<liquidDropCatcher>(out dippingLiquid))
        {
            return;
        }
        if (exclusive)
        {
            int i;
            for(i = 0; i < dippingAllowed.Length; i++)
                if(dippingAllowed[i] = other.gameObject)
                    break;
            if (i == dippingAllowed.Length)
                return;
        }
        liquidReservoir dippingReservoir = dippingLiquid.liquid;
        if (Vector3.Angle(other.transform.forward, transform.up) > 40)
        {
            return;
        }
        if(dippingReservoir.fullness > 0.25f)
        {
            dipped = true;
            Debug.Log("successfully dipped");
            GetComponent<MeshRenderer>().material = dippedMaterial;
        }
    }

    void Update()
    {

    }
}
