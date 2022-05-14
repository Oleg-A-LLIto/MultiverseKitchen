using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fryable : MonoBehaviour
{
    public Color raw_color;
    public Color fried_color;
    public float fried_top = 0;
    public float fried_bottom = 0;
    public float fried_overall = 0;
    public GameObject top_friedness;
    public GameObject bottom_friedness;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = Instantiate(Resources.Load("Materials/raw_patty") as Material);
        top_friedness.GetComponent<MeshRenderer>().material = Instantiate(Resources.Load("Materials/trace") as Material);
        bottom_friedness.GetComponent<MeshRenderer>().material = Instantiate(Resources.Load("Materials/trace") as Material);
        //this.gameObject.GetComponent<MeshRenderer>().material.SetColor("_TintColor", Color.green);
    }

    public void onFry(bool top)
    {
        if (top)
        {
            fried_top += Time.deltaTime / 30;
            if (fried_top <= 1)
            {
                top_friedness.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", Mathf.Lerp(1, 0.4f, fried_top));
            }
            else
            {
                top_friedness.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", Mathf.Lerp(0.4f, 0.1f, fried_top-1));
            }
        }
        else
        {
            fried_bottom += Time.deltaTime / 30;
            if (fried_bottom <= 1)
            {
                bottom_friedness.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", Mathf.Lerp(1,0.4f, fried_bottom));
            }
            else
            {
                bottom_friedness.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", Mathf.Lerp(0.4f, 0.1f, fried_bottom));
            }
        }
        fried_overall += Time.deltaTime / 60;
        if (fried_overall <= 1)
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(raw_color,fried_color,fried_overall);
        }
        else
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(fried_color, Color.black, fried_overall-1);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
