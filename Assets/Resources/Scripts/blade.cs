using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blade : MonoBehaviour
{
    //CutMultiplePartsConcaveAndConcavePolygon razor;
    CutSimpleConcave razor;
    float cut_timer = 0;
    public GameObject food_prefab;
    // Start is called before the first frame update
    void Start()
    {
        //razor = GetComponent<CutMultiplePartsConcaveAndConcavePolygon>();
        razor = GetComponent<CutSimpleConcave>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision!");
        Debug.Log(collision);
        cutable tocut;
        if (collision.collider.TryGetComponent<cutable>(out tocut))
        {
            Debug.Log("found cutable!");
            if (cut_timer <= 0)
            {
                cut_timer = 2;
                Debug.Log("can cut");
                razor.target = tocut.gameObject;
                razor.prefabPart = food_prefab;
                razor.Cut();
                StartCoroutine(Set_cut_timer());
            }
            else
            {
                Debug.Log("cant cut due to a recent cut "+cut_timer);
            }
        }
        else
        {
            Debug.Log("no cutables here");
        }
    }

    IEnumerator Set_cut_timer()
    {
        for(; cut_timer > 0; cut_timer -= Time.deltaTime)
        {
            yield return 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}