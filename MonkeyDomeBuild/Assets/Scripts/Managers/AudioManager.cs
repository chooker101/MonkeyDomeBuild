using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {


    public AudioClip[] tracks;

    private AudioSource audioSource;
    private float introLength;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        // ******* we will determine our audio clip based on the index of the currently loaded scene 
        // ******* given pregame room is index 0, load 	"MD_Pregame_Loop"
        // ******* given pregame room is index 1, load 	"MD_Gameplay_Loop"
        // ******* additionally, figure out how to play "MD_Gameplay_Intro" on first play of "MD_Gameplay_Loop"
        PlayAudioTracks();

    }

    // Update is called once per frame
    void Update () {
	    
	}

    int CheckLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    void SetAudioTrack(int sceneIndex)
    {
        audioSource.clip = tracks[sceneIndex];
        CheckIntro();
    }

   void CheckIntro()
    {
        if(audioSource.clip.name == "MD_Gameplay_Intro")
        {
            audioSource.loop = false;
            introLength = audioSource.clip.length;
            Invoke("IncreaseIndex",introLength);
        } else
        {
            audioSource.loop = true;
        }
    }
    void IncreaseIndex()
    {
        SetAudioTrack(CheckLevel() + 1);
    }

    public void PlayAudioTracks()
    {
        SetAudioTrack(CheckLevel());
        audioSource.volume = GameManager.Instance.gmVolumeManager.musicVolume;
        audioSource.Play();

    }
}
