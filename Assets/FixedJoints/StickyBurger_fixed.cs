using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickyBurger_fixed : MonoBehaviour
{
    public bool sticky = false;
    public GameObject plate = null;
    public List<GameObject> stickkids;
    public int hierarchy_level = 99999;
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
        FixedJoint[] joints = GetComponents<FixedJoint>();
        for (int i = 0; i < joints.Length; i++)
        {
            if (joints[i].connectedBody.gameObject == par)
            {
                Destroy(joints[i]);
            }
        }
        if (!TryGetComponent<FixedJoint>(out _))
        {
            DeathEvent();
        }
    }

    private void DeathEvent()
    {
        foreach (GameObject kid in stickkids)
        {
            kid.GetComponent<StickyBurger_fixed>().FallOff(gameObject);
        }
        hierarchy_level = 99999;
        plate = null;
        stickkids.Clear();
        sticky = false;
    }

    private void stick(Collider other)
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
                            FixedJoint fj = other.gameObject.AddComponent<FixedJoint>();
                            fj.connectedBody = GetComponent<Rigidbody>();
                            fj.enablePreprocessing = false;
                            fj.breakForce = 100;
                            fj.breakTorque = 75;
                            other.GetComponent<JointMonitor_fixed>().MonitorNew(fj);
                            other.GetComponent<StickyBurger>().plate = plate;
                            other.GetComponent<StickyBurger>().sticky = true;
                            other.GetComponent<StickyBurger>().hierarchy_level = hierarchy_level + 1;
                            Debug.Log(other.gameObject.name + " is jailed :B");
                            stickkids.Add(other.gameObject);
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        stick(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (TryGetComponent<cuttable>(out _))
        {
            Debug.Log("HIT!");
            stick(collision.collider);
        }
    }

    public void BreakJoint(GameObject j)
    {
        Unparent(j);
        if (!TryGetComponent<FixedJoint>(out _))
        {
            DeathEvent();
        }
    }

    /*
        
    */

    // Update is called once per frame
    void Update()
    {
        
    }
}
