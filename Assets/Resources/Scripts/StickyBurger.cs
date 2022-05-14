using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class StickyBurger : MonoBehaviour
{
    public bool sticky = false;
    public GameObject plate = null;
    public List<GameObject> stickkids;
    public int hierarchy_level = 99999;
    public string type;
    public float amount = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void JB_event(GameObject j)
    {
        Debug.Log(name + " broke free :D");
        BreakJoint(j);
    }

    private void Unparent(GameObject par)
    {
        par.GetComponent<StickyBurger>().stickkids.Remove(gameObject);
    }

    private void FallOff(GameObject par)
    {
        ConfigurableJoint[] joints = GetComponents<ConfigurableJoint>();
        for (int i = 0; i < joints.Length; i++)
        {
            if (joints[i].connectedBody.gameObject == par)
            {
                Destroy(joints[i]);
            }
        }
        if (!TryGetComponent<ConfigurableJoint>(out _))
        {
            DeathEvent();
        }
    }

    private void DeathEvent()
    {
        foreach (GameObject kid in stickkids)
        {
            kid.GetComponent<StickyBurger>().FallOff(gameObject);
        }
        hierarchy_level = 99999;
        plate = null;
        stickkids.Clear();
        sticky = false;
        GetComponent<Rigidbody>().drag = 1;
    }

    public void recursive_calc()
    {
        plate.GetComponent<CalculateIngredients>().ingreds.Add(this);
        if (type == "patty")
        {
            fryable fry = GetComponent<fryable>();
            FindObjectOfType<Rating>().FriedRating(fry.fried_bottom, fry.fried_overall, fry.fried_top);
        }
        for(int i = 0; i < stickkids.Count;)
        {
            if(stickkids[i] == null)
            {
                stickkids.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
        foreach (GameObject kid in stickkids)
        {
            if (kid.GetComponent<StickyBurger>().hierarchy_level > hierarchy_level)
            {
                plate.GetComponent<CalculateIngredients>().connections.Add(new IngredsConnection(this, kid.GetComponent<StickyBurger>()));
                kid.GetComponent<StickyBurger>().recursive_calc();
            }
        }
    }

    public void stick(Collider other)
    {
        if (sticky)
        {
            if (other.gameObject.CompareTag("burger_component"))
            {
                if (!other.attachedRigidbody.isKinematic)
                {
                    if (other.GetComponent<StickyBurger>().hierarchy_level > hierarchy_level)
                    {
                        if (!stickkids.Contains(other.gameObject))
                        {
                            if (!other.GetComponent<Interactable>().attachedToHand)
                            {
                                ConfigurableJoint fj = other.gameObject.AddComponent<ConfigurableJoint>();
                                fj.connectedBody = GetComponent<Rigidbody>();
                                fj.enablePreprocessing = false;
                                fj.breakForce = 100;
                                fj.breakTorque = 75;
                                //fj.enableCollision = true;
                                fj.angularXMotion = ConfigurableJointMotion.Locked;
                                fj.angularYMotion = ConfigurableJointMotion.Locked;
                                fj.angularZMotion = ConfigurableJointMotion.Locked;
                                fj.xMotion = ConfigurableJointMotion.Locked;
                                fj.yMotion = ConfigurableJointMotion.Locked;
                                fj.zMotion = ConfigurableJointMotion.Locked;
                                other.GetComponent<JointMonitor>().MonitorNew(fj);
                                other.GetComponent<StickyBurger>().plate = plate;
                                other.GetComponent<StickyBurger>().sticky = true;
                                other.GetComponent<StickyBurger>().hierarchy_level = hierarchy_level + 1;
                                other.GetComponent<Rigidbody>().drag = 50;
                                Debug.Log(other.gameObject.name + " is jailed :B");
                                stickkids.Add(other.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }


    /*
    private void OnTriggerEnter(Collider other)
    {
        stick(other);
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        //if (TryGetComponent<cuttable>(out _))
        //{
            stick(collision.collider);
        //}
    }

    public void BreakJoint(GameObject j)
    {
        Unparent(j);
        if (!TryGetComponent<ConfigurableJoint>(out _))
        {
            DeathEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Interactable>().attachedToHand)
        {
            if (sticky)
            {
                if (gameObject != plate)
                {
                    ConfigurableJoint[] myjoints = GetComponents<ConfigurableJoint>();
                    foreach (ConfigurableJoint j in myjoints)
                    {
                        if (j != null)
                        {
                            GameObject go = j.connectedBody.gameObject;
                            Destroy(j);
                            Unparent(go);
                        }
                    }
                    DeathEvent();
                }
            }
        }
    }
}

public struct IngredsConnection{
    public StickyBurger first;
    public StickyBurger second;
    public IngredsConnection(StickyBurger a, StickyBurger b){
        first = a;
        second = b;
    }
}