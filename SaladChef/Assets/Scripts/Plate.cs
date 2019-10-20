using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plate : MonoBehaviour
{
    public VegitableType currVegType;
    public bool isEmpty = true;
    public Text plateText;
    // Start is called before the first frame update

    public void OnPlate(VegitableType veg)
    {
        if (isEmpty)
        {
            currVegType = veg;
            plateText.text = veg.ToString();
            isEmpty = false;
        }
        else
            return;
    }

    public VegitableType CollectFromPlate()
    {
        plateText.text = "";

        if (!isEmpty)
            return currVegType;
        else
            return 0;
    }
}
