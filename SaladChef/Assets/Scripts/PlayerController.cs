using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Input Controls for Multiplayer
public enum Controller
{
    Control1,
    Control2
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Controller controller;
    private float horizontalMove;
    private float verticalMove;
    public bool isPlayerActive;
    public float movingSpeed = 10;
    public float playerBoundaryXMax;
    public float playerBoundaryXMin;
    public float playerBoundaryYMax;
    public float playerBoundaryYMin;
    public List<VegitableType> collectedVegies;


    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPlayerActive)
            return;

        //Get input
        Vector3 move = new Vector3(CheckHorizontalInput(), CheckVerticalInput(), 0);
        transform.position += move * movingSpeed * Time.deltaTime;

    }

    //Check For Horizontal Input From Keyboard
    int CheckHorizontalInput()
    {

        //check for multiplayer controller
        if (controller == Controller.Control1)
        {
            if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < playerBoundaryXMax)
                return 1;
            else if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > playerBoundaryXMin)
                return -1;
            else
                return 0;
        }
        else
        {
            if (Input.GetKey(KeyCode.D) && transform.position.x < playerBoundaryXMax)
                return 1;
            else if (Input.GetKey(KeyCode.A) && transform.position.x > playerBoundaryXMin)
                return -1;
            else
                return 0;
        }
    }

    //Check For Vertical Input From Keyboard
    int CheckVerticalInput()
    {
        //check for multiplayer controller
        if (controller == Controller.Control1)
        {
            if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < playerBoundaryYMax)
                return 1;
            else if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > playerBoundaryYMin)
                return -1;
            else
                return 0;
        }
        else
        {
            if (Input.GetKey(KeyCode.W) && transform.position.y < playerBoundaryYMax)
                return 1;
            else if (Input.GetKey(KeyCode.S) && transform.position.y > playerBoundaryYMin)
                return -1;
            else
                return 0;
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Trigger Enter");
        if (col.gameObject.GetComponent<Vegitable>())
        {
            foreach (VegitableType veg in collectedVegies)
            {
                if (veg == col.gameObject.GetComponent<Vegitable>().vegitableType)
                    return;
            }
            collectedVegies.Add(col.gameObject.GetComponent<Vegitable>().vegitableType);
        }


        if (col.gameObject.GetComponent<ChoppingBoard>())
        {

        }

        //Plate Dequq one veg item if its empty else collect item from plate
        if (col.gameObject.GetComponent<Plate>())
        {
            VegitableType plateVeg = col.gameObject.GetComponent<Plate>().CollectFromPlate();
            if (plateVeg == 0 && collectedVegies.Count > 0)
            {
                col.gameObject.GetComponent<Plate>().OnPlate(collectedVegies[0]);
                collectedVegies.RemoveAt(0);
            }
            else
            {
                foreach (VegitableType veg in collectedVegies)
                {
                    if (veg == plateVeg)
                        return;
                }
                collectedVegies.Add(plateVeg);
            }
        }

        //Trash
        if (col.gameObject.layer == 8)
        {
            if (collectedVegies.Count > 0)
            {
                collectedVegies.RemoveAt(0);
            }
        }

    }


}
