using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class spatula : MonoBehaviour
{
    enum mode { cut, spat };
    mode m = mode.cut;
    Hand holding;
    public GameObject razor1;
    public GameObject razor2;
    public GameObject picker;
    Valve.VR.SteamVR_Skeleton_Poser poser;
    public Valve.VR.SteamVR_Skeleton_Pose pose1;
    public Valve.VR.SteamVR_Skeleton_Pose pose2;
    // Start is called before the first frame update
    void Start()
    {
        poser = GetComponent<Valve.VR.SteamVR_Skeleton_Poser>();
    }

    public void CheckMode()
    {
        holding = GetComponent<Interactable>().attachedToHand;
        if(holding != null)
        {
            if (holding.IsGrabbingWithType(GrabTypes.Grip))
            {
                if (m != mode.cut)
                {
                    StartCoroutine(GotoKnife());
                }
            }
            else
            {
                if (m != mode.spat)
                {
                    StartCoroutine(GotoSpat());
                }
            }
        }
    }

    IEnumerator GotoKnife()
    {
        float initangle = transform.rotation.eulerAngles.z;
        m = mode.cut;
        for(float i = 0; i < 1; i += Time.deltaTime*2)
        {
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Lerp(initangle, initangle + 90, i));
            yield return 0;
        }
        picker.SetActive(false);
        razor1.SetActive(true);
        razor2.SetActive(true);
        //poser.skeletonMainPose = pose2;
        yield return 0;
    }

    IEnumerator GotoSpat()
    {
        float initangle = transform.rotation.eulerAngles.z;
        m = mode.spat;
        for (float i = 0; i < 1; i += Time.deltaTime * 2)
        {
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Lerp(initangle, initangle - 90, i));
            yield return 0;
        }
        picker.SetActive(true);
        razor1.SetActive(false);
        razor2.SetActive(false);
        //poser.skeletonMainPose = pose1;
        yield return 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
