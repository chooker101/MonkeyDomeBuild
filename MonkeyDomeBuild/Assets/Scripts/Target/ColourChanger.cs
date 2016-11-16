using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ColourChanger : MonoBehaviour
{
    public TargetAxis targetAxis = TargetAxis.OnTop;
    public int playerTargetIndex = -1;
    public bool isHit = false;
    public bool activated = false;
    public Transform hitParticlePivot;
    public Image targetBase;
    public Image targetJoin;
    public Image targetUnready;
    public Image targetReady;
    public Image targetReadyAll;
    public Image targetHeadMonkey;
    public Image targetHeadGorilla;
    public Text playerNumberText;

    private int playerTargetNumberRegular = 0;
    private GameObject objectHit = null;
    private Material materialToApply;
    private Vector3 targetRot;
    private GameObject target;
    private bool canEnableTargetHeadCollider = false;
    private PreGameTimer preGameTimerObject;

    bool join = false;

    // Use this for initialization
    void Start()
    {
        // Fills a list with all current players
        target = transform.FindChild("Pivot").gameObject;
		//myPlayer = GameManager.Instance.gmPlayers[playerTargetIndex];
        playerTargetNumberRegular = playerTargetIndex + 1;
        GetComponentInChildren<CircleCollider2D>().enabled = false;
        preGameTimerObject = FindObjectOfType<PreGameTimer>().GetComponent<PreGameTimer>();
        Init();
        if (GameManager.Instance.TotalNumberofPlayers >= playerTargetNumberRegular)
        {
            activated = true;
            TargetSetter();
        }

        if(activated)
        {
            playerNumberText.gameObject.SetActive(true);
            playerNumberText.text = "P" + playerTargetNumberRegular.ToString();
        }
        else
        {
            playerNumberText.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        CheckNewPlayerJoinInput();
        DebugFunction();
        // Add player target to join
        if (join && !activated)
        {
            GameManager.Instance.AddPlayer(playerTargetIndex);
            activated = true;
            GetComponentInChildren<Collider2D>().enabled = false;
            TargetSetter();
            playerNumberText.gameObject.SetActive(false);
            playerNumberText.text = "P" + playerTargetNumberRegular.ToString();

        }
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

        // State of target
        if(!activated)
        {
            if (!preGameTimerObject.AllTargetsHit() && !targetJoin.gameObject.activeSelf)
            {
                targetBase.gameObject.SetActive(true);
                targetJoin.gameObject.SetActive(true);
                targetUnready.gameObject.SetActive(false);
                targetReady.gameObject.SetActive(false);
                targetReadyAll.gameObject.SetActive(false);
            }
            else if(preGameTimerObject.AllTargetsHit() && targetJoin.gameObject.activeSelf)
            {
                targetBase.gameObject.SetActive(false);
                targetJoin.gameObject.SetActive(false);
                targetUnready.gameObject.SetActive(false);
                targetReady.gameObject.SetActive(false);
                targetReadyAll.gameObject.SetActive(false);
            }
        }
        else if(activated && !isHit)
        {
            if(!targetUnready.gameObject.activeSelf)
            {
                targetBase.gameObject.SetActive(true);
                targetJoin.gameObject.SetActive(false);
                targetUnready.gameObject.SetActive(true);
                targetReady.gameObject.SetActive(false);
                targetReadyAll.gameObject.SetActive(false);
            }
        }
        else if(activated && isHit && !preGameTimerObject.AllTargetsHit())
        {
            if (!targetReady.gameObject.activeSelf)
            {
                targetBase.gameObject.SetActive(true);
                targetJoin.gameObject.SetActive(false);
                targetUnready.gameObject.SetActive(false);
                targetReady.gameObject.SetActive(true);
                targetReadyAll.gameObject.SetActive(false);
            }
        }
        else if(activated && isHit && preGameTimerObject.AllTargetsHit())
        {
            if (!targetReadyAll.gameObject.activeSelf)
            {
                targetBase.gameObject.SetActive(true);
                targetJoin.gameObject.SetActive(false);
                targetUnready.gameObject.SetActive(false);
                targetReady.gameObject.SetActive(false);
                targetReadyAll.gameObject.SetActive(true);
            }
        }

        if(activated) // Displays player heads on panel
        {
            if (GameManager.Instance.gmPlayerScripts[playerTargetIndex].characterType is Gorilla && !targetHeadGorilla.gameObject.activeSelf)
            {
                targetHeadGorilla.gameObject.SetActive(true);
                targetHeadMonkey.gameObject.SetActive(false);
            }
            else if (GameManager.Instance.gmPlayerScripts[playerTargetIndex].characterType is Monkey && !targetHeadMonkey.gameObject.activeSelf)
            {
                targetHeadGorilla.gameObject.SetActive(false);
                targetHeadMonkey.gameObject.SetActive(true);
            }
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
                GameObject particle = ParticlesManager.Instance.TargetHitParticle;
                particle.SetActive(true);
                particle.transform.position = hitParticlePivot.position;
                Vector3 ballPos = other.transform.position;
                ballPos.z = 0;
                Vector3 pivotPos = hitParticlePivot.position;
                pivotPos.z = 0;
                Quaternion targetAng = Quaternion.FromToRotation(Vector3.right, (ballPos - pivotPos).normalized);
                particle.transform.rotation = Quaternion.Euler(0, 0, targetAng.eulerAngles.z);
                particle.GetComponentInChildren<ParticleSystem>().Play();
                if (objectHit.GetComponent<BallInfo>().GetLastThrowMonkey().GetComponent<Actor>().playerIndex == playerTargetIndex) // If the player who threw the ball is the one for this target
                {
                    isHit = true;
                    if (objectHit.GetComponent<BallInfo>().GetLastThrowMonkey().GetComponent<Actor>().IsHoldingBall)
                    {
                        objectHit.GetComponent<BallInfo>().GetLastThrowMonkey().GetComponent<Actor>().ReleaseBall();
                    }
                    GameManager.Instance.gmRecordKeeper.SetPlayerMaterial(playerTargetIndex, materialToApply);
                    GameManager.Instance.gmPlayers[playerTargetIndex].GetComponent<Actor>().UpdateColour();
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
        switch (playerTargetIndex)
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
                GameManager.Instance.gmRecordKeeper.ResetPlayerMaterial(playerTargetIndex);
                GameManager.Instance.RemovePlayer(playerTargetIndex);
                activated = false;
                GetComponentInChildren<Collider2D>().enabled = false;
                Reset();
            }
            else
            {
                GameManager.Instance.AddPlayer(playerTargetIndex);
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
    void CheckNewPlayerJoinInput()
    {
        string p = "p" + (playerTargetIndex + 1);
        if (Input.GetJoystickNames().Length >= playerTargetIndex + 1)
        {
            if (Input.GetJoystickNames()[playerTargetIndex] != null)
            {
                if (Input.GetJoystickNames()[playerTargetIndex] == "Wireless Controller")
                {
                    join = Input.GetButtonDown(p+"_ps4_jump");
                }
                else if (Input.GetJoystickNames()[playerTargetIndex] == "XBOX 360 For Windows (Controller)")
                {
                    join = Input.GetButtonDown(p+"_jump");
                }
                else
                {
                    join = Input.GetButtonDown(p+"_jump");
                }
            }
            else
            {
                if (Input.GetJoystickNames()[playerTargetIndex] == "Wireless Controller")
                    join = Input.GetButtonDown(p+"_ps4_jump");
                else if (Input.GetJoystickNames()[playerTargetIndex] == "XBOX 360 For Windows (Controller)")
                    join = Input.GetButtonDown(p+"_jump");
            }
        }
    }
}
