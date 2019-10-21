using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public List<VegitableType> saladRecipe;
    public float customerWaitingTime;
    public float custWaitingTimePerItem=10;
    public bool isServed;
    public bool isActive;
    private Coroutine timerCoroutine;
    private SpriteRenderer renderer1;
    private BoxCollider2D collider1;

    public Text saladRecipeText;
    public Image progressBarBase;
    public Image progressBar;
    private float timeMultiplier=1;

    void Start()
    {
        renderer1=GetComponent<SpriteRenderer>();
        collider1=GetComponent<BoxCollider2D>();
        renderer1.enabled=false;
        collider1.enabled=false;
        progressBarBase.enabled=false;
        progressBar.enabled=false;
        saladRecipeText.text="";
    }
    public void InitCustomer()
    {
        //enable renderer
        //start timer
        renderer1.enabled=true;
        collider1.enabled=true;
        timeMultiplier=1;
        isActive = true;
        isServed = false;
        SetSaladRecipe();
        progressBarBase.enabled=true;
        progressBar.enabled=true;
        timerCoroutine=StartCoroutine(StartTimer());

    }

    IEnumerator StartTimer()
    {
        float waitingTime=customerWaitingTime;
        while(waitingTime>0)
        {
            waitingTime-=Time.deltaTime*timeMultiplier;
            progressBar.fillAmount=1-(waitingTime/customerWaitingTime);
            yield return new WaitForFixedUpdate();
        }
        isServed = false;
        isActive = false;
        renderer1.enabled=false;
        collider1.enabled=false;
        progressBarBase.enabled=false;
        progressBar.enabled=false;
        saladRecipeText.text="";
        Debug.Log("Failed To  Serve");
        foreach(PlayerController player in GamePlayManager.Instance.players)
        {
            player.AddScore(-5);
        }
    }

    public void CustomerServedSuccessfully()
    {
        isServed = true;
        isActive = false;
        renderer1.enabled=false;
        collider1.enabled=false;
        progressBarBase.enabled=false;
        progressBar.enabled=false;
        saladRecipeText.text="";
        Debug.Log("Success");
        StopCoroutine(timerCoroutine);
        //disable renderer
    }

    //Random Salad Reciepe
    void SetSaladRecipe()
    {
        //for time being max ingrediants is set to 3
        int numIngrediants = Random.Range(1, 4);
        customerWaitingTime=custWaitingTimePerItem*numIngrediants;
        Debug.Log("num ingrediants:" + numIngrediants);
        saladRecipe.Clear();
        //set first random item
        saladRecipe.Add((VegitableType)Random.Range(1, 7));
        //set other random vegitables to recipe
        for (int i = 1; i < numIngrediants; i++)
        {
            int newItem = Random.Range(1, 7);
            check:
            for (int j = 0; j < saladRecipe.Count; j++)
            {
                if (newItem == (int)saladRecipe[j])
                {
                    newItem=Random.Range(1,7);
                    goto check;
                }
            }
            saladRecipe.Add((VegitableType)newItem);

        }

        for(int i=0;i<numIngrediants;i++)
            saladRecipeText.text=saladRecipeText.text+" "+saladRecipe[i].ToString();
    }

    public void DecreaseWaitingTimeFaster()
    {
        //Angry Customer
        timeMultiplier=1.5f;
    }

}
