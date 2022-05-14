using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JointMonitor_fixed : MonoBehaviour
{
    public List<FixedJoint> m_MonitoredJoints;
    public List<GameObject> JointObjects;

    void Start()
    {
        m_MonitoredJoints = new List<FixedJoint>();
        JointObjects = new List<GameObject>();
        GetComponents<FixedJoint>(m_MonitoredJoints);
    }

    public void MonitorNew(FixedJoint newJ)
    {
        m_MonitoredJoints.Add(newJ);
        JointObjects.Add(newJ.connectedBody.gameObject);
    }

    void FixedUpdate()
    {
        for (var i = 0; i < m_MonitoredJoints.Count; ++i)
        {
            var joint = m_MonitoredJoints[i];
            if (joint)
                continue;
            GameObject go = JointObjects[i];
            m_MonitoredJoints.RemoveAt(i);
            JointObjects.RemoveAt(i);
            Debug.Log("calling JB event");
            GetComponent<StickyBurger>().JB_event(go);
        }
    }
}