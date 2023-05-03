using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class TestAgent : Agent
{
    public GameObject ground;

    public GameObject Friendly;
    public GameObject Target;
    public GameObject Friendly_F;
    public GameObject Target_F;

    Rigidbody m_AgentRb;
    Material m_GroundMaterial;
    Renderer m_GroundRenderer;
    HallwaySettings m_HallwaySettings;
    int m_Selection;
    StatsRecorder m_statsRecorder;


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(StepCount / (float)MaxStep);
    }
}
