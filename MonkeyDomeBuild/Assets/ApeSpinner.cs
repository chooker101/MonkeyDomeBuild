using UnityEngine;
using System.Collections;

public class ApeSpinner : MonoBehaviour
{
    public GameObject spinnerBase;
    public GameObject spinnerPointer;
    public GameObject spinnerPivot;
    Transform pivot;
    float angle;
    public float spinnerSpeedMin;
    public float spinnerSpeedMax;
    public float spinnerDecay;
    float spinnerSpeed;

    // Use this for initialization
    void Start()
    {
        pivot = spinnerPivot.GetComponent<Transform>();
        spinnerSpeed = Random.Range(spinnerSpeedMin, spinnerSpeedMax);
        //spinnerSpeed = spinnerSpeedMax;
    }

    // Update is called once per frame
    void Update()
    {
        angle += spinnerSpeed*Time.deltaTime;
        if(spinnerSpeed > 0)
        {
            spinnerSpeed -= spinnerDecay * Time.deltaTime;
        }
        else if(spinnerSpeed <= 0)
        {
            spinnerSpeed = 0;
        }
        pivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
    }
}
