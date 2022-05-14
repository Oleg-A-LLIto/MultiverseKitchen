using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BzKovSoft.ObjectSlicerSamples;

public class pestle : MonoBehaviour
{
    BzKnife thisBlade;
    [SerializeField]
    float timer = 0.3f;

    void Start()
    {
        thisBlade = GetComponent<BzKnife>();
        StartCoroutine(rotateBlade());
    }

    IEnumerator rotateBlade()
    {
        transform.localRotation = Quaternion.Euler(0,0,UnityEngine.Random.Range(0, 180));
        for(float i = 0; i < timer; i += Time.deltaTime)
        {
            yield return 0;
        }
        StartCoroutine(rotateBlade());
    }

    void Update()
    {
        /*
        transform.localRotation = Quaternion.Euler(0,0,thisBlade.SliceID%180);
        Debug.Log("Slice id: " + thisBlade.SliceID + " Angle: " + thisBlade.SliceID % 180);
        */
    }
}
