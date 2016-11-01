using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    private GameObject stuffToShoot;
    private Rigidbody2D projectile;
    private Vector3 velocity;
    private Transform target;
    private Vector3 fireLoc;
    private Rigidbody m_rigid;
    private float firingAngle;

    private float gravity;
    private float projectileExistTime = 12f; // in sec

    private bool canFire = false;
    private bool isFiring = false;
    private float timeForNext = 0f;

    //private float currentMinFiringAngle;
    //private float currentMaxFiringAngle;
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
        firingAngleRange[0] = 40f;
        firingAngleRange[1] = 70f;
        fireLoc = Vector2.zero;
        fireLoc.x = transform.position.x;
        m_rigid = GetComponent<Rigidbody>();
        maxFiringAngle = 70f;
        minFiringAngle = 30f;
        angleRatio = 1f;
        distanceFalloff = 2f;
        coolDownMin = .2f;   // was 2f
        coolDownMax = .5f;   // was 5f
        falloff = 0.5f;
        moveSpeed = 20f;    // was 10f
    }
    void Start()
    {
        GameManager.Instance.gmTurretManager.AddTurret(this);
    }
    void Update()
    {
        if (GetComponent<SpriteRenderer>().flipX)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, firingAngle), 1f * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, -firingAngle), 1f * Time.deltaTime);
        }

        if (canFire)
        {
            MoveToFireLocation();
            if (CheckIfReadyToFire())
            {
                Fire();
                canFire = false;
                isFiring = false;
                firingAngle = -25f;
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
        //currentMinFiringAngle = minAng;
        //currentMaxFiringAngle = maxAng;

        //Quaternion.EulerRotation(0f,GetFiringAngle(),0f);
        //transform.rotation = Quaternion.EulerRotation(0f, 0f, GetFiringAngle());
    }

    public void OpenFire(TargetQueue targetQueue)
    {
        canFire = true;
        isFiring = true;
        isAvailable = false;
        target = targetQueue.GetTarget().transform;
        stuffToShoot = targetQueue.GetWhatToFire();
        firingAngle = GetFiringAngle();
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
        //float firingAngle;
        //firingAngle = GetFiringAngle();
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
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, firingAngle), 1f * Time.deltaTime);
        if (target.transform.position.y > m_rigid.position.y)
        {
            Vector2 targetLoc = m_rigid.transform.position + Vector3.up * moveSpeed * Time.deltaTime;
            m_rigid.MovePosition(targetLoc);
        }
        else if (target.transform.position.y < m_rigid.position.y)
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
        float minAng = firingAngleRange[0] - target.position.y * angleRatio;
        minAng = Mathf.Max(minAng, minFiringAngle);
        float maxAng = firingAngleRange[1] - target.position.y * angleRatio;
        maxAng = Mathf.Min(maxAng, maxFiringAngle);
        return Random.Range(minAng, maxAng);
    }
}
