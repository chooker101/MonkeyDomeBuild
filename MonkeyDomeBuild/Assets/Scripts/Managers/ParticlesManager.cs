using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticlesManager : MonoBehaviour
{
    private static ParticlesManager myInstance;
    public GameObject targetHitEffect;
    private List<GameObject> pooledTargetHitEffect = new List<GameObject>();

    void Awake()
    {
        int pooledAmount = 10;
        for(int i = 0; i < pooledAmount; i++)
        {
            pooledTargetHitEffect.Add(Instantiate(targetHitEffect));
            pooledTargetHitEffect[i].transform.SetParent(transform);
            pooledTargetHitEffect[i].SetActive(false);
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
            for(int i = 0; i < pooledTargetHitEffect.Count; i++)
            {
                if (!pooledTargetHitEffect[i].activeInHierarchy)
                {
                    return pooledTargetHitEffect[i];
                }
            }
            return null;
        }
    }
    public GameObject TargetHitParticle
    {
        get
        {
            for (int i = 0; i < pooledTargetHitEffect.Count; i++)
            {
                if (!pooledTargetHitEffect[i].activeInHierarchy)
                {
                    return pooledTargetHitEffect[i];
                }
            }
            return null;
        }
    }

}
