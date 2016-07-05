using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour {

    public float moveForce;
    public float speedLimit;
    private Rigidbody m_rigid;
    public Vector3 mov;

	void Start ()
    {
        moveForce = 20f;
        speedLimit = 8f;
        m_rigid = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
        bool mLeft = Input.GetKey(KeyCode.A);
        bool mRight = Input.GetKey(KeyCode.D);
        Vector3 movement = new Vector3();
        if (mLeft && Mathf.Abs(m_rigid.velocity.x) < speedLimit)
        {
            movement.x = -moveForce;
        }
        else if (mRight && Mathf.Abs(m_rigid.velocity.x) < speedLimit)
        {
            movement.x = moveForce;
        }
        m_rigid.AddForce(movement);
        mov = m_rigid.velocity;
	}
}
