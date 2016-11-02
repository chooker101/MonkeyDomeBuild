using UnityEngine;
using System.Collections;

public class TargetNode : MonoBehaviour
{
    /*
        The purpose of the script is to spawn a target accrodingly.
    */
    public TargetAxis targetAxis = TargetAxis.OnGround;
    public TargetType targetType = TargetType.Static;
    public Transform moveLoc;
    public GameObject horizontalPanel;
    public GameObject verticalPanel;

    void Awake()
    {
        moveLoc = transform.FindChild("MoveLoc");
        if (GetComponentInParent<TargetNode>().targetAxis == TargetAxis.OnRightSide || GetComponentInParent<TargetNode>().targetAxis == TargetAxis.OnLeftSide)
        {
            horizontalPanel.SetActive(false);
            verticalPanel.SetActive(true);
            verticalPanel.transform.localRotation = Quaternion.Euler(0, 0, 270f);
        }
        else
        {
            horizontalPanel.SetActive(true);
        }
    }
    public void Init()
    {
        GameObject target = (GameObject)Instantiate(TargetNodeManager.Instance.TargetPrefab, transform.position, transform.rotation);
        target.GetComponent<Target>().SetTargetAxis = targetAxis;
        target.GetComponent<Target>().SetTargetType = targetType;
        target.GetComponent<Target>().MoveLocation = moveLoc.position;
        target.GetComponent<Target>().SetTargetBase = GetComponentInChildren<TargetBase>();
        target.transform.SetParent(TargetNodeManager.Instance.transform);

    }
}
