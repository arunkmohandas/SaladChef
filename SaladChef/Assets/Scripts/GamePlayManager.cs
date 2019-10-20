using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{

    public static GamePlayManager instance = null;
     
     // Game Instance Singleton
     public static GamePlayManager Instance
     {
         get
         { 
             return instance; 
         }
     }

[HideInInspector]
    public PlayerController[] players;
    private Customer[] customers;
    public int customerDelay;
    public bool isGameRunning;

    public Transform[] pickupTrans;
    public float pickupInterval=30;
    public GameObject[] pickupObjects;
    public Transform pickupObjParent;


    // Start is called before the first frame update
    void Start()
    {
        players = GetComponentsInChildren<PlayerController>();
        customers = GetComponentsInChildren<Customer>();
        InitGame();

    }

    public void InitGame()
    {
        StartCoroutine(InitializeNewCustomer());
        for(int i=0;i<players.Length;i++)
        players[i].InitPlayer();
        isGameRunning = true;
        StartCoroutine(PickUpManager());
        
    }

   void Update()
   {
       if(!isGameRunning)
       return;
       if(!players[0].isPlayerActive&&!players[1].isPlayerActive)
       {
           Debug.Log("Game Over");
          

       }
   }

    IEnumerator InitializeNewCustomer()
    {
         for (int i = 0; i < 2; i++)
         {
             customers[i].InitCustomer();
         }
        while (isGameRunning)
        {
            yield return new WaitForSeconds(customerDelay);

            for (int i = 0; i < customers.Length; i++)
            {
                if (customers[i].isActive)
                {
                    continue;
                }
                else
                {
                    customers[i].InitCustomer();
                    break;
                }
            }

        }
    }


IEnumerator PickUpManager()
{
    while(isGameRunning)
    {
        yield return new WaitForSeconds(pickupInterval);
        int pickupTypeValue=Random.Range(0,pickupObjects.Length);
        int pickupPosValue=Random.Range(0,pickupTrans.Length);
        Pickups pickups=Instantiate(pickupObjects[pickupTypeValue]).GetComponent<Pickups>();
        pickups.transform.SetParent(pickupObjParent);
        pickups.InitPickup(pickupTrans[pickupPosValue].position);

    }
}


}
