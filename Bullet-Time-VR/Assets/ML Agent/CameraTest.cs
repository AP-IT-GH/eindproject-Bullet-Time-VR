using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

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
public class CameraTest : Agent
{
    public GameObject Friendly;
    public GameObject Target;
    public GameObject Friendly_F;
    public GameObject Target_F;

    Rigidbody m_AgentRb;
    int m_Selection;
    int goalPos;
    kleur selectedKleur;
    StatsRecorder m_statsRecorder;

    public shoot shootScript;
    bool shot = false;
   // string direction = "C";
    direction lookAt;
    direction positionGroen;
    direction positionRood;
    Vector3 links;
    Vector3 rechts;

    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        m_statsRecorder = Academy.Instance.StatsRecorder;
        links = new Vector3(2f, 1f, -4f);
        rechts = new Vector3(-2f, 1f, -4f);
        Friendly_F.transform.localPosition = new Vector3(0f, 1f, 10f);
        Target_F.transform.localPosition = new Vector3(0f, 1f, 10f);
    }

    public override void OnEpisodeBegin()
    {
        shot = false;
        //  direction = "C";
        lookAt = direction.center;
        Friendly.SetActive(true);
        Target.SetActive(true);

        m_Selection = Random.Range(0, 2);
        if (m_Selection == 0)
        {
            Friendly.transform.localPosition = rechts;//friendly is groen
            Target.transform.localPosition = links;
            positionGroen = direction.rechts;
            positionRood = direction.links;
        }
        else
        {
            Friendly.transform.localPosition = links;
            Target.transform.localPosition = rechts;
            positionGroen = direction.links;
            positionRood = direction.rechts;
        }

        transform.localPosition = new Vector3(0f, 1f, 7f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);


        goalPos = Random.Range(0, 2);
        //goalPos = 0;
        if (goalPos == 0)//Groen aan
        {
            selectedKleur = kleur.groen;
            Friendly_F.SetActive(true); //Groen           
            Target_F.SetActive(false);  //Rood           
        }
        else
        {
            selectedKleur = kleur.rood;
            Friendly_F.SetActive(false);          
            Target_F.SetActive(true);           
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(Friendly.transform.localPosition);
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(shot);

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

            if (selectedKleur == kleur.groen)
                CheckGroen();

            if (selectedKleur == kleur.rood)
                CheckRood();
           
             //print(m_Selection);
            //print(goalPos);
             //print(direction);
            print("cummulative: " + GetCumulativeReward());
            EndEpisode();
        }
        else if (shoot && !shot && lookAt == direction.center)
        {
          //AddReward(-0.2f);
          //print("C");
        }
    }
    private void CheckGroen()
    {
        print("ik check groen");
        if ( positionGroen == direction.links && lookAt == direction.links)
        {
            //print("Groen");
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
        print("ik check rood");
        
        if (positionRood == direction.links && lookAt == direction.links)  //Rechts
        {
            //print("Rood");
            AddReward(1f);
        }
        else if(positionRood == direction.rechts  && lookAt == direction.rechts)
        {
            AddReward(1f);
        }
        else
        {
            //print("FOUT");
            AddReward(-1f);
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
