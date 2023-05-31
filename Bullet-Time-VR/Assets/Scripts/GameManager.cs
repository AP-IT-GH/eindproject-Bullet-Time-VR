using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;



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


    // UI 
    public GameObject wonUI;
    public GameObject lostUI;
    public GameObject bothLostUI;



    // Countdown things
    public TextMeshProUGUI countDownUIText;
    public float countdownDuration = 3f; // Duration of the countdown in seconds
    private float countdownTimer; // Timer for the countdown


    private string who_won = null;



    // Start is called before the first frame update
    void Start()
    {
        Agent.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        CheckWhoWon();
        CountDown();
    }

    public void StartCountdown()
    {
        countdownTimer = countdownDuration;
    }



    private void CountDown() {

        if (countdownTimer > 0f)
        {
            countdownTimer -= Time.deltaTime;
            print(countdownTimer);
            countDownUIText.text = (countdownTimer).ToString("0"); ;

            if (countdownTimer <= 0f)
            {
                countDownUIText.text = "";
                StartGame();
            }
        }
    }

    public void StartGame()
    {
        gameActive = true;
        Agent.SetActive(true);

    }

    public void ResetGame()
    {


        // Plaats Enemy en Friendly
        PlaceTargetAndFriendy();

        // Rotate Player and ML to start position
        VRPlayer.transform.localRotation = new Quaternion(0, 180, 0, 0);
        Agent.transform.localRotation = new Quaternion(0, 0, 0, 0);

        // Reset ML Agent
        AgentScript.Reset();

        // Reset who won
        who_won = null;

        // Reset which tag they last hit
        shootScriptPlayer.ResetHitTag();
        shootScriptAgent.ResetHitTag();

    }

    void stopGame()
    {
        gameActive = false;
    }
        


    void EndGame()
    {
        print("The winner is: " + who_won);
        gameActive = false;

        Agent.SetActive(false);
        // Freez ML Agent
    }

    void CheckWhoWon()
    {

        if (gameActive)
        {
            if (shootScriptPlayer.CheckHitTag() == currentTarget.tag) {

                who_won = "You won";
                Invoke("EndGame", 0.2f);
                wonUI.SetActive(true);
            } else if (shootScriptAgent.CheckHitTag() == currentTarget.tag)
            {
                who_won = "AI won";
                Invoke("EndGame", 0.2f);
                lostUI.SetActive(true);

            } else if (shootScriptPlayer.CheckHitTag() != null && shootScriptAgent.CheckHitTag() != null)
            {
                who_won = "No one";
                Invoke("EndGame", 0.2f);
                bothLostUI.SetActive(true);
            }
        }

    }


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

}
