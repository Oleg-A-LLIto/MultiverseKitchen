using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public string type;
    public GameObject ownPuddle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        collision.GetContacts(contacts);
        foreach(ContactPoint ContPoint in contacts)
        {
            GameObject other = ContPoint.otherCollider.gameObject;
            if (other.TryGetComponent<Liquid>(out _))
            {
                return;
            }
            if (other.TryGetComponent<bottle_squishable>(out _))
            {
                return;
            }
            if (other.TryGetComponent<SphereCollider>(out _) || other.name=="Sphere")
            {
                return;
            }
            Puddle p;
            if (other.TryGetComponent<Puddle>(out p))
            {
                if (p.type == type || p.comp_types.Contains(type))
                {
                    p.grow();
                    Destroy(gameObject);
                    return;
                }
            }
            GameObject pud = Instantiate(ownPuddle);
            
            pud.transform.position = ContPoint.point;
            pud.transform.rotation = Quaternion.LookRotation(ContPoint.normal);
            pud.transform.RotateAround(pud.transform.position, pud.transform.forward, Random.Range(0,359));
            Destroy(gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
