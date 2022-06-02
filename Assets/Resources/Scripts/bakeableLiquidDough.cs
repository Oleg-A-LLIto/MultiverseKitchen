using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(liquidReservoir))]
public class bakeableLiquidDough : MonoBehaviour
{
    [SerializeField] GameObject bakedProduct;
    [SerializeField] float bakingThreshold;
    [SerializeField] float bakingSpeed;
    public Dictionary<string, float> doughISO { get; private set; }
    public float bakingProgress { get; private set; }
    public float quality { get; private set; }
    liquidReservoir liquidDough;
    Vector3 fullScale;

    void Start()
    {
        liquidDough = GetComponent<liquidReservoir>();
        declareDoughISO();
    }

    private void declareDoughISO()
    {
        doughISO = new Dictionary<string, float>();
        doughISO.Add("flour",0.3225f);
        doughISO.Add("sugar", 0.075f);
        doughISO.Add("eggyolk", 0.0025f);
        doughISO.Add("eggwhite", 0.1375f);
        doughISO.Add("milk", 0.4625f);
    }

    private void doughQualitySnapshot()
    {
        quality = liquidDough.compareContents(doughISO);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("hot"))
        {
            return;
        }   
        if (!bakedProduct.activeSelf)
        {
            if (liquidDough.fullness > bakingThreshold)
            {
                bakedProduct.SetActive(true);
                fullScale = bakedProduct.transform.localScale;
                float bakeDelta = bakingSpeed * Time.deltaTime;
                liquidDough.fullness -= bakeDelta;
                bakingProgress += bakeDelta;
                bakedProduct.transform.localScale = new Vector3(fullScale.x, Mathf.Lerp(0, fullScale.y, bakingProgress), fullScale.z);
                bakedProduct.GetComponent<MeshRenderer>().enabled = false;
                doughQualitySnapshot();
            }
        }
        else
        {
            if (liquidDough.fullness > 0)
            {
                float bakeDelta = bakingSpeed * Time.deltaTime;
                liquidDough.fullness -= bakeDelta;
                bakingProgress += bakeDelta;
                bakedProduct.transform.localScale = new Vector3(fullScale.x, Mathf.Lerp(0, fullScale.y, bakingProgress), fullScale.z);
                if (bakingProgress > (liquidDough.fullness*0.92f))
                {
                    bakedProduct.GetComponent<MeshRenderer>().enabled = true;
                    bakedProduct.transform.parent = null;
                    fullScale *= bakedProduct.transform.localScale.x / fullScale.x;
                }
            }
        }
    }

    void Update()
    {
        
    }
}
