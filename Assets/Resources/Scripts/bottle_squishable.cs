using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(kinematicAccelerometer))]
public class bottle_squishable : MonoBehaviour
{
    enum mode { squish, initial };
    mode m = mode.squish;
    Hand holding;
    Valve.VR.SteamVR_Skeleton_Poser poser;
    public Valve.VR.SteamVR_Skeleton_Pose pose1;
    public Valve.VR.SteamVR_Skeleton_Pose pose2;
    public Transform bottleneck;
    public GameObject droplet;
    kinematicAccelerometer accelerometer;

    // Start is called before the first frame update
    void Start()
    {
        poser = GetComponent<Valve.VR.SteamVR_Skeleton_Poser>();
        accelerometer = GetComponent<kinematicAccelerometer>();
    }

    private void Update()
    {
        CheckMode();
    }

    private void FixedUpdate()
    {
        
    }

    public void CheckMode()
    {
        holding = GetComponent<Interactable>().attachedToHand;
        if (holding != null)
        {
            //(holding.IsGrabbingWithType(GrabTypes.Grip))
            if ((accelerometer.acceleration < -0.002) && (Vector3.Angle(Vector3.up,transform.forward*-1) < 60) && (Vector3.Angle(accelerometer.velocityVector, transform.forward) < 30))
            {
                if (m != mode.squish)
                {
                    m = mode.squish;
                    StartCoroutine(Squish());
                    Debug.Log("just squished"); 
                }
            }
            else
            {
                if (m != mode.initial)
                {
                    m = mode.initial;
                    StartCoroutine(Retract());
                }
            }
        }
    }

    void MakeDroplet()
    {
        GameObject d = Instantiate(droplet);
        d.transform.position = bottleneck.position;
        d.transform.rotation = bottleneck.rotation;
        
    }

    IEnumerator Squish()
    {
        m = mode.squish;
        GetComponentInChildren<ParticleSystem>().Play();
        for(int j = 0; j<3; j++)
        {
            for (float i = 0; i < 0.1f; i += Time.deltaTime * 2)
            {
                yield return 0;
            }
            GameObject drop = Instantiate(droplet);
            drop.transform.position = bottleneck.position;
            drop.transform.rotation = bottleneck.rotation;
            drop.GetComponent<Rigidbody>().AddForce(transform.forward/36);
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Lerp(initangle, initangle + 90, i));
            yield return 0;
        }
        //poser.skeletonMainPose = pose2;
        GetComponentInChildren<ParticleSystem>().Stop();
        yield return 0;
    }

    IEnumerator Retract()
    {
        m = mode.initial;
        for (float i = 0; i < 1; i += Time.deltaTime * 2)
        {
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Lerp(initangle, initangle - 90, i));
            yield return 0;
        }
        //poser.skeletonMainPose = pose1;
        yield return 0;
    }
}
