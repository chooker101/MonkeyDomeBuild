using UnityEngine;
using System.Collections;

public class Turret2 : MonoBehaviour
{
    public GameObject stuffToShoot;
    private Rigidbody projectile;
    private Transform myTransform;
    Vector3 launchAngle;
    public Vector3 velocity;

    void Awake()
    {
        myTransform = transform;
    }
    void Start()
    {
        launchAngle = new Vector3(15f, 15f, 0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject go = (GameObject)Instantiate(stuffToShoot, transform.position, transform.rotation);
            projectile = go.GetComponent<Rigidbody>();
            projectile.velocity = velocity;
            Destroy(go, 5f);

        }
    }
}
