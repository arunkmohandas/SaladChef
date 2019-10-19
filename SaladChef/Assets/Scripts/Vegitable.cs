using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VegitableType
{
    A=1,B,C,D,E,F
};

public class Vegitable : MonoBehaviour
{
    public VegitableType vegitableType;

    void OnTriggerEnter2D(Collider2D col)
    {
    }
}
