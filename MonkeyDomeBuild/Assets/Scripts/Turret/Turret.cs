using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    private GameObject stuffToShoot;
    private Rigidbody2D projectile;
    private Transform myTransform;
    private Vector3 velocity;
    private Transform target;
    private Vector3 fireLoc;
    private Rigidbody m_rigid;

    private float gravity;
    private float projectileExistTime = 12f; // in sec

    private bool canFire = false;
    private bool isFiring = false;
    private float timeForNext = 0f;

    [SerializeField]
    private float currentMinFiringAngle;
    [SerializeField]
    private float currentMaxFiringAngle;
    [SerializeField]
    private bool isAvailable = true;

    public float[] firingAngleRange = new float[2]; // init fire angle range
    public float minFiringAngle;
    public float maxFiringAngle;
    public float angleRatio;
    public float falloff; // after move within +/- falloff of target y position, open fire
    public float moveSpeed;
    public float coolDownMin;
    public float coolDownMax;
    public float distanceFalloff; // range from -distanceFalloff to +distanceFalloff

    void Awake()
    {
        gravity = -Physics2D.gravity.y;
        myTransform = transform;
        firingAngleRange[0] = 40f;
        firingAngleRange[1] = 70f;
        fireLoc = Vector2.zero;
        fireLoc.x = transform.position.x;
        m_rigid = GetComponent<Rigidbody>();
        maxFiringAngle = 70f;
        minFiringAngle = 30f;
        angleRatio = 1f;
        distanceFalloff = 2f;
        coolDownMin = 2f;
        coolDownMax = 5f;
        falloff = 0.5f;
        moveSpeed = 10f;
    }

    void Update()
    {
        if (canFire)
        {
            MoveToFireLocation();
            if (CheckIfReadyToFire())
            {
                Fire();
                canFire = false;
                isFiring = false;
            }
        }
        if (!isAvailable && !isFiring)
        {
            if(Time.time >= timeForNext)
            {
                isAvailable = true;
            }
        }
        float minAng = firingAngleRange[0] - m_rigid.transform.position.y * angleRatio;
        minAng = Mathf.Max(minAng, minFiringAngle);
        float maxAng = firingAngleRange[1] - m_rigid.transform.position.y * angleRatio;
        maxAng = Mathf.Min(maxAng, maxFiringAngle);
        currentMinFiringAngle = minAng;
        currentMaxFiringAngle = maxAng;
    }

    public void OpenFire(TargetQueue targetQueue)
    {
        canFire = true;
        isFiring = true;
        isAvailable = false;
        target = targetQueue.GetTarget().transform;
        stuffToShoot = targetQueue.GetWhatToFire();
    }

    private void Fire()
    {
        //Debug.Log("fire");
        // work perfect if no drag
        // kinda work with very little drag like around 0.05
        if (target == null) return;

        GameObject go = (GameObject)Instantiate(stuffToShoot, transform.position, transform.rotation);
        projectile = go.GetComponent<Rigidbody2D>();
        float drag = projectile.drag;
        float target_Distance = Vector2.Distance(projectile.position, target.position);
        target_Distance += Random.Range(-distanceFalloff, distanceFalloff);
        float firingAngle;
        firingAngle = GetFiringAngle();
        //Debug.Log(firingAngle);
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
        float vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        vx *= (1 + drag);
        vy *= (1 + drag);
        if (transform.position.x > target.transform.position.x)
        {
            velocity = new Vector2(-vx, vy);
        }
        else
        {
            velocity = new Vector2(vx, vy);
        }
        projectile.velocity = velocity;
        Destroy(go, projectileExistTime);
        isAvailable = false;
        timeForNext = Time.time + Random.Range(coolDownMin,coolDownMax);
    }

    private void MoveToFireLocation()
    {
        if (target.transform.position.y > m_rigid.position.y)
        {
			Vector2 targetLoc = m_rigid.transform.position + Vector3.up * moveSpeed * Time.deltaTime;
            m_rigid.MovePosition(targetLoc);
        }
        else if(target.transform.position.y < m_rigid.position.y)
        {
			Vector2 targetLoc = m_rigid.transform.position + Vector3.down * moveSpeed * Time.deltaTime;
            m_rigid.MovePosition(targetLoc);
        }
    }

    private bool CheckIfReadyToFire()
    {
        if (Mathf.Abs(m_rigid.transform.position.y - target.transform.position.y) < falloff)
        {
            return true;
        }
        return false;
    }

    public bool IsAvailable()
    {
        return isAvailable;
    }

    private float GetFiringAngle()
    {
        float minAng = firingAngleRange[0] - m_rigid.transform.position.y * angleRatio;
        minAng = Mathf.Max(minAng, minFiringAngle);
        float maxAng = firingAngleRange[1] - m_rigid.transform.position.y * angleRatio;
        maxAng = Mathf.Min(maxAng, maxFiringAngle);
        return Random.Range(minAng, maxAng);
    }
}
