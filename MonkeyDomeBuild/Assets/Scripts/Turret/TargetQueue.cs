using UnityEngine;
using System.Collections;

public class TargetQueue
{
    private GameObject target;
    private GameObject whatToFire;

    public TargetQueue(GameObject target, GameObject whatToFire)
    {
        this.target = target;
        this.whatToFire = whatToFire;
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public GameObject GetWhatToFire()
    {
        return whatToFire;
    }

}
