using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kinematicAccelerometer : MonoBehaviour
{
    public float acceleration { get; private set; } = 0;
    public float velocity { get; private set; } = 0;
    public Vector3 velocityVector { get; private set; }
    float lastVelocity = 0;
    Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        velocityVector = transform.position - lastPosition;
        velocity = velocityVector.magnitude;
        acceleration = velocity - lastVelocity;
        lastVelocity = velocity;
        lastPosition = transform.position;
    }
}
