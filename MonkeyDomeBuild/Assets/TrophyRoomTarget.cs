using UnityEngine;
using System.Collections;

public class TrophyRoomTarget : MonoBehaviour
{
    public float hitSubtractTime = 0;
    public Transform hitParticlePivot;

    private VictoryRoomManager script_victoryManager;
    private GameObject objectHit = null;

    // Use this for initialization
    void Start()
    {
        script_victoryManager = FindObjectOfType<VictoryRoomManager>().gameObject.GetComponent<VictoryRoomManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball") && other.gameObject != null)
        {
            if(script_victoryManager.victoryTimer > 0)
            {
                script_victoryManager.victoryTimer -= hitSubtractTime;

                objectHit = other.GetComponentInParent<BallInfo>().gameObject;
                // Particle stuff
                GameObject particle = ParticlesManager.Instance.TargetHitParticle;
                particle.SetActive(true);
                particle.transform.position = hitParticlePivot.position;

                Vector3 ballPos = other.transform.position;
                ballPos.z = 0;
                Vector3 pivotPos = hitParticlePivot.position;
                pivotPos.z = 0;
                Quaternion targetAng = Quaternion.FromToRotation(Vector3.right, (ballPos - pivotPos).normalized);
                particle.transform.rotation = Quaternion.Euler(0, 0, targetAng.eulerAngles.z);
                particle.transform.Find("p1").localScale = transform.localScale;
                particle.transform.Find("p1").transform.FindChild("p2").localScale = transform.localScale;
                particle.transform.Find("p1").transform.FindChild("p3").localScale = transform.localScale;
                particle.GetComponentInChildren<ParticleSystem>().Play();
            }
        }
    }
}
