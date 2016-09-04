using UnityEngine;
using System.Collections;

public class BananaReset : MonoBehaviour
{
    public float resetY = -50f;

    void Update()
    {
        if (transform.position.y < resetY)
        {
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
