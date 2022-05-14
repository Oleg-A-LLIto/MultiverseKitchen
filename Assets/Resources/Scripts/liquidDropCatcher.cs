using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liquidDropCatcher : MonoBehaviour
{
    public liquidReservoir liquid;

    void Start()
    {
        
    }

    public void captureDrop(liquidDroplet drop)
    {
        liquid.addLiquid(drop.ingredient, drop.volume);
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);
    }
}
