using UnityEngine;
using System.Collections;

public class PointEffect : MonoBehaviour {

    private bool isDisplaying;
    private Vector2 startOffset;
    private Vector3 startLocation;
    private Vector3 flyToMe;
    public float flySpeed;

    private bool canBeUsed;
	// Use this for initialization
	void Start () {
 
    }
	
	// Update is called once per frame
	void Update () {
        if (isDisplaying)
        {
            Debug.Log("point display");
            transform.localPosition = Vector3.Lerp(transform.localPosition, flyToMe, Time.deltaTime * flySpeed);
            if (Vector3.Distance(transform.localPosition, flyToMe) < .1f)
            {
                isDisplaying = false;
                transform.localPosition = startLocation;
                gameObject.SetActive(false);
            }
        }
	}

    public void Init()
    {
        isDisplaying = true;
        startLocation = new Vector3(transform.localPosition.x,transform.localPosition.y+3f, transform.localPosition.z  );
        flyToMe = startLocation + Vector3.up * 3f;
    }
    public bool IsDisplaying
    {
        set
        {
            isDisplaying = value;
        }
    }
}
