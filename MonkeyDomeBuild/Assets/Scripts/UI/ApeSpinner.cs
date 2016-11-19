using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ApeSpinner : MonoBehaviour
{
    public GameObject spinnerBase;
    public GameObject spinnerPointer;
    public GameObject spinnerPivot;
    public Text playerChosenText;
    public float spinnerSpeedMin;
    public float spinnerSpeedMax;
    public float spinnerDecay;
    public float spinnerResultTime;

    Transform pivot;
    private float angle;
    private float spinnerSpeed;
    private int playerChosen;
    public bool setGorilla = false;
    //private RecordKeeper rk_keeper;

    // Use this for initialization
    void Start()
    {
        pivot = spinnerPivot.GetComponent<Transform>();
        spinnerSpeed = Random.Range(spinnerSpeedMin, spinnerSpeedMax);

        //rk_keeper = FindObjectOfType<RecordKeeper>().GetComponent<RecordKeeper>();
        //rk_keeper.playerGorilla = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(spinnerSpeed > 0)
        {
            angle += spinnerSpeed * Time.deltaTime;
            spinnerSpeed -= spinnerDecay * Time.deltaTime;
            float current_pivot = pivot.transform.eulerAngles.z;

            if (current_pivot >= 0 && current_pivot < 360 / GameManager.Instance.TotalNumberofPlayers && GameManager.Instance.TotalNumberofPlayers >= 1)
            {
                playerChosen = 1;
            }
            else if(current_pivot >= 0 + (360 / GameManager.Instance.TotalNumberofPlayers) && current_pivot < (360 / GameManager.Instance.TotalNumberofPlayers)*2 && GameManager.Instance.TotalNumberofPlayers >= 2)
            {
                playerChosen = 2;
            }
            else if (current_pivot >= 0 + ((360 / GameManager.Instance.TotalNumberofPlayers) * 2) && current_pivot < (360 / GameManager.Instance.TotalNumberofPlayers) * 3 && GameManager.Instance.TotalNumberofPlayers >= 3)
            {
                playerChosen = 3;
            }
            else if (current_pivot >= 0 + ((360 / GameManager.Instance.TotalNumberofPlayers) * 3) && current_pivot < (360 / GameManager.Instance.TotalNumberofPlayers) * 4 && GameManager.Instance.TotalNumberofPlayers >= 4)
            {
                playerChosen = 4;
            }
            else if (current_pivot >= 0 + ((360 / GameManager.Instance.TotalNumberofPlayers) * 4) && current_pivot < (360 / GameManager.Instance.TotalNumberofPlayers) * 5 && GameManager.Instance.TotalNumberofPlayers >= 5)
            {
                playerChosen = 5;
            }
            playerChosenText.text = "PLAYER: " + playerChosen.ToString();

            pivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        }
        else if(spinnerSpeed <= 0 && !setGorilla)
        {
            FindObjectOfType<PreGameTimer>().GetComponent<PreGameTimer>().gorillaSet = true;
            setGorilla = true;
            spinnerSpeed = 0;

            GameManager.Instance.gmRecordKeeper.playerGorilla = playerChosen-1;
            GameManager.Instance.gmPlayers[playerChosen - 1].GetComponent<Actor>().characterType.Mutate();
        }

        if (setGorilla)
        {
            spinnerResultTime -= Time.deltaTime;

            if(spinnerResultTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
