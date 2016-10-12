using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * FIX CAMSIZE CHANGING
 * MAKE PUBLIC SHAKER METHOD
 */

public class CameraController : MonoBehaviour
{

    private Transform movePos;
    public float startShakeDur;
    private float shakeDur;
    public float shakeMag;
    public float killShake;
    public bool shaking = true;
	Vector3 startPos;

    //private GameObject ball;
    public float smoothing = 3.0f;
    public float maxCamSize = 25f;
    private float minCamSize = 18f;
    private float minCameraHeight = 12f;
    public float maxDistanceFromZero = 4f;
    private float buffer = 4f;
    private float offsetUp = 0f;

    private Vector3 offset;
    private float maxXDistance;
    private float maxYDistance;
    private float panningY;
    //private Vector3 panningScale = Vector3.zero;
    private Vector3 positionSum = Vector3.zero;
    private Vector3 meanPosition = Vector3.zero;
    private Camera myCam;
    private float camSize;

    public bool considerTargets = false;
    private bool targetsExist = false;

    // Use this for initialization
    void Start()
    {
        considerTargets = true;
        shakeDur = startShakeDur;
        myCam = GetComponent<Camera>();
        //CamSize = myCam.orthographicSize;
        MeanOfPositions();

        offset = transform.position - meanPosition;

        transform.position = meanPosition + offset;

        //ball = GameManager.Instance.gmBall;

        if(movePos == null)
        {
            movePos = GetComponent<Transform>();
        }
    }

    void LateUpdate()
    {
        SetCamera();
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (shaking)
                return;
            shaking = true;
            shakeDur = startShakeDur;
        }

        if (shaking)
        {
            Shaker();
        }
    }
    public void ScreenShake()
    {
        if (!shaking)
        {
            shakeDur = startShakeDur;
            shaking = true;
        }
    }
    public void Shaker()
    {
        startPos = movePos.localPosition;
        float tempShakeMag;
        if (myCam.orthographicSize < 18f)
        {
            tempShakeMag = shakeMag / 2;
        } else
        {
            tempShakeMag = shakeMag;
        }
        if(shakeDur > 0)
        {
            movePos.localPosition = startPos + Random.insideUnitSphere * tempShakeMag;

            shakeDur -= Time.deltaTime * killShake;
        }
        else
        {
            shakeDur = 0f;
            movePos.localPosition = startPos;
            shaking = false;
        }
    }
    // find mean of positions
    void MeanOfPositions()
    {
        positionSum = Vector2.zero;
        for (int i = 0; i < GameManager.Instance.TotalNumberofPlayers; i++)
        {
            positionSum += GameManager.Instance.gmPlayers[i].transform.position;
        }
        
        // Checks to see if there is a target manager associated and if there are targets to look for
        if(GameManager.Instance.gmTargetManager != null)
        {
            if(GameManager.Instance.gmTargetManager.GetTargetArrayLength() > 0)
            {
                targetsExist = true;
            }
            else
            {
                targetsExist = false;
            }
        }
        else
        {
            targetsExist = false;
        }
        if (considerTargets && targetsExist)
        {
            int targetCount = 0;
            for (int i = 0; i < GameManager.Instance.gmTargetManager.GetTargetArrayLength(); i++)
            {
                if (!GameManager.Instance.gmTargetManager.GetTargetAtIndex(i).isHit)
                {
                    positionSum += GameManager.Instance.gmTargetManager.GetTargetAtIndex(i).transform.position;
                    targetCount++;
                }
            }
            meanPosition = positionSum / (GameManager.Instance.gmPlayers.Count + targetCount);
        }
        else
        {
            meanPosition = positionSum / (GameManager.Instance.TotalNumberofPlayers);
        }

        if (GameManager.Instance.gmBalls[0] != null)
        {
            meanPosition = (meanPosition + GameManager.Instance.gmBalls[0].transform.position) / 2;
        }
    }

    void FindPanning()
    {
        //get average position of players
        //then use it to get the average position of the ball and the players
        MeanOfPositions();
        camSize = myCam.orthographicSize;
        maxXDistance = 0f;
        maxYDistance = 0f;
        // get maxXDistance       
        for (int i = 0; i < GameManager.Instance.TotalNumberofPlayers; i++)
        {
            /*   
            if(maxXDistance < Vector3.Distance(meanPosition, GameManager.Instance.gmPlayers[i].transform.position))
            {
                maxXDistance = Vector3.Distance(meanPosition, GameManager.Instance.gmPlayers[i].transform.position);
            }
            */
            if (maxXDistance < Mathf.Abs(meanPosition.x - GameManager.Instance.gmPlayers[i].transform.position.x))
            {
                maxXDistance = Mathf.Abs(meanPosition.x - GameManager.Instance.gmPlayers[i].transform.position.x);
            }
            if (maxYDistance < Mathf.Abs(meanPosition.y - GameManager.Instance.gmPlayers[i].transform.position.y))
            {
                maxYDistance = Mathf.Abs(meanPosition.y - GameManager.Instance.gmPlayers[i].transform.position.y);
            }
        }
        if (considerTargets)
        {
            for (int i = 0; i < GameManager.Instance.gmTargetManager.GetTargetArrayLength(); i++)
            {
                if (!GameManager.Instance.gmTargetManager.GetTargetAtIndex(i).isHit)
                {
                    if (maxXDistance < Mathf.Abs(meanPosition.x - GameManager.Instance.gmTargetManager.GetTargetAtIndex(i).transform.position.x))
                    {
                        maxXDistance = Mathf.Abs(meanPosition.x - GameManager.Instance.gmTargetManager.GetTargetAtIndex(i).transform.position.x);
                    }
                    if (maxYDistance < Mathf.Abs(meanPosition.y - GameManager.Instance.gmTargetManager.GetTargetAtIndex(i).transform.position.y))
                    {
                        maxYDistance = Mathf.Abs(meanPosition.y - GameManager.Instance.gmTargetManager.GetTargetAtIndex(i).transform.position.y);
                    }
                }
            }

        }
        if (GameManager.Instance.gmBalls[0] != null)
        {
            if (maxXDistance < Mathf.Abs(meanPosition.x - GameManager.Instance.gmBalls[0].transform.position.x))
            {
                maxXDistance = Mathf.Abs(meanPosition.x - GameManager.Instance.gmBalls[0].transform.position.x);
            }
            if (maxYDistance < Mathf.Abs(meanPosition.y - GameManager.Instance.gmBalls[0].transform.position.y))
            {
                maxYDistance = Mathf.Abs(meanPosition.y - GameManager.Instance.gmBalls[0].transform.position.y);
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
    }

    void SetCamera()
    {
        //lerp to new camera size and position
		Vector3 currentPos = transform.position;
        meanPosition.y += offsetUp;
        meanPosition.y = Mathf.Max(meanPosition.y, minCameraHeight);
        if (meanPosition.x > 0)
        {
            meanPosition.x = Mathf.Min(meanPosition.x, maxDistanceFromZero);
        }
        else
        {
            meanPosition.x = Mathf.Max(meanPosition.x, -maxDistanceFromZero);
        }
		Vector3 myLerp = Vector3.Lerp(currentPos, meanPosition, (Time.deltaTime*smoothing));

        myLerp.z = currentPos.z;
        transform.position = myLerp;
        FindPanning();
        
        panningY = Mathf.Max(minCamSize, panningY);
        panningY = Mathf.Min(maxCamSize, panningY);

        myCam.orthographicSize = Mathf.Lerp(myCam.orthographicSize, panningY, Time.deltaTime * smoothing);


    }

}
