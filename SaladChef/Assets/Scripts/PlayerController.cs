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


}
