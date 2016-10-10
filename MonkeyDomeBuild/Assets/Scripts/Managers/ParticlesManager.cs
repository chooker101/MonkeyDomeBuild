using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticlesManager : MonoBehaviour
{
    private static ParticlesManager myInstance;
    public GameObject jumpParticlePrefab;
    private List<GameObject> pooledJumpParticles = new List<GameObject>();

    void Awake()
    {
        int jumpParticlePooledAmount = 10;
        for(int i = 0; i < jumpParticlePooledAmount; i++)
        {
            pooledJumpParticles.Add(Instantiate(jumpParticlePrefab));
            pooledJumpParticles[i].transform.SetParent(transform);
            pooledJumpParticles[i].SetActive(false);
        }
    }
    public static ParticlesManager Instance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<ParticlesManager>();
            }
            return myInstance;
        }
    }
    public GameObject JumpParticle
    {
        get
        {
            for(int i = 0; i < pooledJumpParticles.Count; i++)
            {
                if (!pooledJumpParticles[i].activeInHierarchy)
                {
                    return pooledJumpParticles[i];
                }
            }
            return null;
        }
    }

}
