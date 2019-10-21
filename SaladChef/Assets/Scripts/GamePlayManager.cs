using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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


    // Start is called before the first frame update
    void Start()
    {
        players = GetComponentsInChildren<PlayerController>();
        customers = GetComponentsInChildren<Customer>();
        InitGame();

    }

    public void InitGame()
    {
        resetButton.SetActive(false);
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
            int i = Random.Range(0, customers.Length);
            if (!customers[i].isActive)
            {
                customers[i].InitCustomer();
            }
        }
    }

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

    public void InitiatePickup()
    {
        int pickupTypeValue = Random.Range(0, pickupObjects.Length);
        int pickupPosValue = Random.Range(0, pickupTrans.Length);
        Pickups pickups = Instantiate(pickupObjects[pickupTypeValue]).GetComponent<Pickups>();
        pickups.transform.SetParent(pickupObjParent);
        pickups.InitPickup(pickupTrans[pickupPosValue].position);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);

    }


}
