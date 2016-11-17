using UnityEngine;
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

    private AudioSource currentAudio;
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
        yield return new WaitForSeconds(currentAudio.clip.length);

        if (currentAudio == startAudio)
        {
            currentAudio = levelAudio;
        }
        else if (currentAudio == levelAudio)
        {
            currentAudio = startAudio2;
        } else if (currentAudio == startAudio2)
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
}
