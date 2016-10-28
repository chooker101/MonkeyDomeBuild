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

    void Awake()
    {
        moveLoc = transform.FindChild("MoveLoc");
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
