using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Google.Protobuf.WellKnownTypes;

[RequireComponent(typeof(Rigidbody))]
public class JumpAgent : Agent
{
    public GameObject obstacle;
    public GameObject bonus;

    Rigidbody rb;
    bool onGround = true;
    private float randomSpeed;
    private RaycastHit hit;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        // Beweeg het blokje vooruit langs de X-as
        obstacle.transform.Translate(Vector3.right * randomSpeed * Time.deltaTime);
        bonus.transform.Translate(Vector3.right * randomSpeed * 1.1f * Time.deltaTime);
    }


    public override void OnEpisodeBegin()
    {
        //reset obstacle naar begin positie
        randomSpeed = Random.Range(3f, 7f);
        bonus.SetActive(true);
        obstacle.transform.localPosition = new Vector3(0, 0.5f, 5f); // reset obstacle 
        bonus.transform.localPosition = new Vector3(0, 5f, 5f); // reset bonus 
        this.gameObject.transform.localPosition = new Vector3(0f, 0.5f, -4f); // reset agent
    }


    public override void CollectObservations(VectorSensor sensor)
    {    // Agent positie    
        sensor.AddObservation(this.transform.localPosition.y);
        //sensor.AddObservation(obstacle.transform.localPosition.x);
        sensor.AddObservation(randomSpeed);
    }


    public float jumpForce = 30f;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {    // Acties, size = 2;


        bool shoot = actionBuffers.DiscreteActions[0] == 1;
        print(actionBuffers.DiscreteActions[0]);

        if (shoot) // if jump button is pressed and is on the ground
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            if (Physics.Raycast(ray, out hit, 100f))
            {
                // Check if the raycast hit an object with the specified tag
                if (hit.collider.CompareTag("Target"))
                {
                    // Do something if the raycast hit the target object
                    Debug.Log("Raycast hit target object!");
                    SetReward(1);

                }
            }

            EndEpisode();
        }

    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var DiscreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetMouseButtonDown(0))
        {
            DiscreteActionsOut[0] = 1;
        } else
        {
            DiscreteActionsOut[0] = 0;
        }


        
    }


    }
}
