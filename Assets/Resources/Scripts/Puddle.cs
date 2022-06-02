using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    public string type;
    public List<string> comp_types;
    public float portion = 0.13f;
    float size;
    Vector3 scale;
    // Start is called before the first frame update
    void Start()
    {
        size = portion;
        scale = transform.localScale;    
        transform.localScale = scale * size;
    }

    public void grow()
    {
        size += portion;
        GetComponent<StickyBurger>().amount = size;
        //transform.localScale = scale * size;
    }

    public void devour(float othersize)
    {
        size += othersize;
        GetComponent<StickyBurger>().amount = size;
        //transform.localScale = scale * size;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        collision.GetContacts(contacts);
        foreach (ContactPoint ContPoint in contacts)
        {
            GameObject other = ContPoint.otherCollider.gameObject;
            Puddle p;
            if (other.TryGetComponent<Puddle>(out p))
            {
                if (p.type == type || p.comp_types.Contains(type))
                {
                    if ((p.transform.localScale.x > transform.localScale.x) || (p.GetInstanceID() < GetInstanceID() && p.transform.localScale.x == transform.localScale.x))
                    {
                        p.devour(size);
                        Destroy(gameObject);
                        return;
                    }
                }
            }
            /*
            if (other.TryGetComponent<Rigidbody>(out Rigidbody a) && a.isKinematic)
            {
                if(!TryGetComponent<ConfigurableJoint>(out _))
                {
                    ConfigurableJoint fj = gameObject.AddComponent<ConfigurableJoint>();
                    fj.connectedBody = other.GetComponentInParent<Rigidbody>();
                    Debug.Log(other.name + fj.connectedBody);
                    fj.enablePreprocessing = false;
                    fj.breakForce = 100;
                    fj.breakTorque = 75;
                    fj.angularXMotion = ConfigurableJointMotion.Locked;
                    fj.angularYMotion = ConfigurableJointMotion.Locked;
                    fj.angularZMotion = ConfigurableJointMotion.Locked;
                    fj.xMotion = ConfigurableJointMotion.Locked;
                    fj.yMotion = ConfigurableJointMotion.Locked;
                    fj.zMotion = ConfigurableJointMotion.Locked;
                }
            }
            */
        }
    }

    private void OnJointBreak(float breakForce)
    {
        Destroy(gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        collision.GetContacts(contacts);
        foreach (ContactPoint ContPoint in contacts)
        {
            Collider other = ContPoint.otherCollider;
            Debug.Log(other.name);
            if (!GetComponent<StickyBurger>().sticky)
            {
                Debug.Log("ready to pair");
                StickyBurger burger_to_go = other.GetComponentInParent<StickyBurger>();
                if (burger_to_go!= null)
                {
                    Debug.Log("sticking");
                    transform.rotation = Quaternion.LookRotation(burger_to_go.plate.transform.right);
                    burger_to_go.stick(GetComponent<Collider>());
                }
            }
        }
    }
    


    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.magnitude < (scale * size).magnitude)
        {
            if(transform.localScale.x < 1.25)
            {
                transform.localScale += scale * Time.deltaTime * 5;
            }
        }
    }
}
