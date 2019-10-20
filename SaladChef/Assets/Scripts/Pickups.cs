using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType { Speed, Time, Score };
public class Pickups : MonoBehaviour
{
    public PickupType pickupType;
    public int pickupTimer;
    private Coroutine timerCoroutine;


    void Start()
    {
        // renderingObj.enabled = false;
        // boxColliderObj.enabled = false;
    }

    public void InitPickup(Vector3 pos)
    {
        transform.position = pos;
        timerCoroutine = StartCoroutine(StartPickupTimer());
    }

    IEnumerator StartPickupTimer()
    {
        yield return new WaitForSeconds(pickupTimer);
        HidePickup();
    }

    public void HidePickup()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
        Destroy(this.gameObject);
    }
}
