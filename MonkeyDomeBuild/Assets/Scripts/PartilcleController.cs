using UnityEngine;
using System.Collections;

public class PartilcleController : MonoBehaviour
{
    //private int playerIndex = 0;

    //private Color playerColor;
    public ParticleSystem testParticle;
    private Transform catchParticlePivot;
    
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //catchParticlePivot.GetComponentInChildren<ParticleSystem>().Play();
            //testParticle.Play();

        }
    }
    public Transform CatchEffect
    {
        get
        {
            if (catchParticlePivot == null)
            {
                catchParticlePivot = transform.FindChild("CatchParticle");
            }
            return catchParticlePivot;
        }
    }
}
