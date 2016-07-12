using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{

    public GameObject ball;
    public float smoothing = 3.0f;

    [SerializeField]
    private float buffer;
    [SerializeField]
    private float offsetUp;

    private Vector3 offset;
    private float maxXDistance;
    private float maxYDistance;
    private float panningY;
    private Vector3 panningScale = Vector3.zero;
    private Vector3 positionSum = Vector3.zero;
    private Vector3 meanPosition = Vector3.zero;
    private Camera myCam;
    private float camSize;


    // Use this for initialization
    void Start()
    {
        myCam = GetComponent<Camera>();
        //CamSize = myCam.orthographicSize;
        MeanOfPositions();

        offset = transform.position - meanPosition;

        transform.position = meanPosition + offset;

    }

    void LateUpdate()
    {


        SetCamera();

    }

    // find mean of positions
    void MeanOfPositions()
    {

        positionSum = Vector3.zero;

        for (int i = 0; i < GameManager.Instance.gmPlayers.Capacity; i++)
        {
            positionSum += GameManager.Instance.gmPlayers[i].transform.position;
        }
        positionSum += ball.transform.position;
        meanPosition = positionSum / (GameManager.Instance.gmPlayers.Capacity + 1);

    }

    void FindPanning()
    {
        MeanOfPositions();
        camSize = myCam.orthographicSize;
        maxXDistance = 0f;
        maxYDistance = 0f;
        // get maxXDistance
        for (int i = 0; i < GameManager.Instance.gmPlayers.Capacity; i++)
        {
            // not including ball yet, fix to grab x, not vector3
            /*
            if(maxXDistance < Vector3.Distance(meanPosition, GameManager.Instance.gmPlayers[i].transform.position))
            {
                maxXDistance = Vector3.Distance(meanPosition, GameManager.Instance.gmPlayers[i].transform.position);
            } */

            if (maxXDistance < Mathf.Abs(meanPosition.x - GameManager.Instance.gmPlayers[i].transform.position.x))
            {
                maxXDistance = Mathf.Abs(meanPosition.x - GameManager.Instance.gmPlayers[i].transform.position.x);
            }
            if (maxYDistance < Mathf.Abs(meanPosition.y - GameManager.Instance.gmPlayers[i].transform.position.y))
            {
                maxYDistance = Mathf.Abs(meanPosition.y - GameManager.Instance.gmPlayers[i].transform.position.y);
            }
            if (maxXDistance < Mathf.Abs(meanPosition.x - ball.transform.position.x))
            {
                maxXDistance = Mathf.Abs(meanPosition.x - ball.transform.position.x);
            }
            if (maxYDistance < Mathf.Abs(meanPosition.y - ball.transform.position.y))
            {
                maxYDistance = Mathf.Abs(meanPosition.y - ball.transform.position.y);
            }
        }



        //16:9ify the x into a y
        maxXDistance = maxXDistance * (myCam.aspect/2);
        if(maxXDistance > maxYDistance)
        {
            panningY = maxXDistance + buffer;
        }
        else
        {
            panningY = maxYDistance + buffer;
        }

        //panningScale.Set(maxXDistance, panningY, 0.0f);

    }

    void SetCamera()
    {
        
        Vector3 currentPos = transform.position;
        meanPosition.y += offsetUp;

        Vector3 myLerp = Vector3.Lerp(currentPos, meanPosition, (Time.deltaTime*smoothing));

        myLerp.z = currentPos.z;
        transform.position = myLerp;
        FindPanning();
        myCam.orthographicSize = Mathf.Lerp(camSize, panningY, (Time.deltaTime*smoothing));
        
        //myCam.orthographicSize = panningY;


    }

}
