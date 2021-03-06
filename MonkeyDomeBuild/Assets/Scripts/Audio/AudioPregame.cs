﻿using UnityEngine;
using System.Collections;

public class AudioPregame : MonoBehaviour {

    public AudioClip pregameLoop;
    public AudioSource levelAudio;

	// Use this for initialization
	void Awake () {
        levelAudio = GetComponent<AudioSource>();
        levelAudio.loop = true;
        levelAudio.clip = pregameLoop;
        levelAudio.Play();
	}
    void Update()
    {
        levelAudio.volume = GameManager.Instance.gmVolumeManager.musicVolume;
    }
	
}
