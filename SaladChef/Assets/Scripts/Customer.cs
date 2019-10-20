using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    // Start is called before the first frame update
    public List<VegitableType> saladRecipe;
   public float customerWaitingTime;
   public bool isServed;
   public bool isActive;
    public  void InitCustomer()
    {
        //enable renderer
        //start timer
        isActive=true;
        isServed=false;
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(customerWaitingTime);
        isServed=false;
        isActive=false;
        Debug.Log("Failed To  Serve");
    }

    public void CustomerServedSuccessfully()
    {
        isServed=true;
        isActive=false;
        Debug.Log("Success");
        //disable renderer
    }

}
