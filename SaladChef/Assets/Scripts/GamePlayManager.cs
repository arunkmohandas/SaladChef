using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;


public class GamePlayManager : MonoBehaviour
{
    private static GamePlayManager instance = null;

    private GamePlayManager()
    {
        instance = this;
    }

    public static GamePlayManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GamePlayManager();
            }
            return instance;
        }
    }

    [HideInInspector]
    public PlayerController[] players;
    private Customer[] customers;
    public int customerDelay;
    public bool isGameRunning;

    public Transform[] pickupTrans;
    public float pickupInterval = 30;
    public GameObject[] pickupObjects;
    public Transform pickupObjParent;
    public GameObject resetButton;
    public GameObject highScorePopup;
    public Text highscoreText;
    public int[] highScores;
    // public string[] highScoredPlayers;


    // Start is called before the first frame update
    void Start()
    {
        players = GetComponentsInChildren<PlayerController>();
        customers = GetComponentsInChildren<Customer>();
        highScorePopup.SetActive(false);
        resetButton.SetActive(false);
        GetHighScores();


        InitGame();

    }

    public void InitGame()
    {
        StartCoroutine(InitializeNewCustomer());
        for (int i = 0; i < players.Length; i++)
            players[i].InitPlayer();
        isGameRunning = true;
        // StartCoroutine(PickUpManager());

    }

    void Update()
    {
        if (!isGameRunning)
            return;
        if (!players[0].isPlayerActive && !players[1].isPlayerActive)
        {
            Debug.Log("Game Over");
            if (players[0].score > players[1].score)
                players[0].ShowResult(true);
            else
                players[1].ShowResult(true);
            isGameRunning = false;
            resetButton.SetActive(true);
        }


        //try to initialize customer if number of active customers is less than 2
        int numActiveCustomers = 0;
        for (int i = 0; i < customers.Length; i++)
        {
            if (customers[i].isActive)
                numActiveCustomers++;
        }
        if (numActiveCustomers < 2)
        {
            int i = UnityEngine.Random.Range(0, customers.Length);
            if (!customers[i].isActive)
            {
                customers[i].InitCustomer();
            }
        }
    }

    //Initialize new customer on a time interval
    IEnumerator InitializeNewCustomer()
    {

        while (isGameRunning)
        {
            yield return new WaitForSeconds(customerDelay);

            //try to initialize customers based on delay
            for (int i = 0; i < customers.Length; i++)
            {
                if (!customers[i].isActive)
                {

                    customers[i].InitCustomer();
                    break;
                }
            }

        }
    }


    // IEnumerator PickUpManager()
    // {
    //     while(isGameRunning)
    //     {
    //         yield return new WaitForSeconds(pickupInterval);


    //     }
    // }

    //Enable Pickup Object
    public void InitiatePickup()
    {
        int pickupTypeValue = UnityEngine.Random.Range(0, pickupObjects.Length);
        int pickupPosValue = UnityEngine.Random.Range(0, pickupTrans.Length);
        Pickups pickups = Instantiate(pickupObjects[pickupTypeValue]).GetComponent<Pickups>();
        pickups.transform.SetParent(pickupObjParent);
        pickups.InitPickup(pickupTrans[pickupPosValue].position);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);

    }

    void GetHighScores()
    {
        highScores = PlayerPrefsX.GetIntArray("Highscores", 0, 10);
    }

    void SetHighScores()
    {

        Array.Sort(highScores);
        Array.Reverse(highScores);
        PlayerPrefsX.SetIntArray("Highscores", highScores);
    }

    //Sort And Save High Score
    public void PostToHighScore(int score)
    {
        for (int i = 0; i < highScores.Length; i++)
        {
            if (score > highScores[i])
            {
                for (int j = i; j < highScores.Length - 1; j++)
                {
                    highScores[j + 1] = highScores[j];
                }
                highScores[i] = score;
                break;
            }
        }
        SetHighScores();
        highScorePopup.SetActive(true);
        highscoreText.text="";
        for (int i = 0; i < highScores.Length; i++)
        {
            if (highScores[i] > 0)
            {
                highscoreText.text =  highscoreText.text+(i + 1).ToString() + ". " + highScores[i] + "\n";
            }
            else
                break;
        }
    }


}
