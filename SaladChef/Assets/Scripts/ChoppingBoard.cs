using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoppingBoard : MonoBehaviour
{

    public bool isChopping=false;
    public float choppingTime;
    public VegitableType currVegie;
    public List<VegitableType> choppedVegitables;
    public PlayerController currentPlayer;
    public Text choppingBoardText;
    public Image progressbarBase;
    public Image progressbar;

    // Start is called before the first frame update
    void Start()
    {
        isChopping=false;
        progressbar.enabled=false;
        progressbarBase.enabled=false;
        choppingBoardText.text="";
    }

   public void ChopVegitable(VegitableType vegType,PlayerController curPlayer)
   {
       if(isChopping||(int)vegType==0)
       return;
       currentPlayer=curPlayer;
       currentPlayer.isPlayerIdle=true;
       currVegie=vegType;
       isChopping=true;
       choppingBoardText.text=choppingBoardText.text+ " "+vegType.ToString();
       progressbarBase.enabled=true;
       progressbar.enabled=true;

       StartCoroutine(WaitForChoppingComplete(choppingTime));

   }

    IEnumerator WaitForChoppingComplete(float waitTime)
    {
        while(waitTime>0)
        {
            progressbar.fillAmount=1-(waitTime/choppingTime);
            waitTime-=Time.deltaTime;
            yield return new WaitForFixedUpdate();

        }
        Debug.Log("Chopping Complete");
        progressbarBase.enabled=false;
        progressbar.enabled=false;
        isChopping=false;
        choppedVegitables.Add(currVegie);
        currentPlayer.isPlayerIdle=false;
        
    }

    public List<VegitableType> CollectChoppedVegitables()
    {
        choppingBoardText.text="";
        return choppedVegitables;
        //empty chopped vegitables from board
    }
}
