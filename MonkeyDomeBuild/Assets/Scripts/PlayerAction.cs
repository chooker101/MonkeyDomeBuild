﻿using UnityEngine;
using System.Collections;

public class PlayerAction : Player {

    public float moveForce;
    public float speedLimit;
    private Rigidbody m_rigid;
    public Vector3 mov;
    private bool canJump = true;
    private float jumpForce;
    private int layerMask;
    private bool ballInRange = false;

    void Start ()
    {
        moveForce = 40f;
        jumpForce = 5f;
        speedLimit = 8f;
        m_rigid = GetComponent<Rigidbody>();
        layerMask = 1 << LayerMask.NameToLayer("Floor");
    }
	
	void FixedUpdate ()
    {
        bool mLeft = Input.GetKey(KeyCode.A);
        bool mRight = Input.GetKey(KeyCode.D);
        Vector3 movement = new Vector3();
        if (mLeft && Mathf.Abs(m_rigid.velocity.x) < speedLimit && !RayCastSide(-1))
        {
            movement.x = -moveForce;
        }
        else if (mRight && Mathf.Abs(m_rigid.velocity.x) < speedLimit && !RayCastSide(1))
        {
            movement.x = moveForce;
        }
        m_rigid.AddForce(movement);
        JumpCheck();
        mov = m_rigid.velocity;
	}
    private void JumpCheck()
    {
        bool mJump = Input.GetKey(KeyCode.Space);

        if (RayCast(-1))
        {
            canJump = true;
        }
        else
        {
            m_rigid.AddForce(new Vector3(0f, -10f));
        }
        if (mJump && canJump)
        {
            canJump = false;
            m_rigid.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }
    private void CatchCheck()
    {
        bool mCatch = Input.GetKey(KeyCode.F);
        if (mCatch)
        {

        }

    }
    bool RayCast(int direction)
    {
        bool hit = Physics.Raycast(m_rigid.position, direction * Vector3.up, 0.5f + 0.1f, layerMask);
        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool RayCastSide(int leftOrRight)
    {
        bool hit = Physics.Raycast(m_rigid.position, leftOrRight * Vector3.right, 0.5f + 0.1f, layerMask);
        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ballInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ballInRange = false;
        }
    }
}
