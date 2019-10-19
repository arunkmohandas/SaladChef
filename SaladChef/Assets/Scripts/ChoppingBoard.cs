using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoard : MonoBehaviour
{

    public bool isChopping=false;
    public float choppingTime;
    public VegitableType currVegie;
    // Start is called before the first frame update
    void Start()
    {
        isChopping=false;
    }

   public void ChopVegitable(VegitableType vegType)
   {
       if(isChopping)
       return;
       currVegie=vegType;
       isChopping=true;
       StartCoroutine(WaitForChoppingComplete(choppingTime));

   }

    IEnumerator WaitForChoppingComplete(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Chopping Complete");
        isChopping=false;
        
    }
}
