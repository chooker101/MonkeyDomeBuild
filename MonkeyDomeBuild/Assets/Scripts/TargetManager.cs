using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour {

    public float resetTime = 1;
    public float alarm;
    public bool inAlarm = false;

    private BallInfo ballInfo;
    private int addScore;

    private int targetTier;
    private bool[] targetsHitInSequence = new bool[5];

    private GameObject[] largeTargets;

    /*private Vector3[] targetLocations = new[] { new Vector3(16f, 1f, 0f), new Vector3(-13f, 1f, 0f), new Vector3(-18f, 6f, 0f),
        new Vector3(-18f, 16f, 0f),new Vector3 (9f, 18f, 0f),new Vector3 (18f, 6f, 0f), new Vector3(18f, 16f, 0f)};
    private Vector3[] targetRotations = new[] {new Vector3(0f,0f,0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -90f),
        new Vector3(0f, 0f, -90f), new Vector3(0f, 0f, -180f),new Vector3(0f, 0f, 90f),new Vector3(0f, 0f, 90f)};*/

    //private List<GameObject> childList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        targetTier = 0;

        largeTargets = GameObject.FindGameObjectsWithTag("Large");

        /*
        foreach (Transform t in transform)
        {
            childList.Add(gameObject);
        }
        Debug.Log("childlist: " + childList.Count);
        */
        ballInfo = GetComponent<BallInfo>();

        for (int i = 0; i < targetsHitInSequence.Length; i++)
        {
            targetsHitInSequence[i] = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (this.gameObject.activeSelf == false && !inAlarm)
        {
            alarm = resetTime;
            inAlarm = true;
            Debug.Log("Alarm set");
        }
        else if (inAlarm && alarm > 0)
        {
            alarm -= Time.deltaTime;
            Debug.Log("Alarm counting down");
        }

        if (alarm <= 0 && this.gameObject.activeSelf == false && inAlarm)
        {
            alarm = 0;
            inAlarm = false;
            this.gameObject.SetActive(true);
            Debug.Log("Alarm disabled");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Something(largeTargets, false);
        }
    }
    void Something(GameObject[] tars,bool b)
    {
        foreach (GameObject g in tars)
        {
            g.SetActive(b);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            this.gameObject.SetActive(false);

            //this.gameObject.GetComponent<SphereCollider>().enabled = false;
            /*
            for (int i = 0; i <= childList.Count; i++)
            {
                childList[i].GetComponent<SphereCollider>().enabled = false;
            }
            */

            //////////////// NOTES //////////////////
           // - need to account for target tier   //
          // - need to connect ballInfo and the- //
         //  Actor class                        //

            //ballInfo.GetLastThrowMonkey();

            //addScore = ScoringManager.TargetScore(0, 0);

            ScoringManager.targetsHit++;
        }
    }



}
