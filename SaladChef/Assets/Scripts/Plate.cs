using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public VegitableType currVegType;
    public bool isEmpty = true;
    // Start is called before the first frame update

    public void OnPlate(VegitableType veg)
    {
        if (isEmpty)
        {
            currVegType = veg;
            isEmpty = false;
        }
        else
            return;
    }

    public VegitableType CollectFromPlate()
    {
        if (!isEmpty)
            return currVegType;
        else
            return 0;
    }
}
