using UnityEngine;
using System.Collections;

public class PointEffect : MonoBehaviour {

    private bool isDisplaying;
    private Vector2 startOffset;
    private Transform startLocation;
    public Transform flyToMe;
    public float flyTime;

    private bool canBeUsed;
	// Use this for initialization
	void Start () {
        isDisplaying = true;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (isDisplaying)
        {
            Vector3.Lerp(transform.position, flyToMe.position, Time.deltaTime * flyTime);
            if(transform.position == flyToMe.position)
            {
                isDisplaying = !isDisplaying;
            }
        }
	}
}
