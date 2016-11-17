using UnityEngine;
using System.Collections;

public class VictoryMusic : MonoBehaviour {

    public AudioSource start;
    public AudioSource loop;
    public AudioSource currentAudio;

	// Use this for initialization
	void Start () {
        currentAudio = start;
        currentAudio.Play();
        StartCoroutine(PlayStartTrack());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator PlayStartTrack()
    {
        yield return new WaitForSeconds(currentAudio.clip.length);

        if (currentAudio == start)
        {
            currentAudio = loop;
        }
        currentAudio.Play();
        StartCoroutine(PlayStartTrack());

    }
}
