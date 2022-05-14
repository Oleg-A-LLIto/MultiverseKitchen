using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createPlayer : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public GameObject[] initiateAfterThePlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            foreach (GameObject playerPrefab in playerPrefabs)
            {
                GameObject obj = Object.Instantiate(playerPrefab);
                DontDestroyOnLoad(obj);
                obj.transform.rotation = transform.rotation;
            }
        }
        foreach (GameObject otherPrefab in initiateAfterThePlayer)
        {
            Object.Instantiate(otherPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
