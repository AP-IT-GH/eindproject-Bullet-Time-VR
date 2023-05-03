using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class TestAgent : Agent
{
    public GameObject Friendly;
    public GameObject Target;
    public GameObject Friendly_F;
    public GameObject Target_F;

    Rigidbody m_AgentRb;
    int m_Selection;
    StatsRecorder m_statsRecorder;

    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        m_statsRecorder = Academy.Instance.StatsRecorder;
    }

    public override void OnEpisodeBegin()
    {
        //Set target to onthoud
        m_Selection = Random.Range(0, 2);
        if (m_Selection == 1)
        {
            Friendly_F.transform.position = new Vector3(0f, 1f, 3f);
            Target_F.transform.position = new Vector3(0f, -100f, 3f);
        }
        else
        {
            Friendly_F.transform.position = new Vector3(0f, -100f, 3f);
            Target_F.transform.position = new Vector3(0f, 1f, 3f);
        }

        //Zet anget locatie
        transform.position = new Vector3(0f, 0.5f, 7f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        m_AgentRb.velocity *= 0f;

        //Switch position for targets to go to
        var goalPos = Random.Range(0, 2);
        if (goalPos == 0)
        {
            Friendly.transform.position = new Vector3(1f, 1f, -4f);
            Target.transform.position = new Vector3(-1f, 1f, -4f);
        }
        else
        {
            Friendly.transform.position = new Vector3(-1f, 1f, -4f);
            Target.transform.position = new Vector3(1f, 1f, -4f);
        }

        //Geen idee wat dit doe
        m_statsRecorder.Add("Goal/Correct", 0, StatAggregationMethod.Sum);
        m_statsRecorder.Add("Goal/Wrong", 0, StatAggregationMethod.Sum);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Target") || col.gameObject.CompareTag("Friendly"))
        {
            if ((m_Selection == 0 && col.gameObject.CompareTag("Target")) || (m_Selection == 1 && col.gameObject.CompareTag("Friendly")))
            {
                SetReward(1f);
                m_statsRecorder.Add("Goal/Correct", 1, StatAggregationMethod.Sum);
                print("Juist");
            }
            else
            {
                SetReward(-0.1f);
                m_statsRecorder.Add("Goal/Wrong", 1, StatAggregationMethod.Sum);
                print("Fout");
            }
            EndEpisode();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(StepCount / (float)MaxStep);
        sensor.AddObservation(transform.rotation.y);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        //AddReward(-1f / MaxStep);
        MoveAgent(actionBuffers.DiscreteActions);

        // Als agent onder de grond is of een blok aanraakt
        if (transform.localPosition.y < 0)
        {
            SetReward(-2f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
        }
        transform.Rotate(rotateDir, Time.deltaTime * 150f);
        m_AgentRb.AddForce(dirToGo * 0.5f, ForceMode.VelocityChange);
    }
}
