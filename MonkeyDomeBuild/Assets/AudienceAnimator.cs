using UnityEngine;
using System.Collections;

public class AudienceAnimator : MonoBehaviour {


    private EventManager eventManager;
    private Animation audienceAnimation;

    public AnimationClip happy;
    public AnimationClip angry;
    public AnimationClip neutral;

    private Animator controller;

	// Use this for initialization
	void Start () {

        audienceAnimation = GetComponent<Animation>();
        audienceAnimation.clip = neutral;

        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
