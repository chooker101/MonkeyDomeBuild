using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurtainsController : MonoBehaviour
{
    public float percentageComplete;
    public float timeStartedLerping;
    public float timeSinceStarted;
    public float timeTakenDuringLerp;
    public bool isLerping;

    public Image leftCurtain;
    public Image rightCurtain;
    private Vector3 leftInitLoc;
    private Vector3 rightInitLoc;
    public Transform leftFinalLoc;
    public Transform rightFinalLoc;
    private Vector3 leftTargetLoc;
    private Vector3 rightTargetLoc;
    private Vector3 leftTargetInitLoc;
    private Vector3 rightTargetInitLoc;
    private float openTime = 2f;

    void Start()
    {
        GameManager.Instance.gmCurtainController = this;
        leftInitLoc = leftCurtain.transform.localPosition;
        rightInitLoc = rightCurtain.transform.localPosition;
        OpenCurtain();
    }
    void Update()
    {
        if (isLerping)
        {

            if (Vector3.Distance(leftCurtain.transform.localPosition, leftTargetLoc) > 0.1f)
            {
                Vector3 tempPos = Vector3.SlerpUnclamped(leftTargetInitLoc, leftTargetLoc, percentageComplete);
                tempPos.y = 0;
                tempPos.z = 0;
                leftCurtain.transform.localPosition = tempPos;
            }
            if (Vector3.Distance(rightCurtain.transform.localPosition, rightTargetLoc) > 0.1f)
            {
                Vector3 tempPos = Vector3.SlerpUnclamped(rightTargetInitLoc, rightTargetLoc, percentageComplete);
                tempPos.y = 0;
                tempPos.z = 0;
                rightCurtain.transform.localPosition = tempPos;
            }

            if (percentageComplete >= 1.0f)
            {
                isLerping = false;
            }
            else
            {
                timeStartedLerping += Time.deltaTime;
                percentageComplete = timeStartedLerping / openTime;
            }
        }
        /*
        if (Input.GetKeyDown(KeyCode.J))
        {
            CloseCurtain();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            OpenCurtain();
        }
        */
    }
    public void OpenCurtain()
    {
        isLerping = true;
        timeStartedLerping = 0;
        percentageComplete = 0;
        leftTargetLoc = leftInitLoc;
        rightTargetLoc = rightInitLoc;
        leftTargetInitLoc = leftFinalLoc.transform.localPosition;
        rightTargetInitLoc = rightFinalLoc.transform.localPosition;
    }
    public void CloseCurtain()
    {
        isLerping = true;
        timeStartedLerping = 0;
        percentageComplete = 0;
        leftTargetLoc = leftFinalLoc.localPosition;
        rightTargetLoc = rightFinalLoc.localPosition;
        leftTargetInitLoc = leftInitLoc;
        rightTargetInitLoc = rightInitLoc;
    }
}
