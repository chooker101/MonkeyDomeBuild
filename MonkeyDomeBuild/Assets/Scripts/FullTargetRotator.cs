using UnityEngine;
using System.Collections;

public class FullTargetRotator : MonoBehaviour {

    public float rotateSpeed;

    public GameObject targetBase;
    public bool targetDown = false;
    public float angleChange = 0f;

    public Vector3 rotateAxis;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.O))
        {
            targetDown = true;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            targetDown = false;
        }
        if (targetDown)
        {
            if (angleChange < 90f)
            {
                RotateTarget(true);
                Debug.Log(targetBase.transform.rotation.eulerAngles.x);
                // disable target
            }
        }
        if (!targetDown)
        {
            if (angleChange > 0f)
            {
                RotateTarget(false);
                Debug.Log(targetBase.transform.rotation.eulerAngles.x);
                // enable target
            }
        }
    }

    void RotateTarget(bool dir)
    {
        if (dir)
        {
            this.gameObject.transform.RotateAround(targetBase.transform.position, rotateAxis,  -(rotateSpeed * Time.deltaTime));
            angleChange += (rotateSpeed * Time.deltaTime);
        } else
        {
            this.gameObject.transform.RotateAround(targetBase.transform.position, rotateAxis,  (rotateSpeed * Time.deltaTime));
            angleChange -= (rotateSpeed * Time.deltaTime);
        }

        
    }
}
