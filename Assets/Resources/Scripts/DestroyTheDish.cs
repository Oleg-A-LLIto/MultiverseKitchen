using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTheDish : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] food = GameObject.FindGameObjectsWithTag("burger_component");
        foreach (GameObject thing in food)
        {
            Destroy(thing);
        }
        Destroy(FindObjectOfType<CalculateIngredients>().gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
