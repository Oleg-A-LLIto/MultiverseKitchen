using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreRenderer : MonoBehaviour
{
    public string category;
    // Start is called before the first frame update
    void Start()
    {
        int score = PlayerPrefs.GetInt(category);
        GetComponent<Text>().text = (score > 0) ? PlayerPrefs.GetInt(category) + "%" : "..."; 
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
