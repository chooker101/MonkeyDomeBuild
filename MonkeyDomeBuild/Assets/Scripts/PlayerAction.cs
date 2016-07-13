using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAction : Player
{
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
            StompCheck();
        }
        mov = m_rigid.velocity;
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isGorilla)
            {
                GorillaToMonkey();
            }
            else
            {
                MonkeyToGorilla();
            }
        }
    }


}
