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

    kleur CurrentColor;

    public shoot shootScript;
    bool shot = false;

    direction lookAt;
    direction positionGroen;
    Vector3 links = new Vector3(2f, 1f, -4f);
    Vector3 rechts = new Vector3(-2f, 1f, -4f);


    public override void OnEpisodeBegin()
    {
        shot = false;

        lookAt = direction.center;
        transform.localPosition = new Vector3(0f, 1f, 7f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        int GreenLeft = Random.Range(0, 2);
        if (GreenLeft == 0)
        {
            Groen.transform.localPosition = rechts;
            positionGroen = direction.rechts;
            Rood.transform.localPosition = links;
        }
        else
        {
            Groen.transform.localPosition = links;
            positionGroen = direction.links;
            Rood.transform.localPosition = rechts;
        }


        CurrentColor = kleur.groen;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(shot);
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        bool shoot = actionBuffers.DiscreteActions[0] == 1;
        int rotate = actionBuffers.DiscreteActions[1];

        if (rotate == 1 && !shot)
        {    //Right
            Vector3 newRotation = new Vector3(0, -160, 0);
            transform.eulerAngles = newRotation;
            lookAt = direction.rechts;
        }
        else if (rotate == 2 && !shot)  //Left
        {
            Vector3 newRotation = new Vector3(0, 160, 0);
            transform.eulerAngles = newRotation;
            lookAt = direction.links;
        }

        if (shoot && !shot && lookAt != direction.center)
        {
            shot = true;
            print("Shoot");

            if (CurrentColor == kleur.groen)
                CheckGroen();

            print("cummulative: " + GetCumulativeReward());
            EndEpisode();
        }

    }
    private void CheckGroen()
    {
        print("GreenCheck");
        if (positionGroen == direction.links && lookAt == direction.links)
        {
            AddReward(1f);
        }
        else if (positionGroen == direction.rechts && lookAt == direction.rechts)
        {
            AddReward(1f);
        }
        else
        {
            AddReward(-1f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var DiscreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.Space))
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
        else if (horizontal < 0)
        {
            DiscreteActionsOut[1] = 2;
        }
    }
}
