using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    public GameObject stuffToShoot;
    private Rigidbody projectile;
    private Transform myTransform;
    private Vector3 velocity;
    public Transform target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    void Awake()
    {
        myTransform = transform;
    }
    void Start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Fire();
        }
    }

    public void Fire()
    {
        // work perfect if no drag
        // kinda work with very little drag like around 0.05
        GameObject go = (GameObject)Instantiate(stuffToShoot, transform.position, transform.rotation);
        projectile = go.GetComponent<Rigidbody>();
        float target_Distance = Vector3.Distance(projectile.position, target.position);
        firingAngle = Random.Range(30, 60);
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
        float vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        if (transform.position.x > target.transform.position.x)
        {
            velocity = new Vector3(-vx, vy);
        }
        else
        {
            velocity = new Vector3(vx, vy);
        }
        projectile.velocity = velocity;
        Destroy(go, 4f);
    }
    public void SetTarget(GameObject tar)
    {
        target = tar.transform;
    }
    public void SetBullet(GameObject obj)
    {
        stuffToShoot = obj;
    }
}
