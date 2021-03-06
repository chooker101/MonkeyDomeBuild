﻿using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(AudioSource))]
public class AudioMain : MonoBehaviour
{
    public AudioClip startTrack;
    public AudioClip mainLoop;

    public AudioSource levelAudio2;
    public AudioSource levelAudio;
    public AudioSource startAudio;
    public AudioSource startAudio2;

    public AudioSource currentAudio;
    // Use this for initialization
    void Start()
    {
        currentAudio = startAudio;
        //levelAudio = GetComponent<AudioSource>();
        //levelAudio.loop = true; ;
        currentAudio.Play();
        StartCoroutine(PlayStartTrack());
    }

    IEnumerator PlayStartTrack()
    {
        //currentAudio.clip = startTrack;
        //currentAudio.Play();

        yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(currentAudio.clip.length));
        // yield return new WaitForSeconds(currentAudio.clip.length * Time.unscaledDeltaTime);

        if (currentAudio == startAudio)
        {
            currentAudio = levelAudio;
        }
        else if (currentAudio == levelAudio)
        {
            currentAudio = startAudio2;
        }
        else if (currentAudio == startAudio2)
        {
            currentAudio = levelAudio2;
        }
        else
        {
            currentAudio = startAudio;
        }

        currentAudio.Play();
        StartCoroutine(PlayStartTrack());
    }

    void Update()
    {
        //can't change music volume from VolumeManager
        levelAudio2.volume = GameManager.Instance.gmVolumeManager.musicVolume;
        levelAudio.volume = GameManager.Instance.gmVolumeManager.musicVolume;
        startAudio.volume = GameManager.Instance.gmVolumeManager.musicVolume;
        startAudio2.volume = GameManager.Instance.gmVolumeManager.musicVolume;
    }

    public static class CoroutineUtil
    {
        public static IEnumerator WaitForRealSeconds(float time)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + time)
            {
                yield return null;
            }
        }
    }
    
}
