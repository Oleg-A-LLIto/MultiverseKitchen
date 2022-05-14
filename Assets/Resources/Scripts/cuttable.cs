using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BzKovSoft.ObjectSlicer.EventHandlers;
using Valve.VR.InteractionSystem;

public class cuttable : MonoBehaviour, IBzObjectSlicedEvent
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ObjectSliced(GameObject original, GameObject resultNeg, GameObject resultPos)
    {
        Hand commonhand = original.GetComponent<Interactable>().attachedToHand;
        resultNeg.GetComponent<StickyBurger>().amount = VolumeOfMesh(resultNeg.GetComponent<MeshFilter>().mesh) * 1000000;
        resultPos.GetComponent<StickyBurger>().amount = VolumeOfMesh(resultPos.GetComponent<MeshFilter>().mesh) * 1000000;
        Debug.Log(VolumeOfMesh(resultNeg.GetComponent<MeshFilter>().mesh)*1000000 + ", " + VolumeOfMesh(resultPos.GetComponent<MeshFilter>().mesh) * 1000000);
        if (commonhand != null)
        {
            GrabTypes grab = GrabTypes.None;
            cuttable neg = resultNeg.GetComponent<cuttable>();
            cuttable pos = resultPos.GetComponent<cuttable>();
            if (commonhand.IsGrabbingWithType(GrabTypes.None))
            {
                Debug.Log("None");
                grab = GrabTypes.None;
            }
            else if (commonhand.IsGrabbingWithType(GrabTypes.Trigger))
            {
                Debug.Log("Trigger");
                grab = GrabTypes.Trigger;
            }
            else if (commonhand.IsGrabbingWithType(GrabTypes.Pinch))
            {
                Debug.Log("Pinch");
                grab = GrabTypes.Pinch;
            }
            else if (commonhand.IsGrabbingWithType(GrabTypes.Grip))
            {
                Debug.Log("Grip");
                grab = GrabTypes.Grip;
            }
            else if (commonhand.IsGrabbingWithType(GrabTypes.Scripted))
            {
                Debug.Log("Scripted");
                grab = GrabTypes.Scripted;
            }
            else
            {
                Debug.Log("What the fuck");
            }
            //neg.transform.parent = original.transform;
            //pos.transform.parent = original.transform;
            //Transform posoffset = new Transform();
            //posoffset.position = pos.transform.position - original.transform.position;

            //, Quaternion.Angle(pos.transform.rotation, original.transform.rotation), 0
            
            commonhand.DetachObject(original.gameObject);
            commonhand.DetachObject(pos.gameObject);
            pos.gameObject.transform.parent = null;
            pos.GetComponent<Rigidbody>().isKinematic = false;
            commonhand.DetachObject(neg.gameObject);
            neg.gameObject.transform.parent = null;
            neg.GetComponent<Rigidbody>().isKinematic = false;
            if ((neg.transform.position - commonhand.transform.position).magnitude < (pos.transform.position - commonhand.transform.position).magnitude)
            {
                //original.transform.position = neg.transform.position;
                neg.transform.SetParent(commonhand.transform, true);
                commonhand.AttachObject(neg.gameObject, grab, Hand.AttachmentFlags.AllowSidegrade | Hand.AttachmentFlags.TurnOnKinematic | Hand.AttachmentFlags.ParentToHand);
            }
            else
            {
                //original.transform.position = pos.transform.position;
                pos.transform.SetParent(commonhand.transform, true);
                commonhand.AttachObject(pos.gameObject, grab, Hand.AttachmentFlags.AllowSidegrade | Hand.AttachmentFlags.TurnOnKinematic | Hand.AttachmentFlags.ParentToHand);
                //, Hand.AttachmentFlags.AllowSidegrade | Hand.AttachmentFlags.ParentToHand
            }

        }
        else
        {
            Debug.Log("The object wasnt in any hand");
        }
    }

    public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;

        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    public float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
