using UnityEngine;
using UnityEngine.UI;


public class VolumeManager : MonoBehaviour
{

    public Slider musicSlider;
    public Slider SFXSlider;

    private GameObject audioHolder;

    [Range(0, 1)]
    public float musicVolume;
    [Range(0, 1)]
    public float SFXVolume;

    private float prevMusicVolume;
    private float prevSFXVolume;

    public float loadedSFX;
    public float loadedMusic;



    public void Start()
    {
        AudioEffectManager SFX = GameManager.Instance.gmAudioEffectManager;

        //Adds a listener to the main slider and invokes a method when the value changes.
        musicSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        SFXSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        audioHolder = GameObject.Find("AudioHolder");

        
            SFXVolume = PlayerPrefs.GetFloat("SFXVolume");

        loadedSFX = PlayerPrefs.GetFloat("SFXVolume");

      
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");

        loadedMusic = PlayerPrefs.GetFloat("MusicVolume"); 

   

        musicSlider.value = musicVolume;
        SFXSlider.value = loadedSFX;

        prevMusicVolume = musicVolume;
        prevSFXVolume = SFXVolume;
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {

        AudioMain game = audioHolder.GetComponent<AudioMain>();
        AudioPregame pregame = audioHolder.GetComponent<AudioPregame>();


        musicVolume = musicSlider.value;
        SFXVolume = SFXSlider.value;

        if (SFXVolume != prevSFXVolume)
        {
            GameManager.Instance.gmAudioEffectManager.monkeyJumpSE.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.monkeyThrowSE.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.monkeyCatchSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.monkeyPerfectCatchSE.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.monkeylandOnSurfaceSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.monkeyGrabLatticeSE.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.ballBounceSoftSE.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.ballBounceHardSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.ballInAirSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.monkeyCatchBananaSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.monkeyCatchPoopSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.bananaHitSurfaceSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.poopHitSurfaceSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.targetPopupSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.targetRetractSE.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.targetUpgradeSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.targetDowngradeSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.targetHitSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.bananaInBasketSE.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.monkeyCallBallSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.gorillaChargeupSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.gorillaTackleSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.gorrila
            //GameManager.Instance.gmAudioEffectManager.gorillaTackleHitMonkeySE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.gorillaInterceptionSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.pregameSpinnerSE.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.shotClockBuzzSE.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.menuButtonSE.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.unMenuButtonSE.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.audienceCheer1.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.audienceCheer2.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.audienceCheer3.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.audienceBoo1.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.audienceBoo2.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.audienceBoo3.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.audienceCatch.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.audienceInterception.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.audienceShotClock.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.audienceTargetUp.volume = SFXVolume;
            GameManager.Instance.gmAudioEffectManager.audienceSmash.volume = SFXVolume;
            //GameManager.Instance.gmAudioEffectManager.audiencePoop.volume = SFXVolume;
            PlayerPrefs.SetFloat("SFXVolume", SFXVolume);

        }


        else if (musicVolume != prevMusicVolume)
        {

            if (pregame != null)
                pregame.levelAudio.volume = musicVolume;

            if (game != null)
            {
                game.currentAudio.volume = musicVolume;
                game.levelAudio.volume = musicVolume;
                game.startAudio.volume = musicVolume;
                game.startAudio2.volume = musicVolume;
            }
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        }


        prevMusicVolume = musicVolume;
        prevSFXVolume = SFXVolume;


    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
        //Debug.Log(musicVolume);
        //Debug.Log(SFXVolume);
    }
}
