using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dippable : MonoBehaviour
{
    [SerializeField] Material dippedMaterial;
    [SerializeField] bool exclusive;
    [SerializeField] GameObject[] dippingAllowed;
    public bool dipped { get; private set; } = false;
    public Transform sprinkleContainer { get; private set; }
    public Dictionary<string, float> glazeISO { get; private set; }
    public float quality { get; private set; }

    void Start()
    {
        sprinkleContainer = transform.GetChild(0);
        declareGlazeISO();
    }

    private void glazeQualitySnapshot(liquidReservoir liquidDough)
    {
        quality = liquidDough.compareContents(glazeISO);
    }

    public int countSprinkles()
    {
        return sprinkleContainer.childCount;
    }

    private void declareGlazeISO()
    {
        glazeISO = new Dictionary<string, float>();
        glazeISO.Add("sugar", 0.25f);
        glazeISO.Add("milk", 0.75f);
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
            glazeQualitySnapshot(dippingReservoir);
        }
    }

    void Update()
    {

    }
}
