using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class VolumeManager : MonoBehaviour {

    public Slider musicSlider;
    public Slider SFXSlider;


    [Range(0,1)]
    public float musicVolume;
    [Range(0, 1)]
    public float SFXVolume;

	// Use this for initialization
	void Start () {
        musicVolume = musicSlider.value;
        SFXVolume = SFXSlider.value;

	}
	
	// Update is called once per frame
	void Update () {
        musicVolume = musicSlider.value;
        SFXVolume = SFXSlider.value;
	}
}
