using UnityEngine;
using System.Collections;

public class Banana : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Vine"))
        {
            //Debug.Log(transform.position.x);
        }
    }
}
