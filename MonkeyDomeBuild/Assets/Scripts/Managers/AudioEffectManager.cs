﻿using UnityEngine;
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
    public AudioSource bananaInBasketSE;

    public AudioSource monkeyCallBallSE; 
    public AudioSource gorillaChargeupSE;
    public AudioSource gorillaTackleSE;
    public AudioSource gorillaTackleHitSurfaceSE;
    public AudioSource gorillaTackleHitMonkeySE;
    public AudioSource gorillaInterceptionSE;

    public AudioSource pregameSpinnerSE;
    public AudioSource shotClockBuzzSE;

    public AudioSource menuButtonSE;

    public AudioSource audienceCheer1;
    public AudioSource audienceCheer2;
    public AudioSource audienceCheer3;

    public AudioSource audienceBoo1;
    public AudioSource audienceBoo2;
    public AudioSource audienceBoo3;



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

    //Audience Sound Effects
    public void PlayAudienceCheer1()
    {
        if (audienceCheer1 != null && audienceCheer1.clip != null && !mute)
        {
            audienceCheer1.PlayOneShot(audienceCheer1.clip);
        }
    }
    public void PlayAudienceCheer2()
    {
        if (audienceCheer2 != null && audienceCheer2.clip != null && !mute)
        {
            audienceCheer2.PlayOneShot(audienceCheer2.clip);
        }
    }
    public void PlayAudienceCheer3()
    {
        if (audienceCheer3 != null && audienceCheer3.clip != null && !mute)
        {
            audienceCheer3.PlayOneShot(audienceCheer3.clip);
        }
    }
    public void PlayAudienceBoo1()
    {
        if (audienceBoo1 != null && audienceBoo1.clip != null && !mute)
        {
            audienceBoo1.PlayOneShot(audienceBoo1.clip);
        }
    }
    public void PlayAudienceBoo2()
    {
        if (audienceBoo2 != null && audienceBoo2.clip != null && !mute)
        {
            audienceBoo2.PlayOneShot(audienceBoo2.clip);
        }
    }
    public void PlayAudienceBoo3()
    {
        if (audienceBoo3 != null && audienceBoo3.clip != null && !mute)
        {
            audienceBoo3.PlayOneShot(audienceBoo3.clip);
        }
    }
    //Monkey Sound Effects
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
    public void PlayMonkeyCallBallSE()
    {
        if (monkeyCallBallSE != null && monkeyCallBallSE.clip != null && !mute)
        {
            monkeyCallBallSE.PlayOneShot(monkeyCallBallSE.clip);
        }
    }

    //Ball Sound Effects
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
    //Target Sound Effects
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

    public void PlayBananaInBasketSE()
    {
        if (bananaInBasketSE != null && bananaInBasketSE.clip != null && !mute)
        {
            bananaInBasketSE.PlayOneShot(bananaInBasketSE.clip);
        }
    }

    //Gorilla Sound Effects
    public void PlayGorillaChargeupSE()
    {
        if (gorillaChargeupSE != null && gorillaChargeupSE.clip != null && !mute)
        {
            gorillaChargeupSE.PlayOneShot(gorillaChargeupSE.clip);
        }
    }
    public void PlayGorillaTackleSE()
    {
        if (gorillaTackleSE != null && gorillaTackleSE.clip != null && !mute)
        {
            gorillaTackleSE.PlayOneShot(gorillaTackleSE.clip);
        }
    }
    public void PlayGorillaTackleSurfaceHitSE()
    {
        if (gorillaTackleHitSurfaceSE != null && gorillaTackleHitSurfaceSE.clip != null && !mute)
        {
            gorillaTackleHitSurfaceSE.PlayOneShot(gorillaTackleHitSurfaceSE.clip);
        }
    }
    public void PlayGorillaTackleHitMonkeySE()
    {
        if (gorillaTackleHitMonkeySE != null && gorillaTackleHitMonkeySE.clip != null && !mute)
        {
            gorillaTackleHitMonkeySE.PlayOneShot(gorillaTackleHitMonkeySE.clip);
        }
    }

    public void PlayGorillaInterceptionSE()
    {
        if (gorillaInterceptionSE != null && gorillaInterceptionSE.clip != null && !mute)
        {
            gorillaInterceptionSE.PlayOneShot(gorillaInterceptionSE.clip);
        }
    }

    public void PlayPregameSpinnerSE()
    {
        if (pregameSpinnerSE != null && pregameSpinnerSE.clip != null && !mute)
        {
            pregameSpinnerSE.PlayOneShot(pregameSpinnerSE.clip);
        }
    }

    public void PlayShotClockBuzzSE()
    {
        if (shotClockBuzzSE != null && shotClockBuzzSE.clip != null && !mute)
        {
            shotClockBuzzSE.PlayOneShot(shotClockBuzzSE.clip);
        }
    }
    // Menu Sound Effects
    public void PlayMenuButtonSE()
    {
        if (menuButtonSE != null && menuButtonSE.clip != null && !mute)
        {
            menuButtonSE.PlayOneShot(menuButtonSE.clip);
        }
    }

}
