using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    private PlayerController[] players;
    private Customer[] customers;
    public int customerDelay;
    public bool isGameRunning;

    // Start is called before the first frame update
    void Start()
    {
        players = GetComponentsInChildren<PlayerController>();
        customers = GetComponentsInChildren<Customer>();
        InitGame();

    }

    public void InitGame()
    {
        Debug.Log("Players" + players.Length);
        Debug.Log("Customers" + customers.Length);
        isGameRunning = true;
        StartCoroutine(InitializeNewCustomer());

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



}
