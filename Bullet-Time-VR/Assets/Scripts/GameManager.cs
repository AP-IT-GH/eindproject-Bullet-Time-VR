using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{



    public List<Vector3> spawnPositions = new List<Vector3>(); // Lijst van mogelijke spawn posities
    public List<GameObject> people = new List<GameObject>(); // Lijst van allemaal mensen die kunnen spelen
    public GameObject currentTarget; // Is elke keer een andere target

    public GameObject VRPlayer; // VR Player Gun
    public shoot shootScriptPlayer;
    public shoot shootScriptAgent;

    public GameObject Agent;
    public FinaalCameraAgent AgentScript;
    private bool gameActive = false;
    


    // Start is called before the first frame update
    void Start()
    {
        Agent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckWhoWon();
    }

    public void StartGame()
    {

        //ChooseTarget();
        PlaceTargetAndFriendy();
        //SpawnCopyOfTarget();
        ResetGame(); // Reset rotation




        gameActive = true;
        Agent.SetActive(true);





    }

    void ResetGame()
    {

        // Rotate Player and ML to start position
        VRPlayer.transform.localRotation = new Quaternion(0, 180, 0, 0);
        Agent.transform.localRotation = new Quaternion(0, 0, 0, 0);
        AgentScript.Reset();



        // Reset which tag they last hit
        shootScriptPlayer.ResetHitTag();
        shootScriptAgent.ResetHitTag();



    }

    void stopGame()
    {
        gameActive = false;
    }
        


    void EndGame(string winner = null)
    {


        print("The winner is: " + winner);
        gameActive = false;

        Agent.SetActive(false);
        // Freez ML Agent


    }

    void CheckWhoWon()
    {

        if (gameActive)
        {
            if (shootScriptPlayer.CheckHitTag() == currentTarget.tag) {
                EndGame("You win");


            } else if (shootScriptAgent.CheckHitTag() == currentTarget.tag)
            {
                EndGame("AI Won");

            } else if (shootScriptPlayer.CheckHitTag() != null && shootScriptAgent.CheckHitTag() != null)
            {
                EndGame("No one");
            }
        }

    }


    //private void ChooseTarget()
    //{

    //    GameObject currentTarget = people[Random.Range(0, people.Count)]; // Kiest een random target, van de lijst van personen

    //}



    private void PlaceTargetAndFriendy() // Spawn alle personen (+ target) op een random positie (die in de spawn points zitten)
    {

        List<Vector3> randomSpawnPositions = new List<Vector3>(spawnPositions); // Create a copy of the spawnPositions list


        int n = randomSpawnPositions.Count;
        while (n > 1)  // Shuffle the randomSpawnPositions list using Fisher-Yates shuffle algorithm
        {
            n--;
            int k = Random.Range(0, n + 1);
            Vector3 value = randomSpawnPositions[k];
            randomSpawnPositions[k] = randomSpawnPositions[n];
            randomSpawnPositions[n] = value;
        }


        for (int i = 0; i < people.Count; i++) // Give the people the random spawn position
        {

            people[i].transform.localPosition = randomSpawnPositions[i];

        }

    }


    //private void SpawnCopyOfTarget()
    //{
    //    currentTarget.transform.localPosition = TargetExamplePosition;
    //}
}
