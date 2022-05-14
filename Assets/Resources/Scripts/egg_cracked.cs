using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class egg_cracked : MonoBehaviour
{
    [SerializeField]
    GameObject OtherHalf;
    float ReferenceDistance = 0;
    [SerializeField]
    float AcceptableDeviation = 0.1f;
    [SerializeField]
    GameObject[] EggParticlesHolder;
    bool cracked = false;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        //ReferenceDistance = (this.transform.position - OtherHalf.transform.position).magnitude;
        //Debug.Log("ReferenceDistance = " + ReferenceDistance);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0.7f)
        {
            timer += Time.deltaTime;
            return;
        }
        if (cracked)
            return;
        if(Mathf.Abs((this.transform.position - OtherHalf.transform.position).magnitude - ReferenceDistance) > AcceptableDeviation){
            Destroy(GetComponent<ConfigurableJoint>());
            foreach (GameObject holder in EggParticlesHolder)
            {
                foreach (Transform child in holder.transform.Cast<Transform>().ToList())
                {
                    if (child != holder.transform)
                    {
                        Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
                        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                        child.parent = null;
                    }
                }
            }
            cracked = true;
        }
    }
}
