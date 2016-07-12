using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

    public List<GameObject> players;
    public GameObject ball;
    public float smoothing = 3.0f;

    private Vector3 offset;
    private float maxXDistance;
    private float panningY;
    private Vector3 panningScale = Vector3.zero;
    private Vector3 positionSum = Vector3.zero;
    private Vector3 meanPosition = Vector3.zero;
    private Camera myCam;
    private float camSize;


    // Use this for initialization
    void Start () { 
        myCam = GetComponent<Camera>();
        //CamSize = myCam.orthographicSize;
        MeanOfPositions();

        offset = transform.position - meanPosition;

        transform.position = meanPosition + offset;

    }
	
	void LateUpdate () {


        SetCamera();

    }

    // find mean of positions
    void MeanOfPositions()
    {

        positionSum = Vector3.zero;

        for(int i = 0;i < players.Capacity; i++)
        {
            positionSum += players[i].transform.position;
        }
        positionSum += ball.transform.position;
        meanPosition = positionSum / (players.Capacity + 1);

    }

    void FindPanning()
    {
        MeanOfPositions();
        camSize = myCam.orthographicSize;
        maxXDistance = 0f;
        // get maxXDistance
        for (int i = 0; i < players.Capacity; i++)
        {
            // not including ball yet, fix to grab x, not vector3
            if(maxXDistance < Vector3.Distance(meanPosition,players[i].transform.position))
            {
                maxXDistance = Vector3.Distance(meanPosition, players[i].transform.position);
            } 
        }

        //16:9ify the x into a y
        panningY = maxXDistance * myCam.aspect;

        //panningScale.Set(maxXDistance, panningY, 0.0f);

    }

    void SetCamera()
    {
        FindPanning();
        Vector3 currentPos = transform.position;

        Vector3 myLerp = Vector3.Lerp(currentPos, meanPosition, smoothing * Time.deltaTime);

        myLerp.z = currentPos.z;

        transform.position = myLerp;
        // myCam.orthographicSize = Mathf.Lerp(camSize, panningY, Time.deltaTime);
        myCam.orthographicSize = panningY;
        

    }

}
