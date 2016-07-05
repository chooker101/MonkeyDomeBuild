using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public GameObject player2;
    public float smoothing = 3.0f;

    private Vector3 offset;
    private Vector3 directionCtoA;
    private Vector3 directionCtoB;
    private Vector3 midpointAtoB;
    private float initY;
    private float initX;
    //private float playerDist;


    // Use this for initialization
    void Start () {
        initY = transform.position.y;
        initX = transform.position.x;
        //playerDist = Vector3.Distance(player.transform.position, player2.transform.position);
        GetMidPos();

        offset = transform.position - midpointAtoB;

        transform.position= player.transform.position + offset;

    }
	
	// Update is called once per frame
	void LateUpdate () {

        if (player != null && player2 != null)
        {
            GetMidPos();
            Vector3 cameraFollow = midpointAtoB + offset;
            if (transform.position.y <= initY)
            {
                transform.position.Set(transform.position.x, initY, transform.position.z);
            }
            transform.position = Vector3.Lerp(transform.position, cameraFollow, smoothing * Time.deltaTime);
        }
        else if(player != null && player2 == null)
        {
            Vector3 cameraFollow = player.transform.position;
            transform.position = Vector3.Lerp(transform.position, cameraFollow, smoothing * Time.deltaTime);
        }


    }

    void GetMidPos() // temporarily, y is 1, use this later: (directionCtoA.y + directionCtoB.y) / 2.0f
    {
        directionCtoA = player.transform.position - transform.position; // directionCtoA = positionA - positionC
        directionCtoB = player2.transform.position - transform.position; // directionCtoB = positionB - positionC
        midpointAtoB = new Vector3((directionCtoA.x + directionCtoB.x) / 
            2.0f, (1.0f), (directionCtoA.z + directionCtoB.z) / 2.0f); // midpoint between A B this is what you want
    }
}
