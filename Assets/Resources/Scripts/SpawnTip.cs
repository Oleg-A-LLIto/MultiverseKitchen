using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SpawnTip : MonoBehaviour
{
    public VideoClip vc;
    public Vector3 pos;
    public Vector3 rot;
    GameObject tip = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Spawn()
    {
        if (tip == null)
        {
            tip = Instantiate(Resources.Load("Prefabs/VideoTip") as GameObject);
            tip.GetComponent<VideoPlayer>().clip = vc;
            tip.transform.position = pos;
            tip.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
