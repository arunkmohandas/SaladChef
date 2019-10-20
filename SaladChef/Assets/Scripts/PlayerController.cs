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
    public List<VegitableType> choppedVegitables;
    public bool isPlayerIdle;


    // Start is called before the first frame update
    void Start()
    {

        InitPlayer();
    }

    void InitPlayer()
    {
        isPlayerActive = true;
        isPlayerIdle = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPlayerActive || isPlayerIdle)
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
        if (col.gameObject.GetComponent<Vegitable>())
        {
            Vegitable _vegitable = col.gameObject.GetComponent<Vegitable>();
            if (collectedVegies.Count >= 2)
                return;
            foreach (VegitableType veg in collectedVegies)
            {
                if (veg == _vegitable.vegitableType)
                    return;
            }
            collectedVegies.Add(_vegitable.vegitableType);
        }


        if (col.gameObject.GetComponent<ChoppingBoard>())
        {
            ChoppingBoard _choppingBoard = col.gameObject.GetComponent<ChoppingBoard>();
            if (collectedVegies.Count > 0)
            {
                //Chop Vegitables
                _choppingBoard.ChopVegitable(collectedVegies[0], this);
                collectedVegies.RemoveAt(0);
                transform.position += new Vector3(0, 2, 0);

            }
            else
            {
                //collect Chopped Vegitables
                choppedVegitables.Clear();
                choppedVegitables = new List<VegitableType>(_choppingBoard.CollectChoppedVegitables());
                _choppingBoard.choppedVegitables.Clear();


            }
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

        if (col.gameObject.GetComponent<Customer>())
        {
            Customer _customer = col.gameObject.GetComponent<Customer>();
            if (choppedVegitables.Count == _customer.saladRecipe.Count)
            {
                for (int i = 0; i < choppedVegitables.Count; i++)
                {
                    if (choppedVegitables[i] != _customer.saladRecipe[i])
                    {
                        goto failed;
                    }
                }
                _customer.CustomerServedSuccessfully();
                return;
            }
            else
                goto failed;
            failed:
            Debug.Log("FAILED");

        }

    }


}
