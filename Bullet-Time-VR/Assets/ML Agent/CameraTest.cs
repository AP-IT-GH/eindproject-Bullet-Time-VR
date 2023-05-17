using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CameraTest : Agent
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
    string direction = "C";

    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        m_statsRecorder = Academy.Instance.StatsRecorder;
    }

    public override void OnEpisodeBegin()
    {
        shot = false;
        direction = "C";
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

        transform.localPosition = new Vector3(0f, 1f, 7f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        m_AgentRb.velocity *= 0f;


        //goalPos = Random.Range(0, 2);
        goalPos = 0;
        if (goalPos == 0)
        {
            Friendly_F.SetActive(false);    //PAS DIT AAN
            Friendly_F.transform.localPosition = new Vector3(0f, 1f, 10f);
            Target_F.SetActive(false);
            Target_F.transform.localPosition = new Vector3(0f, 1f, 10f);
        }
        else
        {
            Friendly_F.SetActive(false);
            Friendly_F.transform.localPosition = new Vector3(0f, 1f, 10f);
            Target_F.SetActive(true);
            Target_F.transform.localPosition = new Vector3(0f, 1f, 10f);
        }
        m_statsRecorder.Add("Goal/Correct", 0, StatAggregationMethod.Sum);
        m_statsRecorder.Add("Goal/Wrong", 0, StatAggregationMethod.Sum);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(Friendly.transform.localPosition);
       // sensor.AddObservation(Target.transform.localPosition);
        sensor.AddObservation(shot);
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        bool shoot = actionBuffers.DiscreteActions[0] == 1;
        int rotate = actionBuffers.DiscreteActions[1];
        print(transform.localRotation);

        if (rotate == 1 && !shot) {    //Right
            Vector3 newRotation = new Vector3(0, -160, 0);
            transform.eulerAngles = newRotation;
            direction = "R";
        }
        else if (rotate == 2 && !shot)  //Left
        {
            Vector3 newRotation = new Vector3(0, 160, 0);
            transform.eulerAngles = newRotation;
            direction = "L";
        }

        if (shoot && !shot && direction != "C")
        {
            shot = true;
            print("Shoot");
            //GoalPos 0 = groen
            //Mselection 0 = groen L rood R
            if ((goalPos == 0 && m_Selection==0 && direction == "L") || (m_Selection == 1 && goalPos == 0 && direction == "R"))
            {
                print("Groen");
                SetReward(1f);
            }
            else if ((m_Selection == 1 && goalPos == 1 && direction == "L") || (m_Selection == 0 && goalPos == 1 && direction == "R"))  //Rechts
            {
                //print("Rood");
                print("NEEN");
                //SetReward(1f);
            }
            else
            {
                print("FOUT");
                SetReward(-0.5f);
            }
           // print(m_Selection);
            //print(goalPos);
           // print(direction);
            EndEpisode();
        }
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var DiscreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            DiscreteActionsOut[0] = 1;
        }
        else
        {
            DiscreteActionsOut[0] = 0;

        }

        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0)
        {
            DiscreteActionsOut[1] = 1;
        }
        else if(horizontal < 0)
        {
            DiscreteActionsOut[1] = 2;
        }
    }
}
