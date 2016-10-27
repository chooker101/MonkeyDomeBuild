﻿using UnityEngine;
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
    public float smoothing = 2f;
    public float maxCamSize = 40f;
    public float minCamSize = 25f;
    private float minCameraHeight = 12f;
    public float maxDistanceXFromZero = 4f;
    public float maxDistanceYFromZero = 20f;
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
    public float focusAmount = 1f;

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
        int targetCount = 0;
        if (considerTargets && targetsExist)
        {
            for (int i = 0; i < GameManager.Instance.gmTargetManager.GetTargetArrayLength(); i++)
            {
                if (!GameManager.Instance.gmTargetManager.GetTargetAtIndex(i).isHit)
                {
                    positionSum += GameManager.Instance.gmTargetManager.GetTargetAtIndex(i).transform.position;
                    targetCount++;
                }
            }
            //meanPosition = positionSum / (GameManager.Instance.gmPlayers.Count + targetCount);
            meanPosition = positionSum / targetCount;
            positionSum = Vector2.zero;
        }
        else
        {
            //meanPosition = positionSum / (GameManager.Instance.TotalNumberofPlayers);
        }
        bool monkeyCharging = false;
        if (GameManager.Instance.gmBalls[0] != null)
        {
            if (GameManager.Instance.gmBalls[0].GetComponent<BallInfo>().GetHoldingMonkey() != null)
            {
                if (GameManager.Instance.gmBalls[0].GetComponent<BallInfo>().GetHoldingMonkey().GetComponent<Actor>().IsChargingThrow)
                {
                    if (!monkeyCharging)
                    {
                        focusAmount = 1f;
                    }
                    monkeyCharging = true;
                }
            }
        }
        Actor throwingMonkey = null;
        for (int i = 0; i < GameManager.Instance.TotalNumberofPlayers; i++)
        {
            if (monkeyCharging)
            {
                if (GameManager.Instance.gmPlayers[i] != GameManager.Instance.gmBalls[0].GetComponent<BallInfo>().GetHoldingMonkey())
                {
                    positionSum += GameManager.Instance.gmPlayers[i].transform.position;
                }
                else if(GameManager.Instance.gmPlayers[i] == GameManager.Instance.gmBalls[0].GetComponent<BallInfo>().GetHoldingMonkey())
                {
                    throwingMonkey = GameManager.Instance.gmPlayers[i].GetComponent<Actor>();
                }
            }
            else
            {
                positionSum += GameManager.Instance.gmPlayers[i].transform.position;
            }
        }
        if (throwingMonkey != null)
        {
            int i = (int)GameManager.Instance.TotalNumberofPlayers - 1;
            if (i == 0)
            {
                i = 1;
            }
            meanPosition = positionSum / i;
        }
        else
        {
            int i = (int)GameManager.Instance.TotalNumberofPlayers;
            if (i == 0)
            {
                i = 1;
            }
            meanPosition = positionSum / i;
        }
        if (GameManager.Instance.gmBalls[0] != null)
        {
            if (throwingMonkey != null)
            {
                meanPosition += GameManager.Instance.gmBalls[0].transform.position;
                meanPosition /= 2;
                meanPosition += throwingMonkey.transform.position;
                focusAmount = Mathf.Lerp(focusAmount, 2, Time.deltaTime * 20f);
                meanPosition /= focusAmount;
            }
            else
            {
                focusAmount = Mathf.Lerp(focusAmount, 1, Time.deltaTime * 5f);
                meanPosition += GameManager.Instance.gmBalls[0].transform.position;
                meanPosition = meanPosition / 2;
            }
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
            meanPosition.x = Mathf.Min(meanPosition.x, maxDistanceXFromZero);
        }
        else
        {
            meanPosition.x = Mathf.Max(meanPosition.x, -maxDistanceXFromZero);
        }
        if (meanPosition.y > 0)
        {
            meanPosition.y = Mathf.Min(meanPosition.y, maxDistanceYFromZero);
        }
        Vector3 myLerp = Vector3.Lerp(currentPos, meanPosition, (Time.deltaTime * smoothing));

        myLerp.z = currentPos.z;
        transform.position = myLerp;
        FindPanning();
        
        panningY = Mathf.Max(minCamSize, panningY);
        panningY = Mathf.Min(maxCamSize, panningY);

        myCam.orthographicSize = Mathf.Lerp(myCam.orthographicSize, panningY, Time.deltaTime * smoothing);


    }

}
