using UnityEngine;
using System.Collections;

public class ShotClockManager : MonoBehaviour
{
    private static float shotClockCount = 0f;
    private static float shotClockTime = 8f;
    private static bool shotClockActive = false;
    public float ShotClockCount
    {
        get
        {
            return shotClockCount;
        }
        set
        {
            shotClockCount = value;
        }
    }
    public float ShotClockTime
    {
        get
        {
            return shotClockTime;
        }
        set
        {
            shotClockTime = value;
        }
    }
    public bool IsShotClockActive
    {
        get
        {
            return shotClockActive;
        }
        set
        {
            shotClockActive = value;
        }
    }
    public void ResetShotClock()
    {
        shotClockCount = 0;
    }
    
}
