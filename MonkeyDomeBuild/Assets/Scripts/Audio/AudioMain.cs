using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(AudioSource))]
public class AudioMain : MonoBehaviour
{

    public AudioClip startTrack;
    public AudioClip mainLoop;
    private AudioSource levelAudio;

    // Use this for initialization
    void Start()
    {
        levelAudio = GetComponent<AudioSource>();
        levelAudio.loop = true; ;

        StartCoroutine(PlayStartTrack());
    }

    IEnumerator PlayStartTrack()
    {
        levelAudio.clip = startTrack;
        levelAudio.Play();
        yield return new WaitForSeconds(levelAudio.clip.length);
        levelAudio.clip = mainLoop;
        levelAudio.Play();

    }
}
