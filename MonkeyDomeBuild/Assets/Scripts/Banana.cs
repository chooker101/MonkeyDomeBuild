using UnityEngine;
using System.Collections;

public class Banana : MonoBehaviour {

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Vine"))
        {
            //Debug.Log(transform.position.x);
        }
    }
}
