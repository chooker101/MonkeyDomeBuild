using UnityEngine;
using System.Collections;

public class BananaReset : MonoBehaviour
{
    public float resetY = -50f;

    void Update()
    {
        if (transform.position.y < resetY)
        {
            gameObject.SetActive(false);
        }
    }
}
