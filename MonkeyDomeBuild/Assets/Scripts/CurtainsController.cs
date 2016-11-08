using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurtainsController : MonoBehaviour
{
    public Image leftCurtain;
    public Image rightCurtain;
    private Vector3 leftInitLoc;
    private Vector3 rightInitLoc;
    public Transform leftFinalLoc;
    public Transform rightFinalLoc;
    private Vector3 leftTargetLoc;
    private Vector3 rightTargetLoc;

    void Start()
    {
        GameManager.Instance.gmCurtainController = this;
        leftInitLoc = leftCurtain.transform.localPosition;
        rightInitLoc = rightCurtain.transform.localPosition;
        OpenCurtain();
    }
    void Update()
    {
        if (Vector3.Distance(leftCurtain.transform.localPosition, leftTargetLoc) > 0.1f)
        {
            Vector3 tempPos = Vector3.SlerpUnclamped(leftCurtain.transform.localPosition, leftTargetLoc, Time.deltaTime);
            tempPos.y = 0;
            tempPos.z = 0;
            leftCurtain.transform.localPosition = tempPos;
        }
        if (Vector3.Distance(rightCurtain.transform.localPosition, rightTargetLoc) > 0.1f)
        {
            Vector3 tempPos = Vector3.SlerpUnclamped(rightCurtain.transform.localPosition, rightTargetLoc, Time.deltaTime);
            tempPos.y = 0;
            tempPos.z = 0;
            rightCurtain.transform.localPosition = tempPos;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            //CloseCurtain();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            //OpenCurtain();
        }
    }
    public void OpenCurtain()
    {
        leftTargetLoc = leftInitLoc;
        rightTargetLoc = rightInitLoc;
    }
    public void CloseCurtain()
    {
        leftTargetLoc = leftFinalLoc.localPosition;
        rightTargetLoc = rightFinalLoc.localPosition;
    }
}
