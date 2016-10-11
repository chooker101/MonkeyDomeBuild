using UnityEngine;
using System.Collections;

public class ParticleReset : MonoBehaviour
{
    public float duration = 1;
    void OnEnable()
    {
        Invoke("Disable", duration);
    }
    void Disable()
    {
        gameObject.SetActive(false);
    }

}
