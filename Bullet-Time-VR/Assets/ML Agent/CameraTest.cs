using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;


public class CameraTest : Agent
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
    public GameObject Groen_vb;
    public GameObject Rood_vb;

    kleur CurrentColor;

    public shoot shootScript;
    bool shot = false;
    bool turned = false;

    direction lookAt;
    direction positionGroen;
    direction positionRood;
    Vector3 links = new Vector3(2f, 1f, -4f) ;
    Vector3 rechts = new Vector3(-2f, 1f, -4f);

    public override void Initialize()
    {
        Groen_vb.transform.localPosition = new Vector3(0f, 1f, 10f);
        Rood_vb.transform.localPosition = new Vector3(0f, 1f, 10f);
    }

    public override void OnEpisodeBegin()
    {
        shot = false;
        turned = false;

        lookAt = direction.center;
        transform.localPosition = new Vector3(0f, 1f, 7f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        int GreenLeft = Random.Range(0, 2);
        if (GreenLeft == 0)
        {
            Groen.transform.localPosition = rechts;
            positionGroen = direction.rechts;
            Rood.transform.localPosition = links;
            positionRood = direction.links;
        }
        else
        {
            Groen.transform.localPosition = links;
            positionGroen = direction.links;
            Rood.transform.localPosition = rechts;
            positionRood = direction.rechts;
        }

        int rndColor = Random.Range(0, 2);
        if (rndColor == 0)//Groen aan
        {
            CurrentColor = kleur.groen;
            Groen_vb.SetActive(true); //Groen           
            Rood_vb.SetActive(false);  //Rood           
        }
        else
        {
            CurrentColor = kleur.rood;
            Groen_vb.SetActive(false);          
            Rood_vb.SetActive(true);           
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(Friendly.transform.localPosition);
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(shot);
        //sensor.AddObservation(turned);

        //sensor.AddObservation(goalPos);
        //sensor.AddObservation(m_Selection);
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        bool shoot = actionBuffers.DiscreteActions[0] == 1;
        int rotate = actionBuffers.DiscreteActions[1];

        if (rotate == 1 && !shot) {    //Right
            Vector3 newRotation = new Vector3(0, -160, 0);
            transform.eulerAngles = newRotation;
            lookAt = direction.rechts;
            turned = true;
        }
        else if (rotate == 2 && !shot)  //Left
        {
            Vector3 newRotation = new Vector3(0, 160, 0);
            transform.eulerAngles = newRotation;
            lookAt = direction.links;
            turned = true;
        }

        if (shoot && !shot && lookAt != direction.center)
        {
            shot = true;
            print("Shoot");

            if (CurrentColor == kleur.groen)
                CheckGroen();

            if (CurrentColor == kleur.rood)
                CheckRood();

            print("cummulative: " + GetCumulativeReward());
            EndEpisode();
        }

    }
    private void CheckGroen()
    {
        print("GreenCheck");
        if ( positionGroen == direction.links && lookAt == direction.links)
        {
            AddReward(1f);
        }
        else if ( positionGroen == direction.rechts && lookAt == direction.rechts)
        {
            AddReward(1f);
        }
        else
        {
            AddReward(-1f);
        }
    }
    private void CheckRood()
    {
        print("RedCheck");
        
        if (positionRood == direction.links && lookAt == direction.links)  //Rechts
        {
            AddReward(1f);
        }
        else if(positionRood == direction.rechts  && lookAt == direction.rechts)
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
        else if(horizontal < 0)
        {
            DiscreteActionsOut[1] = 2;
        }
    }
}
