using UnityEngine;
using System.Collections;

public class AudioEffectManager : MonoBehaviour
{
    private static AudioEffectManager myInstance;
    public bool mute = false;
    public AudioSource monkeyJumpSE;
    public AudioSource monkeyThrowSE;
    public AudioSource monkeyCatchSE;
    public AudioSource monkeyPerfectCatchSE;
    public AudioSource monkeylandOnSurfaceSE;
    public AudioSource monkeyGrabLatticeSE;
    public AudioSource ballBounceSoftSE;
    public AudioSource ballBounceHardSE;



    public static AudioEffectManager Instance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<AudioEffectManager>();
            }
            return myInstance;
        }
    }
    public void PlayMonkeyJumpSE()
    {
        if (monkeyJumpSE != null && monkeyJumpSE.clip != null && !mute)
        {
            monkeyJumpSE.PlayOneShot(monkeyJumpSE.clip);
        }
    }
    public void PlayMonkeyThrowSE()
    {
        if (monkeyThrowSE != null && monkeyThrowSE.clip != null && !mute)
        {
            monkeyThrowSE.PlayOneShot(monkeyThrowSE.clip);
        }
    }
    public void PlayMonkeyCatchSE()
    {
        if (monkeyCatchSE != null && monkeyCatchSE.clip != null && !mute)
        {
            monkeyCatchSE.PlayOneShot(monkeyCatchSE.clip);
        }
    }
    public void PlayMonkeyPerfectCatchSE()
    {
        if (monkeyPerfectCatchSE != null && monkeyPerfectCatchSE.clip != null && !mute)
        {
            monkeyPerfectCatchSE.PlayOneShot(monkeyPerfectCatchSE.clip);
        }
    }
    public void PlayMonkeyLandOnSurfaceSE()
    {
        if (monkeylandOnSurfaceSE != null && monkeylandOnSurfaceSE.clip != null && !mute)
        {
            monkeylandOnSurfaceSE.PlayOneShot(monkeyThrowSE.clip);
        }
    }
    public void PlayMonkeyGrabLatticeSE()
    {
        if (monkeyGrabLatticeSE != null && monkeyGrabLatticeSE.clip != null && !mute)
        {
            monkeyGrabLatticeSE.PlayOneShot(monkeyGrabLatticeSE.clip);
        }
    }
    public void PlayBallBounceSoftSE()
    {
        if (ballBounceSoftSE != null && ballBounceSoftSE.clip != null && !mute)
        {
            ballBounceSoftSE.PlayOneShot(ballBounceSoftSE.clip);
        }
    }
    public void PlayBallBounceHardSE()
    {
        if (ballBounceHardSE != null && ballBounceHardSE.clip != null && !mute)
        {
            ballBounceHardSE.PlayOneShot(ballBounceHardSE.clip);
        }
    }

}
