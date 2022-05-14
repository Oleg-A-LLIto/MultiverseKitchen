using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goToAnotherRoom : MonoBehaviour
{
    public int sceneToGoTo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void go()
    {
        SceneManager.LoadScene(sceneToGoTo);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
