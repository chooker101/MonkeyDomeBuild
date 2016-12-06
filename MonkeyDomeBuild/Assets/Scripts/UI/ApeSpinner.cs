using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ApeSpinner : MonoBehaviour
{
    public GameObject spinnerBase;
    public GameObject spinnerPointer;
    public GameObject spinnerPivot;
    public float spinnerSpeedMin;
    public float spinnerSpeedMax;
    public float spinnerDecay;
    public float spinnerResultTime;
    public bool setGorilla = false;
    public List<Image> p_Colour;
    public List<Text> p_Text;

    Transform pivot;
    private float angle;
    private float spinnerSpeed;
    private int playerChosen;
    private float playerAngle;
    //private RecordKeeper rk_keeper;

    // Use this for initialization
    void Start()
    {
        pivot = spinnerPivot.GetComponent<Transform>();
        spinnerSpeed = Random.Range(spinnerSpeedMin, spinnerSpeedMax);
        
        playerAngle = 360 / GameManager.Instance.TotalNumberofActors;

        for(int i = 0; i < p_Colour.Count; i++)
        {
            p_Colour[i].transform.localRotation = Quaternion.AngleAxis(playerAngle * i, Vector3.back);
            p_Colour[i].fillAmount = playerAngle / 360;
            p_Text[i].transform.localRotation = Quaternion.AngleAxis((playerAngle * i) + (playerAngle / 2), Vector3.back);

            if(GameManager.Instance.TotalNumberofActors > i)
            {
                p_Colour[i].gameObject.SetActive(true);
                p_Colour[i].color = GameManager.Instance.gmRecordKeeper.GetPlayerColour(i);
                p_Text[i].gameObject.SetActive(true);
            }
            else
            {
                p_Colour[i].gameObject.SetActive(false);
                p_Text[i].gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(spinnerSpeed > 0)
        {
            angle += spinnerSpeed * Time.deltaTime;
            spinnerSpeed -= spinnerDecay * Time.deltaTime;
            float current_pivot = pivot.transform.eulerAngles.z;

            for(int i = 0; i < GameManager.Instance.TotalNumberofActors; i++)
            {
                if (current_pivot < 360-(playerAngle*i) && current_pivot >= 360 - (playerAngle*(i+1)))
                {
                    playerChosen = i+1;
                    break;
                }
            }

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
