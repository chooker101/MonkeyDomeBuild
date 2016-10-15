using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ColourChanger : MonoBehaviour
{
    public TargetAxis targetAxis = TargetAxis.OnTop;
    public int playerTargetNumber = -1;
    private int playerTargetNumberRegular = 0;
    public Text targetText;

    //private RecordKeeper recordKeeper;
    //private CircleCollider2D targetCollider;
    
    private GameObject myPlayer = null;
    private GameObject objectHit = null;
    private Material materialToApply;
    public bool isHit = false;
    private Vector3 targetRot;
    private GameObject target;
    public bool activated = false;
    public bool canEnableTargetHeadCollider = false;

    // Use this for initialization
    void Start()
    {
        // Fills a list with all current players
        target = transform.FindChild("Pivot").gameObject;
		myPlayer = GameManager.Instance.gmPlayers[playerTargetNumber];
        playerTargetNumberRegular = playerTargetNumber + 1;
        GetComponentInChildren<CircleCollider2D>().enabled = false;
        Init();
        if (GameManager.Instance.TotalNumberofPlayers >= playerTargetNumberRegular)
        {
            activated = true;
            TargetSetter();
        }
    }
    void Update()
    {
        DebugFunction();
        if (Vector3.Distance(target.transform.localEulerAngles, targetRot) > 0.01f)
        {
            target.transform.localRotation = Quaternion.LerpUnclamped(target.transform.localRotation, Quaternion.Euler(targetRot), Time.deltaTime * 10f);
        }
        if (activated)
        {
            if (Vector3.Distance(target.transform.localEulerAngles, targetRot) > 0.2f)
            {
                if (canEnableTargetHeadCollider)
                {
                    canEnableTargetHeadCollider = false;
                    GetComponentInChildren<CircleCollider2D>().enabled = true;
                }
            }
        }
        if (isHit)
        {
            targetText.text = "P" + playerTargetNumberRegular.ToString() + " READY!";
        }
        else
        {
            targetText.text = "P" + playerTargetNumberRegular.ToString() + " NOT READY";
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Ball") )
        {
            objectHit = other.GetComponentInParent<BallInfo>().gameObject;
            if(objectHit != null)
            {
                materialToApply = objectHit.GetComponent<BallInfo>().mySpriteColour; // Get the material from the ball

                if (objectHit.GetComponent<BallInfo>().GetLastThrowMonkey().GetComponent<Player>().playerIndex == playerTargetNumber) // If the player who threw the ball is the one for this target
                {
                    isHit = true;
                    if (objectHit.GetComponent<BallInfo>().GetLastThrowMonkey().GetComponent<Player>().IsHoldingBall)
                    {
                        objectHit.GetComponent<BallInfo>().GetLastThrowMonkey().GetComponent<Player>().ReleaseBall();
                    }
                    for (int i = 0; i < GameManager.Instance.gmRecordKeeper.colourPlayers.Length; i++)
                    {
                        if (i == myPlayer.GetComponent<Player>().playerIndex)
                        {
							GameManager.Instance.gmRecordKeeper.colourPlayers[i] = materialToApply;
                            break;
                        }
                    }
                }
                else
                {
                }
            }
        }
    }
    void Init()
    {
        switch (targetAxis)
        {
            case TargetAxis.OnGround:
                targetRot = new Vector3(90f, 0, 0);
                break;
            case TargetAxis.OnLeftSide:
                targetRot = new Vector3(0, 270f, 270f);
                break;
            case TargetAxis.OnRightSide:
                targetRot = new Vector3(0, 90f, 90);
                break;
            case TargetAxis.OnTop:
                targetRot = new Vector3(270f, 0, 180f);
                break;
        }
        target.transform.localEulerAngles = targetRot;
    }
    void TargetSetter()
    {
        // set target axis in editor for each target
        targetRot = Vector3.zero;
        switch (targetAxis)
        {
            case TargetAxis.OnGround:
                targetRot = new Vector3(0, 0, 0);
                break;
            case TargetAxis.OnLeftSide:
                targetRot = new Vector3(0, 0, 270f);
                break;
            case TargetAxis.OnRightSide:
                targetRot = new Vector3(0, 0, 90);
                break;
            case TargetAxis.OnTop:
                targetRot = new Vector3(0, 0, 180f);
                break;
        }
        canEnableTargetHeadCollider = true;
    }
    void Reset()
    {
        switch (targetAxis)
        {
            case TargetAxis.OnGround:
                targetRot = new Vector3(90f, 0, 0);
                break;
            case TargetAxis.OnLeftSide:
                targetRot = new Vector3(0, 270f, 270f);
                break;
            case TargetAxis.OnRightSide:
                targetRot = new Vector3(0, 90f, 90f);
                break;
            case TargetAxis.OnTop:
                targetRot = new Vector3(270f, 0, 180f);
                break;
        }
        isHit = false;
        canEnableTargetHeadCollider = false;
        GetComponentInChildren<CircleCollider2D>().enabled = false;
    }
    void DebugFunction()
    {
        bool keyPressed = false;
        bool isHitKeyPressed = false;
        switch (playerTargetNumber)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    keyPressed = true;
                }
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    isHitKeyPressed = true;
                }
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    keyPressed = true;
                }
                if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    isHitKeyPressed = true;
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    keyPressed = true;
                }
                if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    isHitKeyPressed = true;
                }
                break;
            case 3:
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    keyPressed = true;
                }
                if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    isHitKeyPressed = true;
                }
                break;
            case 4:
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    keyPressed = true;
                }
                if (Input.GetKeyDown(KeyCode.Keypad5))
                {
                    isHitKeyPressed = true;
                }
                break;
        }
        if (keyPressed)
        {
            if (activated)
            {
                GameManager.Instance.RemovePlayer(playerTargetNumber);
                activated = false;
                GetComponentInChildren<Collider2D>().enabled = false;
                Reset();
            }
            else
            {
                GameManager.Instance.AddPlayer(playerTargetNumber);
                activated = true;
                GetComponentInChildren<Collider2D>().enabled = false;
                TargetSetter();
            }
        }
        if (isHitKeyPressed && activated)
        {
            isHit = !isHit;
        }
    }
    public bool IsActivated
    {
        get
        {
            return activated;
        }
    }
}
