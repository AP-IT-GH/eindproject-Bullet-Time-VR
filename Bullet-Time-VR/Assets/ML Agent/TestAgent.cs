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
    int goalPos;
    StatsRecorder m_statsRecorder;

    public shoot shootScript;
    bool shot = false;

    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        m_statsRecorder = Academy.Instance.StatsRecorder;
    }

    public override void OnEpisodeBegin()
    {
        shot = false;
        Friendly.SetActive(true);
        Target.SetActive(true);

        m_Selection = Random.Range(0, 2);
        if (m_Selection == 0)
        {
            Friendly.transform.localPosition = new Vector3(2f, 1f, -4f);
            Target.transform.localPosition = new Vector3(-2f, 1f, -4f);
        }
        else
        {
            Friendly.transform.localPosition = new Vector3(-2f, 1f, -4);
            Target.transform.localPosition = new Vector3(2f, 1f, -4);
        }

        transform.localPosition = new Vector3(0f ,1f, 7f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        m_AgentRb.velocity *= 0f;

        goalPos = Random.Range(0, 2);
        if (goalPos == 0)
        {
            Friendly_F.transform.localPosition = new Vector3(0f, 1f, 10f);
            Target_F.transform.localPosition = new Vector3(0f, -1000f, 10f);
        }
        else
        {
            Friendly_F.transform.localPosition = new Vector3(0f, -1000f, 10f);
            Target_F.transform.localPosition = new Vector3(0f, 1f, 10f);
        }
        m_statsRecorder.Add("Goal/Correct", 0, StatAggregationMethod.Sum);
        m_statsRecorder.Add("Goal/Wrong", 0, StatAggregationMethod.Sum);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.rotation.y);
        sensor.AddObservation(shot);
    }
 
    public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        bool shoot = actionBuffers.DiscreteActions[0] == 1;
        print(shoot);
        transform.Rotate(0.0f, 2.0f * actionBuffers.ContinuousActions[0], 0.0f);
        if (shoot && !shot)
        {
            shot = true;
            print("Shoot");
            if (shootScript != null)
            {
                string tag = shootScript.Shoot();
                if ((tag == "Friendly" && goalPos == 0) || (tag == "Target" && goalPos == 1))
                {
                    print("Target hit");
                    SetReward(1);
                }
                else if ((tag == "Friendly" && goalPos == 1) || (tag == "Target" && goalPos == 0))  //Hit wrong
                {
                    print("Wrong target");
                    SetReward(-0.5f);
                }
                else if (tag == null)    //Hit nothing
                {
                    print("Miss");
                    SetReward(-0.1f);
                }
                delay();
                EndEpisode();
            }
        }
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var DiscreteActionsOut = actionsOut.DiscreteActions;
        var continuousActionsOut = actionsOut.ContinuousActions;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            DiscreteActionsOut[0] = 1;
        }
        else
        {
            DiscreteActionsOut[0] = 0;

        }
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
    }
}
