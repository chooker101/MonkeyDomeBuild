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

	public AudioSource ballInAirSE;
	public AudioSource monkeyCatchBananaSE;
	public AudioSource monkeyCatchPoopSE;
	public AudioSource bananaHitSurfaceSE;
	public AudioSource poopHitSurfaceSE;
	public AudioSource targetPopupSE;
	public AudioSource targetRetractSE;
	public AudioSource targetUpgradeSE;
	public AudioSource targetDowngradeSE;
	public AudioSource targetHitSE;


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
    public void PlayBallInAirSE()
    {
        if (ballInAirSE != null && ballInAirSE.clip != null && !mute)
        {
            ballInAirSE.PlayOneShot(ballInAirSE.clip);
        }
    }
    public void PlayMonkeyCatchBananaSE()
    {
        if (monkeyCatchBananaSE != null && monkeyCatchBananaSE.clip != null && !mute)
        {
            monkeyCatchBananaSE.PlayOneShot(monkeyCatchBananaSE.clip);
        }
    }

    public void PlayMonkeyCatchPoopSE()
    {
        if (monkeyCatchPoopSE != null && monkeyCatchPoopSE.clip != null && !mute)
        {
            monkeyCatchPoopSE.PlayOneShot(monkeyCatchPoopSE.clip);
        }
    }
    public void PlayBananaHitSurfaceSE()
    {
        if (bananaHitSurfaceSE != null && bananaHitSurfaceSE.clip != null && !mute)
        {
            bananaHitSurfaceSE.PlayOneShot(bananaHitSurfaceSE.clip);
        }
    }
    public void PlayPoopHitSurfaceSE()
    {
        if (poopHitSurfaceSE != null && poopHitSurfaceSE.clip != null && !mute)
        {
            poopHitSurfaceSE.PlayOneShot(poopHitSurfaceSE.clip);
        }
    }
    public void PlayTargetPopupSE()
    {
        if (targetPopupSE != null && targetPopupSE.clip != null && !mute)
        {
            targetPopupSE.PlayOneShot(targetPopupSE.clip);
        }
    }
    public void PlayTargetRetractSE()
    {
        if (targetRetractSE != null && targetRetractSE.clip != null && !mute)
        {
            targetRetractSE.PlayOneShot(targetRetractSE.clip);
        }
    }
    public void PlayTargetUpgradeSE()
    {
        if (targetUpgradeSE != null && targetUpgradeSE.clip != null && !mute)
        {
            targetUpgradeSE.PlayOneShot(targetUpgradeSE.clip);
        }
    }
    public void PlayTargetDowngradeSE()
    {
        if (targetDowngradeSE != null && targetDowngradeSE.clip != null && !mute)
        {
            targetDowngradeSE.PlayOneShot(targetDowngradeSE.clip);
        }
    }
    public void PlayTargetHitSE()
    {
        if (targetHitSE != null && targetHitSE.clip != null && !mute)
        {
            targetHitSE.PlayOneShot(targetHitSE.clip);
        }
    }

}
