using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour {

    public float resetTime = 1;
    public float alarm;
    public bool inAlarm = false;
    //private List<GameObject> childList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        /*
        foreach (Transform t in transform)
        {
            childList.Add(gameObject);
        }
        Debug.Log("childlist: " + childList.Count);
        */
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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

            ScoringManager.targetsHit++;
        }
    }

}
