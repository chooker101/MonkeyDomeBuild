using UnityEngine;
using System.Collections;

public class AudienceAnimator : MonoBehaviour
{
    //private EventManager eventManager;
    private Animation audienceAnimation;

    //public AnimationClip happy;
    //public AnimationClip angry;
    ////public AnimationClip neutral;
    public float animationLength;
    public Animator controller;

    // Use this for initialization
    void Start ()
	{
		GameManager.Instance.gmAudienceAnimator = this;
        audienceAnimation = GetComponent<Animation>();
        //audienceAnimation.clip = neutral;
        controller.SetBool("Happy", false);
        controller.SetBool("Angry", false);
    }

    public void AudienceHappy()
    {
        controller.SetBool("Happy", true);
        //Debug.Log("Happy");
        Invoke("ResetAudience", animationLength);
    }

    public void AudienceAngry()
    {
        controller.SetBool("Angry", true);
        //Debug.Log("Angry");
        Invoke("ResetAudience", animationLength);
    }

    protected void ResetAudience()
    {
        controller.SetBool("Angry", false);

        controller.SetBool("Happy", false);
    }

    // Update is called once per frame
    void Update ()
	{
	
	}
}
