using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoard : MonoBehaviour
{

    public bool isChopping=false;
    public float choppingTime;
    public VegitableType currVegie;
    public List<VegitableType> choppedVegitables;
    public PlayerController currentPlayer;
    // Start is called before the first frame update
    void Start()
    {
        isChopping=false;
    }

   public void ChopVegitable(VegitableType vegType,PlayerController curPlayer)
   {
       if(isChopping)
       return;
       currentPlayer=curPlayer;
       currentPlayer.isPlayerIdle=true;
       currVegie=vegType;
       isChopping=true;
       StartCoroutine(WaitForChoppingComplete(choppingTime));

   }

    IEnumerator WaitForChoppingComplete(float waitTime)
    {
        while(waitTime>0)
        {
            waitTime-=1;
            Debug.Log("Chopping"+waitTime);
            //show a chopping progress bar on board
            yield return new WaitForSeconds(1);

        }
        Debug.Log("Chopping Complete");
        isChopping=false;
        choppedVegitables.Add(currVegie);
        currentPlayer.isPlayerIdle=false;
        
    }

    public List<VegitableType> CollectChoppedVegitables()
    {
        return choppedVegitables;
        //empty chopped vegitables from board
    }
}
