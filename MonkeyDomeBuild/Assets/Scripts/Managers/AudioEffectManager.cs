using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public AudioSource unMenuButtonSE;

    public AudioSource audienceCheer1;
    public AudioSource audienceCheer2;
    public AudioSource audienceCheer3;

    public AudioSource audienceBoo1;
    public AudioSource audienceBoo2;
    public AudioSource audienceBoo3;

    public AudioSource audienceCatch;
    public AudioSource audienceInterception;
    public AudioSource audienceShotClock;
    public AudioSource audienceTargetUp;
    public AudioSource audienceSmash;
    public AudioSource audiencePoop;

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
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (audienceCheer1 != null && audienceCheer1.clip != null && !mute)
            {
                audienceCheer1.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
                audienceCheer1.PlayOneShot(audienceCheer1.clip);
            }
        }

    }
    public void PlayAudienceCheer2()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (audienceCheer2 != null && audienceCheer2.clip != null && !mute)
            {
                audienceCheer2.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
                audienceCheer2.PlayOneShot(audienceCheer2.clip);
            }
        }

    }
    public void PlayAudienceCheer3()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (audienceCheer3 != null && audienceCheer3.clip != null && !mute)
            {
                audienceCheer3.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
                audienceCheer3.PlayOneShot(audienceCheer3.clip);
            }
        }

    }
    public void PlayAudienceBoo1()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (audienceBoo1 != null && audienceBoo1.clip != null && !mute)
            {
                audienceBoo1.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
                audienceBoo1.PlayOneShot(audienceBoo1.clip);
            }
        }

    }
    public void PlayAudienceBoo2()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (audienceBoo2 != null && audienceBoo2.clip != null && !mute)
            {
                audienceBoo2.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
                audienceBoo2.PlayOneShot(audienceBoo2.clip);
            }
        }

    }
    public void PlayAudienceBoo3()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (audienceBoo3 != null && audienceBoo3.clip != null && !mute)
            {
                audienceBoo3.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
                audienceBoo3.PlayOneShot(audienceBoo3.clip);
            }
        }

    }
    public void PlayAudienceCatch()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (audienceCatch != null && audienceCatch.clip != null && !mute)
            {
                audienceCatch.pitch = Random.Range(.65f, .9f);
                //audienceCatch.volume = Random.Range(.3f, .6f);
                audienceCatch.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
                audienceCatch.PlayOneShot(audienceCatch.clip);
            }
        }

    }
    public void PlayAudienceInterception()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (audienceInterception != null && audienceInterception.clip != null && !mute)
            {
                audienceInterception.pitch = Random.Range(.95f, 1.05f);
                //audienceCatch.volume = Random.Range(0.65f, 1.05f);
                audienceInterception.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
                audienceInterception.PlayOneShot(audienceInterception.clip);
            }
        }
    }
    public void PlayAudienceShotClock()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (audienceShotClock != null && audienceShotClock.clip != null && !mute)
            {
                audienceShotClock.pitch = Random.Range(.65f, .85f);
                //audienceShotClock.volume = Random.Range(.4f, .8f);
                audienceShotClock.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
                audienceShotClock.PlayOneShot(audienceShotClock.clip);
            }
        }

    }
    public void PlayAudienceTargetUp()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (audienceTargetUp != null && audienceTargetUp.clip != null && !mute)
            {
                audienceTargetUp.pitch = Random.Range(.65f, .85f);
                //audienceTargetUp.volume = Random.Range(.7f, .9f);
                audienceTargetUp.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
                audienceTargetUp.PlayOneShot(audienceTargetUp.clip);
            }
        }

    }
    public void PlayAudienceSmash()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (audienceSmash != null && audienceSmash.clip != null && !mute)
            {
                audienceSmash.pitch = Random.Range(.95f, 1f);
                //audienceCatch.volume = Random.Range(.65f, .8f)
                audienceSmash.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
                audienceSmash.PlayOneShot(audienceSmash.clip);
            }
        }

    }
    public void PlayAudiencePoop()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (audiencePoop != null && audiencePoop.clip != null && !mute)
            {
                audiencePoop.pitch = Random.Range(.65f, 1.05f);
                //audiencePoop.volume = Random.Range(.8f, 1.2f);
                audiencePoop.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
                audiencePoop.PlayOneShot(audiencePoop.clip);
            }
        }

    }
    //Monkey Sound Effects
    public void PlayMonkeyJumpSE()
    {
        if (monkeyJumpSE != null && monkeyJumpSE.clip != null && !mute)
        {
            monkeyJumpSE.pitch = Random.Range(0.7f, 1);
            //monkeyJumpSE.volume = 0.5f;
            monkeyJumpSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume / 2 ;
            monkeyJumpSE.PlayOneShot(monkeyJumpSE.clip);
        }
    }
    public void PlayMonkeyThrowSE()
    {
        if (monkeyThrowSE != null && monkeyThrowSE.clip != null && !mute)
        {
            monkeyThrowSE.pitch = Random.Range(0.85f, 1f);
            //monkeyThrowSE.volume = Random.Range(.35f, .45f);
            monkeyThrowSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume / 2;
            monkeyThrowSE.PlayOneShot(monkeyThrowSE.clip);
        }
    }
    public void PlayMonkeyCatchSE()
    {
        if (monkeyCatchSE != null && monkeyCatchSE.clip != null && !mute)
        {
            monkeyCatchSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            monkeyCatchSE.PlayOneShot(monkeyCatchSE.clip);
        }
    }
    public void PlayMonkeyPerfectCatchSE()
    {
        if (monkeyPerfectCatchSE != null && monkeyPerfectCatchSE.clip != null && !mute)
        {
            monkeyPerfectCatchSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            monkeyPerfectCatchSE.PlayOneShot(monkeyPerfectCatchSE.clip);
        }
    }
    public void PlayMonkeyLandOnSurfaceSE()
    {
        if (monkeylandOnSurfaceSE != null && monkeylandOnSurfaceSE.clip != null && !mute)
        {
            monkeylandOnSurfaceSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            monkeylandOnSurfaceSE.PlayOneShot(monkeyThrowSE.clip);
        }
    }
    public void PlayMonkeyGrabLatticeSE()
    {
        if (monkeyGrabLatticeSE != null && monkeyGrabLatticeSE.clip != null && !mute)
        {
            monkeyGrabLatticeSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            monkeyGrabLatticeSE.PlayOneShot(monkeyGrabLatticeSE.clip);
        }
    }
    public void PlayMonkeyCallBallSE()
    {
        if (monkeyCallBallSE != null && monkeyCallBallSE.clip != null && !mute)
        {
            monkeyCallBallSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            monkeyCallBallSE.PlayOneShot(monkeyCallBallSE.clip);
        }
    }

    //Ball Sound Effects
    public void PlayBallBounceSoftSE()
    {
        if (ballBounceSoftSE != null && ballBounceSoftSE.clip != null && !mute)
        {
            ballBounceSoftSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            ballBounceSoftSE.PlayOneShot(ballBounceSoftSE.clip);
        }
    }
    public void PlayBallBounceHardSE()
    {
        if (ballBounceHardSE != null && ballBounceHardSE.clip != null && !mute)
        {
            ballBounceHardSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            ballBounceHardSE.PlayOneShot(ballBounceHardSE.clip);
        }
    }
    public void PlayBallInAirSE()
    {
        if (ballInAirSE != null && ballInAirSE.clip != null && !mute)
        {
            ballInAirSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            ballInAirSE.PlayOneShot(ballInAirSE.clip);
        }
    }
    public void PlayMonkeyCatchBananaSE()
    {
        if (monkeyCatchBananaSE != null && monkeyCatchBananaSE.clip != null && !mute)
        {
            monkeyCatchBananaSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            monkeyCatchBananaSE.PlayOneShot(monkeyCatchBananaSE.clip);
        }
    }

    public void PlayMonkeyCatchPoopSE()
    {
        if (monkeyCatchPoopSE != null && monkeyCatchPoopSE.clip != null && !mute)
        {
            monkeyCatchPoopSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            monkeyCatchPoopSE.PlayOneShot(monkeyCatchPoopSE.clip);
        }
    }
    public void PlayBananaHitSurfaceSE()
    {
        if (bananaHitSurfaceSE != null && bananaHitSurfaceSE.clip != null && !mute)
        {
            bananaHitSurfaceSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            bananaHitSurfaceSE.PlayOneShot(bananaHitSurfaceSE.clip);
        }
    }
    public void PlayPoopHitSurfaceSE()
    {
        if (poopHitSurfaceSE != null && poopHitSurfaceSE.clip != null && !mute)
        {
            poopHitSurfaceSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            poopHitSurfaceSE.PlayOneShot(poopHitSurfaceSE.clip);
        }
    }
    //Target Sound Effects
    public void PlayTargetPopupSE()
    {
        if (targetPopupSE != null && targetPopupSE.clip != null && !mute)
        {
            targetPopupSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            targetPopupSE.PlayOneShot(targetPopupSE.clip);
        }
    }
    public void PlayTargetRetractSE()
    {
        if (targetRetractSE != null && targetRetractSE.clip != null && !mute)
        {
            targetRetractSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            targetRetractSE.PlayOneShot(targetRetractSE.clip);
        }
    }
    public void PlayTargetUpgradeSE()
    {
        if (targetUpgradeSE != null && targetUpgradeSE.clip != null && !mute)
        {
            targetUpgradeSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            targetUpgradeSE.PlayOneShot(targetUpgradeSE.clip);
        }
    }
    public void PlayTargetDowngradeSE()
    {
        if (targetDowngradeSE != null && targetDowngradeSE.clip != null && !mute)
        {
            targetDowngradeSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            targetDowngradeSE.PlayOneShot(targetDowngradeSE.clip);
        }
    }
    public void PlayTargetHitSE()
    {
        if (targetHitSE != null && targetHitSE.clip != null && !mute)
        {
            targetHitSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            targetHitSE.pitch = Random.Range(.75f, 1f);
            targetHitSE.PlayOneShot(targetHitSE.clip);
        }
    }

    public void PlayBananaInBasketSE()
    {
        if (bananaInBasketSE != null && bananaInBasketSE.clip != null && !mute)
        {
            bananaInBasketSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            bananaInBasketSE.PlayOneShot(bananaInBasketSE.clip);
        }
    }

    //Gorilla Sound Effects
    public void PlayGorillaChargeupSE()
    {
        if (gorillaChargeupSE != null && gorillaChargeupSE.clip != null && !mute)
        {
            gorillaChargeupSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            gorillaChargeupSE.PlayOneShot(gorillaChargeupSE.clip);
        }
    }
    public void PlayGorillaTackleSE()
    {
        if (gorillaTackleSE != null && gorillaTackleSE.clip != null && !mute)
        {
            gorillaTackleSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            gorillaTackleSE.PlayOneShot(gorillaTackleSE.clip);
        }
    }
    public void PlayGorillaTackleSurfaceHitSE()
    {
        if (gorillaTackleHitSurfaceSE != null && gorillaTackleHitSurfaceSE.clip != null && !mute)
        {
            gorillaTackleHitMonkeySE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            gorillaTackleHitSurfaceSE.PlayOneShot(gorillaTackleHitSurfaceSE.clip);
        }
    }
    public void PlayGorillaTackleHitMonkeySE()
    {
        if (gorillaTackleHitMonkeySE != null && gorillaTackleHitMonkeySE.clip != null && !mute)
        {
            gorillaTackleHitMonkeySE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            gorillaTackleHitMonkeySE.PlayOneShot(gorillaTackleHitMonkeySE.clip);
        }
    }

    public void PlayGorillaInterceptionSE()
    {
        if (gorillaInterceptionSE != null && gorillaInterceptionSE.clip != null && !mute)
        {
            gorillaInterceptionSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            gorillaInterceptionSE.PlayOneShot(gorillaInterceptionSE.clip);
        }
    }

    public void PlayPregameSpinnerSE()
    {
        if (pregameSpinnerSE != null && pregameSpinnerSE.clip != null && !mute)
        {
            pregameSpinnerSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            pregameSpinnerSE.PlayOneShot(pregameSpinnerSE.clip);
        }
    }

    public void PlayShotClockBuzzSE()
    {
        if (shotClockBuzzSE != null && shotClockBuzzSE.clip != null && !mute)
        {
            shotClockBuzzSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            shotClockBuzzSE.PlayOneShot(shotClockBuzzSE.clip);
        }
    }
    // Menu Sound Effects
    public void PlayMenuButtonSE()
    {
        if (menuButtonSE != null && menuButtonSE.clip != null && !mute)
        {
            menuButtonSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            menuButtonSE.PlayOneShot(menuButtonSE.clip);
        }
    }

    public void PlayUnMenuButtonSE()
    {
        if (unMenuButtonSE != null && unMenuButtonSE.clip != null && !mute)
        {
            unMenuButtonSE.volume = GameManager.Instance.gmVolumeManager.SFXVolume;
            unMenuButtonSE.PlayOneShot(unMenuButtonSE.clip);
        }
    }

}
