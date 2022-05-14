using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BzKovSoft.ObjectSlicer.EventHandlers;
using Valve.VR.InteractionSystem;
using BzKovSoft.ObjectSlicerSamples;

public class dissolvable : MonoBehaviour, IBzObjectSlicedEvent
{
    public float shareOfInitialVolume;
    public float initialVolume;
    [SerializeField] float dissolveThreshold;
    [SerializeField] string ingredient;

    void Start()
    {
        
    }

    public void ObjectSliced(GameObject original, GameObject resultNeg, GameObject resultPos)
    {
        float negVol = VolumeOfMesh(resultNeg.GetComponent<MeshFilter>().mesh) * 100000000;
        float posVol = VolumeOfMesh(resultPos.GetComponent<MeshFilter>().mesh) * 100000000;
        float totalVol = negVol + posVol;
        float negVolShare = negVol / totalVol * shareOfInitialVolume;
        float posVolShare = posVol / totalVol * shareOfInitialVolume;

        if (negVolShare <= dissolveThreshold)
        {
            Destroy(resultNeg.GetComponent<dissolvable>());
            Destroy(resultNeg.GetComponent<ObjectSlicerSample>());
            liquidDroplet drop = resultNeg.AddComponent<liquidDroplet>();
            drop.setup(ingredient, true, negVolShare * initialVolume);
        }
        else
        {
            resultNeg.GetComponent<dissolvable>().shareOfInitialVolume = negVolShare;
        }
        if (posVolShare <= dissolveThreshold)
        {
            Destroy(resultPos.GetComponent<dissolvable>());
            Destroy(resultPos.GetComponent<ObjectSlicerSample>());
            liquidDroplet drop = resultPos.AddComponent<liquidDroplet>();
            drop.setup(ingredient, true, posVolShare * initialVolume);
        }
        else
        {
            resultPos.GetComponent<dissolvable>().shareOfInitialVolume = posVolShare;
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
