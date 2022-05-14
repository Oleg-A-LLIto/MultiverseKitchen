using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bakeableLiquidDough : MonoBehaviour
{
    [SerializeField] GameObject bakedProduct;
    [SerializeField] float bakingThreshold;
    [SerializeField] float bakingSpeed;
    liquidReservoir liquidDough;
    Vector3 fullScale;
    float bakingProgress = 0;

    void Start()
    {
        liquidDough = GetComponent<liquidReservoir>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("hot"))
        {
            if (!bakedProduct.activeSelf)
            {
                if (liquidDough.fullness > bakingThreshold)
                {
                    bakedProduct.SetActive(true);
                    fullScale = bakedProduct.transform.localScale;
                    float bakeDelta = bakingSpeed * Time.deltaTime;
                    liquidDough.fullness -= bakeDelta;
                    bakingProgress += bakeDelta;
                    //float productHeight = bakedProduct.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
                    //bakedProduct.transform.position -= productHeight * fullScale.z * Vector3.up * 0.4f;
                    bakedProduct.transform.localScale = new Vector3(fullScale.x, Mathf.Lerp(0, fullScale.y, bakingProgress), fullScale.z);
                    bakedProduct.GetComponent<MeshRenderer>().enabled = false;
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
    }

    void Update()
    {
        
    }
}
