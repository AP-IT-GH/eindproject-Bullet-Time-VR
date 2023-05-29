using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class FinaalCameraAgent : Agent
{
    public enum direction
    {
        links = 0,
        rechts = 1,
        center = 2
    }
    public enum kleur
    {
        groen = 0,
        rood = 1
    }

    //Game objecten
    public GameObject Groen;
    public GameObject Rood;


    public shoot shootScript;
    bool shot = false;

    Vector3 links = new Vector3(2f, 1f, -4f);
    Vector3 rechts = new Vector3(-2f, 1f, -4f);

    float rotation=0f;
    public float rotationSpeed = 20f;
    public float stepSize = 5f;


    //public override void OnEpisodeBegin()
    //{
    //    shot = false;

    //    transform.localPosition = new Vector3(0f, 1f, 7f);
    //    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    //    rotation = 0f;

    //    int GreenLeft = Random.Range(0, 2);
    //    if (GreenLeft == 0)
    //    {
    //        Groen.transform.localPosition = rechts;
    //        Rood.transform.localPosition = links;
    //    }
    //    else
    //    {
    //        Groen.transform.localPosition = links;
    //        Rood.transform.localPosition = rechts;
    //    }

    //}

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(shot);
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        bool shoot = actionBuffers.DiscreteActions[0] == 1;
        transform.Rotate(0.0f, 2.0f * actionBuffers.ContinuousActions[0], 0.0f);

        if (shoot && !shot)
        {
            shot = true;
            print("Shoot");
            string tag = shootScript.Shoot();

            if (tag == "Friendly")
            {
                AddReward(1f);
            }
            else if(tag == "Target")
            {
                AddReward(-1f);
            }
            else
            {
                AddReward(-0.5f);
            }

            print("cummulative: " + GetCumulativeReward());
            EndEpisode();
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var DiscreteActionsOut = actionsOut.DiscreteActions;
        var continuousActionsOut = actionsOut.ContinuousActions;

        if (Input.GetKey(KeyCode.Space))
        {
            DiscreteActionsOut[0] = 1;
        }
        else
        {
            DiscreteActionsOut[0] = 0;

        }
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
    }



    public void Reset()
    {
        shot = false;

    }
}
