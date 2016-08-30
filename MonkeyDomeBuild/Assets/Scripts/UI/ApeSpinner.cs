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

    Transform pivot;
    private float angle;
    private float spinnerSpeed;
    private int playerChosen;
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
        angle += spinnerSpeed*Time.deltaTime;
        if(spinnerSpeed > 0)
        {
            spinnerSpeed -= spinnerDecay * Time.deltaTime;

            if (pivot.transform.eulerAngles.z < 360 && pivot.transform.eulerAngles.z >= 240)
            {
                playerChosen = 2;
            }
            else if(pivot.transform.eulerAngles.z < 240 && pivot.transform.eulerAngles.z >= 120)
            {
                playerChosen = 3;
            }
            else if(pivot.transform.eulerAngles.z < 120 && pivot.transform.eulerAngles.z >= 0)
            {
                playerChosen = 1;
            }
            playerChosenText.text = "PLAYER: " + playerChosen.ToString();

            pivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
            Debug.Log("Pointer Pivot z: " + pivot.transform.eulerAngles.z.ToString());
            Debug.Log("Chosen Monkey: " + playerChosen.ToString());
        }
        else if(spinnerSpeed <= 0)
        {
            spinnerSpeed = 0;

            GameManager.Instance.gmRecordKeeper.playerGorilla = playerChosen-1;
        }
    }
}
