using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAction : Player
{




    
    // Player Stats


    void Start()
    {
        //whichPlayer = 0;
        moveForce = 100f;
        jumpForce = 65f;
        speedLimit = 12f;
        throwForce = 40f;
        downForce = 80f;
        tempDownForce = downForce;
        downForceIncrement = 100f; // per second
        maxDownForce = 200f;
        climbForce = 200f; ;
        climbSpeedLimit = speedLimit;
        m_rigid = GetComponent<Rigidbody>();
        normalDrag = m_rigid.drag;
        climbDrag = 12f;
        layerMask = 1 << LayerMask.NameToLayer("Floor");
        
    }

    void FixedUpdate()
    {
        CheckInputs();
        Movement();
        JumpCheck();
        Aim();
        if (!isGorilla)
        {
            if (haveBall)
            {
                ThrowCheck();
            }
            else
            {
                CatchCheck();
            }
        }
        else
        {
            CatchCheck();
        }

        mov = m_rigid.velocity;
    }


}
