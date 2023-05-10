using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Google.Protobuf.WellKnownTypes;


/*[RequireComponent(typeof(Rigidbody))]*/
public class ShooterAgent : Agent
{
    public GameObject Target;
    public GameObject Friendly;

    public Transform MLAgentStartPosition;

    private float randomSpeed;
    private RaycastHit hit;
    public float rotationMultiplier = 2f;


    public shoot shootScript;


    public List<Vector3> positions = new List<Vector3>();




    public override void OnEpisodeBegin()
    {

        this.gameObject.transform.localPosition = MLAgentStartPosition.localPosition;
        this.gameObject.transform.localRotation = MLAgentStartPosition.localRotation;
        Vector3 randomPosition = positions[Random.Range(0, positions.Count)]; // Zet target op een random positie
        Target.transform.localPosition = randomPosition;


    }


    public override void CollectObservations(VectorSensor sensor)
    {    // Agent positie    
        sensor.AddObservation(this.transform.rotation.y);


    }


    public override void OnActionReceived(ActionBuffers actionBuffers)
    {    // Acties, size = 2;


        bool shoot = actionBuffers.DiscreteActions[0] == 1;
        this.transform.Rotate(0.0f, rotationMultiplier * actionBuffers.ContinuousActions[0], 0.0f);
        /*        print(this.transform.rotation);
                print(actionBuffers.DiscreteActions[0]);
        */
        if (shoot) // if jump button is pressed and is on the ground
        {
            print("Pang");
            /*            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                        if (Physics.Raycast(ray, out hit, 100f))
                        {
                            // Check if the raycast hit an object with the specified tag
                            if (hit.collider.CompareTag("Target"))
                            {
                                // Do something if the raycast hit the target object
                                Debug.Log("Raycast hit target object!");
                                SetReward(1);

                            }
                        }*/

            if (shootScript != null)
            {
                string tag = shootScript.ShootGun();
                if (tag == "Target")
                {
                    print("Target hit");
                    SetReward(1);
                } else
                {
                    SetReward(-0.1f);
                }
            }

            EndEpisode();
        }

    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var DiscreteActionsOut = actionsOut.DiscreteActions;
        var continuousActionsOut = actionsOut.ContinuousActions;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            DiscreteActionsOut[0] = 1;
            print("shoot");
        }
        else
        {
            DiscreteActionsOut[0] = 0;

        }


        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        print(continuousActionsOut[0]);



    }


    }

