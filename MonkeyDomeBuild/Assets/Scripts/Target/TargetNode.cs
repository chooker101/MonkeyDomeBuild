using UnityEngine;
using System.Collections;

public class TargetNode : MonoBehaviour
{
    /*
        The purpose of the script is to spawn a target accrodingly.
    */
    public TargetAxis targetAxis = TargetAxis.OnGround;
    public void Init()
    {
        GameObject target = (GameObject)Instantiate(TargetNodeManager.Instance.TargetPrefab, transform.position, transform.rotation);
        target.GetComponent<Target>().SetTargetAxis = targetAxis;
        target.transform.SetParent(TargetNodeManager.Instance.transform);
    }
}
