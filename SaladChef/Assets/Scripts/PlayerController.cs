using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Text vegCollectedText;
    public int score;
    public int playingTime;
    public Text scoreText;
    public Text timeText;
    public bool isReadyForPickup;
    private int speedPickupActiveTime = 20;
    public Text victoryText;

    // Start is called before the first frame update
    void Start()
    {

        // InitPlayer();
    }

    public void InitPlayer()
    {
        isPlayerActive = true;
        isPlayerIdle = false;
        victoryText.text="";
        StartCoroutine(UpdateTime());
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
            ShowCollectedVegText();
        }


        if (col.gameObject.GetComponent<ChoppingBoard>())
        {
            ChoppingBoard _choppingBoard = col.gameObject.GetComponent<ChoppingBoard>();
            if (collectedVegies.Count > 0)
            {
                //Chop Vegitables
                _choppingBoard.ChopVegitable(collectedVegies[0], this);
                collectedVegies.RemoveAt(0);
                transform.position += new Vector3(0, 1, 0);

                ShowCollectedVegText();

            }
            else
            {
                //collect Chopped Vegitables
                choppedVegitables.Clear();
                choppedVegitables = new List<VegitableType>(_choppingBoard.CollectChoppedVegitables());
                _choppingBoard.choppedVegitables.Clear();

                vegCollectedText.text = "";
                for (int i = 0; i < choppedVegitables.Count; i++)
                    vegCollectedText.text = vegCollectedText.text + " " + choppedVegitables[i].ToString();


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
            else if (plateVeg != 0)
            {
                foreach (VegitableType veg in collectedVegies)
                {
                    if (veg == plateVeg)
                        return;
                }
                collectedVegies.Add(plateVeg);
            }
            ShowCollectedVegText();
        }

        //Trash
        if (col.gameObject.layer == 8)
        {
            if (collectedVegies.Count > 0)
            {
                collectedVegies.RemoveAt(0);
            }
            else
            {
                if (choppedVegitables.Count > 0)
                    choppedVegitables.RemoveAt(0);
            }
            ShowCollectedVegText();
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
                AddScore(10);
                if (_customer.progressBar.fillAmount < .7f)
                {
                    isReadyForPickup = true;
                    GamePlayManager.Instance.InitiatePickup();
                }
                choppedVegitables.Clear();
                ShowCollectedVegText();
                return;
            }
            else
                goto failed;
            failed:
            _customer.DecreaseWaitingTimeFaster();
            AddScore(-5);
            choppedVegitables.Clear();
            ShowCollectedVegText();
            Debug.Log("FAILED");

        }

        if (col.gameObject.GetComponent<Pickups>() && isReadyForPickup)
        {
            Pickups pickups = col.gameObject.GetComponent<Pickups>();
            OnPickupCollect(pickups.pickupType);
            pickups.HidePickup();
        }

    }

    void ShowCollectedVegText()
    {
        vegCollectedText.text = "";
        for (int i = 0; i < collectedVegies.Count; i++)
            vegCollectedText.text = vegCollectedText.text + " " + collectedVegies[i].ToString();
    }

    IEnumerator UpdateTime()
    {
        while (isPlayerActive)
        {
            yield return new WaitForSeconds(1);
            playingTime--;
            timeText.text = "Time:" + playingTime.ToString();
            if (playingTime <= 0)
                isPlayerActive = false;
        }

    }
    public void ShowResult(bool result)
    {
        if (result)
           victoryText.text=gameObject.name+" Wins...";
    }

    void OnPickupCollect(PickupType pickupType)
    {
        if (pickupType == PickupType.Time)
        {
            playingTime += 10;
            timeText.text = "Time:" + playingTime.ToString();
        }
        else if (pickupType == PickupType.Speed)
        {
            //increase speed
            StartCoroutine(SpeedPickupCollected());
        }
        else if (pickupType == PickupType.Score)
        {
            AddScore(10);
        }
        isReadyForPickup = false;

    }

    public void AddScore(int _score)
    {
        score += _score;
        scoreText.text = "Score:" + score.ToString();

    }

    IEnumerator SpeedPickupCollected()
    {
        movingSpeed = movingSpeed * 2;
        yield return new WaitForSeconds(speedPickupActiveTime);
        movingSpeed = movingSpeed * .5f;
    }


}
