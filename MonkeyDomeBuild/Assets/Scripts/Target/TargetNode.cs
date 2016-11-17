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
    //public GameObject horizontalPanel;
    //public GameObject verticalPanel;
    //public TargetStand stand;

    public void Init()
    {
        moveLoc = transform.FindChild("MoveLoc");
        /*if (GetComponentInParent<TargetNode>().targetAxis == TargetAxis.OnRightSide || GetComponentInParent<TargetNode>().targetAxis == TargetAxis.OnLeftSide)
        {
            horizontalPanel.SetActive(false);
            verticalPanel.SetActive(true);
            verticalPanel.transform.localRotation = Quaternion.Euler(0, 0, 270f);
        {
        else
        {
            horizontalPanel.SetActive(true);
        }*/
        GameObject target = (GameObject)Instantiate(TargetNodeManager.Instance.TargetPrefab, transform.position, transform.rotation);
        target.GetComponent<Target>().SetTargetAxis = targetAxis;
        target.GetComponent<Target>().SetTargetType = targetType;
        target.GetComponent<Target>().MoveLocation = moveLoc.position;
        target.GetComponent<Target>().SetTargetNode(this);
        /*if (horizontalPanel.activeInHierarchy)
        {
            target.GetComponent<Target>().SetTargetBase = horizontalPanel.GetComponent<TargetBase>();
        }
        else
        {
            target.GetComponent<Target>().SetTargetBase = verticalPanel.GetComponent<TargetBase>();
        }*/
        target.transform.SetParent(TargetNodeManager.Instance.transform);

    }
}
